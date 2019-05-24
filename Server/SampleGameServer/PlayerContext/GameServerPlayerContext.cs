using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using Crazy.NetSharp;
using Crazy.ServerBase;
namespace GameServer
{
    /// <summary>
    /// 玩家现场
    /// </summary>
    public sealed class GameServerPlayerContext:PlayerContextBase
    {
        public GameServerPlayerContext()
        {
            m_csm = CreatePlayerContextStateMachine();
        }
        /// <summary>
        /// 创建玩家现场状态机
        /// </summary>
        /// <returns></returns>
        private PlayerContextStateMachine CreatePlayerContextStateMachine()
        {
            return new PlayerContextStateMachine();
        }
        /// <summary>
        /// 该事件基类现场没有实现任何逻辑
        /// 只需要添加最新的逻辑即可
        /// </summary>
        public override void OnConnected()
        {
            m_csm.SetStateCheck(PlayerContextStateMachine.EventOnConnected);
        }


        /// <summary>
        /// 直接重写
        /// </summary>
        /// <param name="msg">本地消息类型</param>
        /// <returns></returns>
        public override Task OnMessage(ILocalMessage msg)
        {
            switch (msg.MessageId)
            {
                
                default:
                    return base.OnMessage(msg);
                   
            }


            return Task.CompletedTask;
        }
        protected override Task OnPlayerContextTimer(PlayerTimerMessage msg)
        {

            return Task.CompletedTask;
        }
        private PlayerContextStateMachine m_csm;//玩家现场状态机
    }
}
