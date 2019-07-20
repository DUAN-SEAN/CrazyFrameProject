using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using GameServer.Battle;

namespace GameServer.System.NetHandlerSystem
{
    [MessageHandler]
    public class C2S_BattleCommandHandler : AMHandler<C2S_BattleCommandMessage>
    {
        /// <summary>
        /// 由客户端同步过来的个人指令
        /// 服务器做如下处理
        /// 1 验证 玩家的确定性
        /// 2 分发 至战斗现场
        /// 3 记录 指令
        /// </summary>
        /// <param name="playerContext"></param>
        /// <param name="message">网络消息</param>
        protected override void Run(ISession playerContext, C2S_BattleCommandMessage message)
        {
            
            GameServer.Instance.PostMessageToSystem<BattleSystem>(null);
        }
    }



}
