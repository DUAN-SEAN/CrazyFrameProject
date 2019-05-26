using Crazy.Common;
using Crazy.NetSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    /// <summary>
    /// 恢复现场本地消息
    /// </summary>
    public class LocalMessageContextTransformOk : ILocalMessage
    {
        public int MessageId => GameServerConstDefine.LocalMsgGameServerContextTransformOK;
    }
    /// <summary>
    /// 通知目标现场关闭
    /// </summary>
    public class LocalMessageShutdownContext : ILocalMessage
    {
        public int MessageId
        {
            get { return GameServerConstDefine.LocalMsgShutdownContext; }
        }

        /// <summary>
        /// 是否需要立刻关闭，而不要进行平滑的关闭操作以保证恰当的保存能够进行
        /// </summary>
        public bool m_shutdownRightNow = false;
        /// <summary>
        /// 关闭原因
        /// </summary>
        public int m_shutdownReason;
    }
    public class SystemSendNetMessage : ILocalMessage
    {
        public int MessageId => GameServerConstDefine.SystemSendNetMessage;
        /// <summary>
        /// 玩家Id
        /// </summary>
        public string PlayerId;
        /// <summary>
        /// 待发送的网络消息
        /// </summary>
        public IMessage Message;
    }
}
