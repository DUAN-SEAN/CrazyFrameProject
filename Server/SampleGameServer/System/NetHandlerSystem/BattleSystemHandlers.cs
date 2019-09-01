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
    /// <summary>
    /// 接受指令消息handler
    /// </summary>
    [MessageHandler]
    public class C2S_CommandBattleMessageHandler:AMHandler<C2S_CommandBattleMessage>
    {
        /// <summary>
        /// 序列化器
        /// </summary>
        private static BinaryFormatter bf = new BinaryFormatter();
        /// <summary>
        /// 由客户端同步过来的个人指令
        /// 服务器做如下处理
        /// 1 验证 玩家的确定性
        /// 2 分发 至战斗现场
        /// 3 记录 指令
        /// </summary>
        /// <param name="playerContext"></param>
        /// <param name="message">网络消息</param>
        protected override void Run(ISession playerContext, C2S_CommandBattleMessage message)
        {
            CommandBattleLocalMessage localMessage = new CommandBattleLocalMessage {battleId = message.BattleId};

            using (MemoryStream ms = new MemoryStream(message.Command.ToByteArray()))
            {
                localMessage.ICommand = bf.Deserialize(ms) as ICommand;//将其反序列化
                //Log.Info("服务器收到指令时间："+ (localMessage.ICommand as Command).currenttime);
               // (localMessage.ICommand as Command).currenttime = DateTime.Now;
                //Log.Debug("接收到一条指令:"+localMessage.ICommand.CommandType);
                //GameServer.Instance.PostMessageToSystem<BattleSystem>(localMessage);
                GameServer.Instance.GetSystem<BattleSystem>().GetBattleEntity(message.BattleId)?.SendCommandToLevel(localMessage.ICommand);


            }
        }
    }
    [MessageHandler]
    public class C2S_ExitBattleMessageHandler:AMHandler<C2S_ExitBattleMessage>
    {
        protected override void Run(ISession playerContext, C2S_ExitBattleMessage message)
        {
            ExitBattleLocalMessage localMessage = new ExitBattleLocalMessage
            {
                BattleId = message.BattleId, PlayerId = message.PlayerId
            };


            GameServer.Instance.PostMessageToSystem<BattleSystem>(localMessage);
        }
    }

    [MessageHandler]
    public class C2S_ReadyBattleMessageHandler:AMHandler<C2S_ReadyBattleBarrierReq>
    {
        protected override void Run(ISession playerContext, C2S_ReadyBattleBarrierReq message)
        {
            ClientReadyBattleLocalMessage crblm = new ClientReadyBattleLocalMessage
            {
                battleId = message.BattleId, playerId = message.PlayerId
            };
            GameServer.Instance.PostMessageToSystem<BattleSystem>(crblm);
        }
    }


    [MessageHandler]
    public class C2S_DelayReqHandler:AMRpcHandler<C2S_DelayReq,S2C_DelayAck>
    {
        protected override void Run(ISession playerContext, C2S_DelayReq message, Action<S2C_DelayAck> reply)
        {
            S2C_DelayAck response = new S2C_DelayAck();
            response.Time = DateTime.Now.Ticks;
            reply(response);

        }
    }
}
