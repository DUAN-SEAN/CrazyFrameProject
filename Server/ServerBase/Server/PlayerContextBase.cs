using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.NetSharp;
using Crazy.Common;
using System.IO;

namespace Crazy.ServerBase
{
    /// <summary>
    /// 玩家现场基类
    /// </summary>
    public class PlayerContextBase : IClientEventHandler,ILocalMessageHandler,ILockableContext,IManagedContext
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
            Log.Debug("PlayerContextBase::OnConnected");
        }
        #region ILockableContext
        public Task EnterLock()
        {
            throw new NotImplementedException();
        }
        public long GetInstanceId()
        {
            throw new NotImplementedException();
        }
        
        public void LeaveLock()
        {
            throw new NotImplementedException();
        }
        #endregion


        #region IClientEventHandler
        /// <summary>
        /// 由
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="dataAvailable"></param>
        /// <returns></returns>
        public virtual async Task<int> OnData(byte[] buffer, int dataAvailable)
        {
            if (dataBuff == null || dataAvailable < 1)
            {
                Log.Error("PlayerContextBase::OnData, but checked buffer failed");
                return 0;
            }

            // 1.解析数据包（这里固定使用包头格式，有需求以后再扩展）
            // 2.反序列化消息对象（使用C#的Protobuff）
            // 3.调用消息对象上的处理函数（每个消息必须从IMessage继承）

            // 以下的任何处理都使用异常传递机制表示这里出现问题，由调用上层处理该状况。


            int dataOffset = 0;
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

                var byteHandled = ServerBase.Instance.UnpackProtobufObject(dataBuff, dataAvailable, dataOffset, out msgType, out deserializeObject, out deserializeBuff);
                if (byteHandled == 0) return dataOffset;

                try
                {
                    //这里调用消息处理
                    Google.Protobuf.IMessage message = deserializeObject as Google.Protobuf.IMessage;
                    if(message == null)
                    {
                        Log.Error($"Message Deserialize FAIL MessageType = {deserializeObject.GetType()}");
                        return 0;
                    }
                    //在这里将消息进行分发


                }
                catch (Exception ex)
                {
                    Log.Error($"PlayerContextBase::OnData func failed {ex.ToString()}");
                    throw;
                }
                finally
                {
                    // 移动到下一个消息头
                    dataOffset += byteHandled;
                }

                

            }
        }

        public Task OnDisconnected()
        {
            throw new NotImplementedException();
        }

        public void OnException(Exception e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ILocalMessageHandler
        public Task OnMessage(ILocalMessage msg)
        {
            throw new NotImplementedException();
        }

        public bool IsAvaliable()
        {
            throw new NotImplementedException();
        }

        public void AddRef4AsyncAction()
        {
            throw new NotImplementedException();
        }

        public int GetRef4AsyncAction()
        {
            throw new NotImplementedException();
        }

        public void RemoveRef4AsyncAction()
        {
            throw new NotImplementedException();
        }

        public bool PostLocalMessage(ILocalMessage msg)
        {
            throw new NotImplementedException();
        }
        #endregion
        /// <summary>
        /// 玩家现场和通信体绑定
        /// </summary>
        private IClient m_client;
        /// <summary>
        /// 
        /// </summary>
        private OpcodeTypeDictionary m_OpcodeTypeDictionary;
        private byte[] dataBuff;
        #region IManagedContext
        public ulong ContextId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string ContextStringName => throw new NotImplementedException();
        #endregion



    }
}
