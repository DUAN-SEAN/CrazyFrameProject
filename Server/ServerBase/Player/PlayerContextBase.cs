using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.NetSharp;
using Crazy.Common;
using System.IO;
using System.Threading;

namespace Crazy.ServerBase
{
    /// <summary>
    /// 玩家现场基类
    public class PlayerContextBase : IClientEventHandler,ILocalMessageHandler,ILockableContext,IManagedContext,ISession
    /// </summary>
    {
        /// <summary>
        /// 将玩家现场和一个client对象关联在一起
        /// </summary>
        /// <param name="client">client对象</param>
        /// <param name="clientProtoDictionary">协议字典</param>
        public void AttachClient(IClient client,OpcodeTypeDictionary opcodeTypeDictionary)
        {
            if (m_client != null)
            {
                m_client.Close();
            }
            m_OpcodeTypeDictionary = opcodeTypeDictionary;
            m_client = client;
          
        }
        /// <summary>
        /// 通知玩家现场对象连接完成
        /// 由ServerBase::OnConnect()来发起call
        /// </summary>
        public virtual void OnConnected()
        {
            Send(new C2S_SearchUser { Account = "Sean Duan",UserId = 00001});
            Log.Debug("PlayerContextBase::OnConnected");
        }
        #region ILockableContext
        public async Task EnterLock()
        {
            await m_ctxLock.WaitAsync();

            m_lockedBindStubLock = m_bindStubLock;
            if (m_lockedBindStubLock != null)
            {
                await m_lockedBindStubLock.WaitAsync();
            }
        }
        public ulong GetInstanceId()
        {
            return m_contextId;
        }
        
        public void LeaveLock()
        {
            if (m_lockedBindStubLock != null)
            {
                m_lockedBindStubLock.Release();
                m_lockedBindStubLock = null;
            }
            m_ctxLock.Release();
        }
        #endregion


        #region IClientEventHandler
        /// <summary>
        /// 默认网络消息的处理是解析protobuf消息 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="dataAvailable"></param>
        /// <returns></returns>
        public virtual async Task<int> OnData(byte[] buffer, int dataAvailable)
        {
            if (buffer == null || dataAvailable < 1)
            {
                Log.Error("PlayerContextBase::OnData, but checked buffer failed");
                return 0;
            }

            // 1.解析数据包（这里固定使用包头格式，有需求以后再扩展）
            // 2.反序列化消息对象（使用C#的Protobuff）
            // 3.调用消息对象上的处理函数（每个消息必须从IMessage继承）

            // 以下的任何处理都使用异常传递机制表示这里出现问题，由调用上层处理该状况。


            int dataOffset = 0;//处理的消息长度
            const int uint16Length = sizeof(ushort);
            //由ServerBase提供消息解析方法
            while (true)
            {
                // 剩余数据已经小于包头，则直接返回
                if (dataAvailable - dataOffset < uint16Length * 2)
                    return dataOffset;

                Type msgType = null;
                Object deserializeObject = null;
                MemoryStream deserializeBuff = null;
                bool flag;
                var byteHandled = ServerBase.Instance.UnpackProtobufObject(buffer, dataAvailable, dataOffset, out msgType, out deserializeObject, out deserializeBuff,out flag);
                if (byteHandled == 0) return dataOffset;

                try
                {
                    //这里调用消息处理
                    IMessage message = deserializeObject as IMessage;
                    if(message == null)
                    {
                        Log.Error($"Message Deserialize FAIL MessageType = {deserializeObject.GetType()}");
                        return 0;
                    }
                    //Log.Msg(message);
                    var opcode = OpcodeTypeDictionary.Instance.GetIdByType(msgType);
                    if (!flag)//普通消息
                    {
                        //在这里将消息进行分发
                        MessageDispather.Instance.
                            Handle(this, new MessageInfo
                            {
                                Message = message,
                                Opcode = opcode
                            });

                        //PostLocalMessage()
                    }
                    else
                    {
                        IResponse response = message as IResponse;
                        if (response == null)
                        {
                            throw new Exception($"flag is response, but message is not! {opcode}");
                        }
                        Action<IResponse> action;
                        if (!m_requestCallback.TryGetValue(response.RpcId, out action))
                        {
                            return -1;
                        }
                        m_requestCallback.Remove(response.RpcId);

                        action(response);
                    }
                    

                   


                }
                catch (Exception ex)
                {
                    Log.Error($"PlayerContextBase::OnData func failed {ex.ToString()}");
                    throw;
                }
                finally
                {
                    //无论 移动到下一个消息头
                    dataOffset += byteHandled;
                }

                

            }
        }
        //玩家上下文切断与客户端的联系
        public Task OnDisconnected()
        {
            Log.Debug("PlayerContextBase::OnDisconnected");
            // 默认直接关闭客户端对象并释放现场
            m_client.Close();
            Release();
            return Task.CompletedTask;
        }

        public virtual void OnException(Exception e)
        {
            if (e != null)
            {
                Log.Error($"PlayerContextBase::OnException{e}");
            }

            // 默认直接关闭客户端对象并释放现场
            m_client.Close();
            Release();
        }
        #endregion

        #region ILocalMessageHandler
        /// <summary>
        /// 消息都走这里，从网络来的消息 或者是内部产生的消息 都走这里 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual async Task OnMessage(ILocalMessage msg)
        {
            //在这里将处理一系列消息

            switch (msg.MessageId)
            {
                //AsyncAction消息的回调
                case ServerBaseLocalMesssageIDDef.LocalMsgAsyncActionResult:
                    ContextAsyncAction contextAsyncAction = msg as ContextAsyncAction;
                    contextAsyncAction.OnResultInternal();//执行回调

                    break;
                case ServerBaseLocalMesssageIDDef.NetMessage://如何是网络消息就通过分发器进行分发



                    break;
                default:break;
            }


            //本地消息和网络消息分开

            //收到的如果是网络消息就通过分发器进行分发 分发器由ServerBase维护

            //收到的是本地消息就在本地进行处理


            return;
        }
        /// <summary>
        /// 从资源上释放玩家现场 (管理器中移除)
        /// </summary>
        public virtual void Release()
        {
            m_isReleased = true;
            ServerBase.Instance.PlayerCtxManager.FreePlayerContext(this);
        }
        public virtual bool IsAvaliable()
        {
            return !m_isReleased;
        }

        public void AddRef4AsyncAction()
        {
            Interlocked.Increment(ref m_asyncActionRef);
        }

        public int GetRef4AsyncAction()
        {
            return m_asyncActionRef;
        }

        public void RemoveRef4AsyncAction()
        {
            Interlocked.Decrement(ref m_asyncActionRef);
        }

        public virtual bool PostLocalMessage(ILocalMessage msg)
        {
            if (IsAvaliable())
            {
                return m_client.PostLocalMessage(msg);
            }
            return false;
        }
        #endregion

        #region ISession

        public void Reply(IResponse message)
        {
            Send(message);
        }  
        //一次发送多个消息
        public int Send(List<IMessage> messages)
        {
            foreach (var item in messages)
            {
                Send(item);
            }

            return 0;
        }
        public int Send(IMessage message)
        {
            // 包检查
            if (message == null)
                throw new ArgumentNullException("PlayerContextBase::SendPackage packageObj");

            // 客户端代理不可用
            if (m_client == null || m_client.Disconnected)
                return -1;

            Task t = SendAsync(ServerBase.Instance.PackProtobufObject(message));
            return 0;
        }
        public async Task SendAsync(ClientOutputBuffer buff)
        {
            //这里可以写一个发包统计
            if (buff != null)
            {
                
            }
            else
            {
                Log.Error("SendPackageImpl error buff==null");
            }
            var flag = await m_client.Send(buff);
            if(flag == false)
            {
                Log.Error("发送消息失败");
            }
        }
        public Task<IResponse> Call(IRequest request)
        {
            int rpcId = ++RpcId;
            var tcs = new TaskCompletionSource<IResponse>();

            this.m_requestCallback[rpcId] = (response) =>
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
            this.Send(request);
            return tcs.Task;
        }

        public Dictionary<int, Action<IResponse>> GetRPCActionDic()
        {
            return m_requestCallback;
        }
        #endregion
        /// <summary>
        /// 玩家现场和通信体绑定
        /// </summary>
        private IClient m_client;
        /// <summary>
        /// 协议字典集
        /// </summary>
        private OpcodeTypeDictionary m_OpcodeTypeDictionary;
   
        /// <summary>
        /// 是否已经释放
        /// </summary>
        protected bool m_isReleased = false;
        /// <summary>
        /// 针对asyncaction的ref
        /// </summary>
        protected int m_asyncActionRef = 0;
        /// <summary>
        /// 现场锁
        /// </summary>
        protected SemaphoreSlim m_ctxLock = new SemaphoreSlim(1);
        /// <summary>
        /// 用来保存ExchangeLock时传入的lock
        /// </summary>
        protected SemaphoreSlim m_bindStubLock = null;
        /// <summary>
        /// 已经处于锁定状态的stublock,见EnterLock
        /// </summary>
        protected SemaphoreSlim m_lockedBindStubLock = null;


        #region IManagedContext
        public ulong ContextId { get => m_contextId; set => m_contextId = value; }

        public string ContextStringName { get => m_gameUserId; }
        /// <summary>
        /// 对象标识，有应用层设定并且由应用层保证其唯一。
        /// </summary>
        private String m_gameUserId;
        private ulong m_contextId;
        #endregion

        private Dictionary<int, Action<IResponse>> m_requestCallback = new Dictionary<int, Action<IResponse>>();
        private static int c_rpcId;
        private static int RpcId { get {
                if (c_rpcId >= Int32.MaxValue)
                {
                    c_rpcId = 0;
                    
                }
                return c_rpcId;

            }set { c_rpcId = value; } }

        public ulong SessionId { get => m_contextId; }
    }
}
