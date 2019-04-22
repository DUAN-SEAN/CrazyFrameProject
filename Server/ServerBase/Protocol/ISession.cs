using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Crazy.Common;
namespace Crazy.ServerBase
{
    public interface ISession
    {
        void Reply(IResponse message);
        void Send(IMessage message);
        void Send(MemoryStream message);
        void Send(byte flag, IMessage message);
        void Send(byte flag, ushort opcode, object message);

        Task<IResponse> Call(IRequest request);
        Task<IResponse> Call(IRequest request, CancellationToken cancellationToken);
    }

    public class Session :ISession
    {
        private static int RpcId { get; set; }

        private readonly Dictionary<int, Action<IResponse>> requestCallback = new Dictionary<int, Action<IResponse>>();
        private readonly List<byte[]> byteses = new List<byte[]>() { new byte[1], new byte[2] };

        public NetworkComponent Network
        {
            get
            {
                return this.GetParent<NetworkComponent>();
            }
        }

        public int Error
        {
            get
            {
                return this.channel.Error;
            }
            set
            {
                this.channel.Error = value;
            }
        }

        public  void Dispose()
        {
            

            foreach (Action<IResponse> action in this.requestCallback.Values.ToArray())
            {
                action.Invoke(new ResponseMessage { Error = this.Error });
            }

            //int error = this.channel.Error;
            //if (this.channel.Error != 0)
            //{
            //	Log.Trace($"session dispose: {this.Id} ErrorCode: {error}, please see ErrorCode.cs!");
            //}
            this.requestCallback.Clear();
        }

        public void Start()
        {
            this.channel.Start();
        }

     

        public MemoryStream Stream
        {
            get
            {
                return this.channel.Stream;
            }
        }
   

        private void Run(MemoryStream memoryStream)
        {
            memoryStream.Seek(Packet.MessageIndex, SeekOrigin.Begin);
            byte flag = memoryStream.GetBuffer()[Packet.FlagIndex];
            ushort opcode = BitConverter.ToUInt16(memoryStream.GetBuffer(), Packet.OpcodeIndex);


            //if (OpcodeHelper.IsClientHotfixMessage(opcode))
            //{
            //    this.GetComponent<SessionCallbackComponent>().MessageCallback.Invoke(this, flag, opcode, memoryStream);
            //    return;
            //}

            object message;
            try
            {
                OpcodeTypeComponent opcodeTypeComponent = this.Network.Entity.GetComponent<OpcodeTypeComponent>();
                object instance = opcodeTypeComponent.GetInstance(opcode);
                //Log.Info("开始解析数据！");

                //message = this.Network.MessagePacker.DeserializeFrom(instance, memoryStream);
                message = ((Google.Protobuf.IMessage)instance).Descriptor.Parser.ParseFrom(memoryStream);

            }
            catch (Exception e)
            {
                // 出现任何消息解析异常都要断开Session，防止客户端伪造消息
                Log.Error($"opcode: {opcode} {this.Network.Count} {e} ");
                this.Error = ErrorCode.ERR_PacketParserError;
                this.Network.Remove(this.Id);
                return;
            }

            // flag第一位为1表示这是rpc返回消息,否则交由MessageDispatcher分发
            if ((flag & 0x01) == 0)
            {
                // Log.Debug("一个消息来了++++" + opcode.ToString()+message.GetType());
                this.Network.MessageDispatcher.Dispatch(this, opcode, message);
                return;
            }

            IResponse response = message as IResponse;
            if (response == null)
            {
                throw new Exception($"flag is response, but message is not! {opcode}");
            }
            Action<IResponse> action;
            if (!this.requestCallback.TryGetValue(response.RpcId, out action))
            {
                return;
            }
            this.requestCallback.Remove(response.RpcId);

            action(response);
        }

        public Task<IResponse> Call(IRequest request)
        {
            int rpcId = ++RpcId;
            var tcs = new TaskCompletionSource<IResponse>();

            this.requestCallback[rpcId] = (response) =>
            {
                try
                {
                    if (ErrorCode.IsRpcNeedThrowException(response.Error))
                    {
                        throw new RpcException(response.Error, response.Message);
                    }

                    tcs.SetResult(response);
                }
                catch (Exception e)
                {
                    tcs.SetException(new Exception($"Rpc Error: {request.GetType().FullName}", e));
                }
            };

            request.RpcId = rpcId;
            this.Send(0x00, request);
            return tcs.Task;
        }

        public Task<IResponse> Call(IRequest request, CancellationToken cancellationToken)
        {
            int rpcId = ++RpcId;
            var tcs = new TaskCompletionSource<IResponse>();

            this.requestCallback[rpcId] = (response) =>
            {
                try
                {
                    if (ErrorCode.IsRpcNeedThrowException(response.Error))
                    {
                        throw new RpcException(response.Error, response.Message);
                    }

                    tcs.SetResult(response);
                }
                catch (Exception e)
                {
                    tcs.SetException(new Exception($"Rpc Error: {request.GetType().FullName}", e));
                }
            };

            cancellationToken.Register(() => this.requestCallback.Remove(rpcId));

            request.RpcId = rpcId;
            this.Send(0x00, request);
            return tcs.Task;
        }

        public void Send(IMessage message)
        {
            this.Send(0x00, message);
        }

        public void Reply(IResponse message)
        {
           

            this.Send(0x01, message);
        }

        public void Send(byte flag, IMessage message)
        {
            OpcodeTypeComponent opcodeTypeComponent = this.Network.Entity.GetComponent<OpcodeTypeComponent>();
            ushort opcode = opcodeTypeComponent.GetOpcode(message.GetType());
            // Log.Info(opcode.ToString()+"  "+message);
            Send(flag, opcode, message);
        }

        public void Send(byte flag, ushort opcode, object message)
        {
            

            
        }

        public void Send(MemoryStream stream)
        {
            
        }
    }
}
