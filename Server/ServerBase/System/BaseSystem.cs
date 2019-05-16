using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.NetSharp;
namespace Crazy.ServerBase
{
    /// <summary>
    /// 服务器逻辑系统的基类，由于目前不做分布式系统，所以所有业务服务用System表示一类逻辑
    /// 具体的有匹配系统、物理系统等等
    /// </summary>
    public abstract class BaseSystem : ILocalMessageHandler,ILocalMessageClient
    {


        /// <summary>
        /// 开始
        /// </summary>
        public virtual void Start()
        {
            p_localMessages = new ConcurrentQueue<ILocalMessage>();
        }
        /// <summary>
        /// 驱动更新
        /// </summary>
        public virtual void Update()
        {
            ILocalMessage localMessage;
            if (p_localMessages.TryDequeue(out localMessage))
            {
                var nothing = OnMessage(localMessage);
            }
        }
        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
        {
            p_localMessages.Clear();
            p_localMessages = null;
        }
        /// <summary>
        /// 该方法需要被重写
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual Task OnMessage(ILocalMessage msg)
        {
            return Task.CompletedTask;
        }
        /// <summary>
        /// 接收传入的本地消息,玩家现场向GameServer发送的消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool PostLocalMessage(ILocalMessage msg)
        {
            p_localMessages.Enqueue(msg);
            return true;
        }

        protected ConcurrentQueue<ILocalMessage> p_localMessages;


    }

}
