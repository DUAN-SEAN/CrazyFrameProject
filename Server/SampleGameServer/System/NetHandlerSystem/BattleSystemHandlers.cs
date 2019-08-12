using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using GameActorLogic;
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
            
            //GameServer.Instance.PostMessageToSystem<BattleSystem>(null);
        }
    }

    /// <summary>
    /// 接受指令消息handler
    /// </summary>
    [MessageHandler]
    public class C2S_CommandBattleMessageHandler:AMHandler<C2S_CommandBattleMessage>
    {
        private static BinaryFormatter bf = new BinaryFormatter();
        protected override void Run(ISession playerContext, C2S_CommandBattleMessage message)
        {
            GameServerPlayerContext ctx = playerContext as GameServerPlayerContext;

            CommandBattleLocalMessage localMessage = new CommandBattleLocalMessage();
            localMessage.battleId = message.BattleId;

            using (MemoryStream ms = new MemoryStream(message.Command.ToByteArray()))
            {
                localMessage.ICommand = bf.Deserialize(ms) as ICommand;//将其反序列化


                GameServer.Instance.PostMessageToSystem<BattleSystem>(localMessage);

            }
        }
    }
}
