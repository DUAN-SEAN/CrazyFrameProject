using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.NetSharp;
namespace Crazy.ServerBase
{
    /// <summary>
    /// 玩家现场基类
    /// </summary>
    public class PlayerConetextBase : IClientEventHandler,ILocalMessageHandler,ILockableContext,IManagedContext
    {
        /// <summary>
        /// 将玩家现场和一个client对象关联在一起
        /// </summary>
        /// <param name="client">client对象</param>
        /// <param name="clientProtoDictionary">协议字典</param>
        /// <param name="clientProtoHandlerDictionary">协议实现句柄字典</param>
        public void AttachClient(IClient client)
        {
            if (m_client != null)
            {
                m_client.Close();
            }
            m_client = client;
          
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
        public Task<int> OnData(byte[] buffer, int dataAvailable)
        {
            throw new NotImplementedException();
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
        #region IManagedContext
        public ulong ContextId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string ContextStringName => throw new NotImplementedException();
        #endregion
    }
}
