using Crazy.NetSharp;
using System.Collections.Generic;
using GameActorLogic;


namespace GameServer
{
    /// <summary>
    /// 玩家现场被关闭时发送给匹配系统
    /// </summary>
    public class ToBattlePlayerShutdownMessage : ILocalMessage
    {
        public int MessageId => GameServerConstDefine.BattleSystemPlayerShutdown;

        public string playerId;//玩家Id

    }
    /// <summary>
    /// 创建战斗关卡
    /// 
    /// </summary>
    public class CreateBattleBarrierMessage : ILocalMessage
    {
        public CreateBattleBarrierMessage()
        {
            Players = new List<string>();
        }
        public int MessageId => GameServerConstDefine.BattleSystemCreateBattleBarrier;
        public int BarrierId;//关卡Id
        public List<string> Players;//玩家Id集合
    }


    /// <summary>
    /// 战斗指令服务器转发内部消息
    /// </summary>
    public class CommandBattleLocalMessage:ILocalMessage
    {
        public int MessageId => GameServerConstDefine.BattleSystemCommandUpLoad;

        public ulong battleId;//战斗id

        public ICommand ICommand;//战斗指令

    }
    public class ClientReadyBattleLocalMessage:ILocalMessage
    {
        public int MessageId
        {
            get => GameServerConstDefine.BattleSystemClientReadyBattle;
        }

        public ulong battleId;//战斗Id

        public string playerId;//玩家Id
    }

    public class ExitBattleLocalMessage:ILocalMessage
    {
        public int MessageId
        {
            get => GameServerConstDefine.BattleSystemExitBattleBarrier;
        }

        public ulong BattleId;

        public string PlayerId;
    }
    public class ReleaseBattleTimerLocalMessage:ILocalMessage
    {
        public int MessageId
        {
            get => GameServerConstDefine.BattleSystemNeedReleaseBattleTimer;
        }

        public long TimerId;
    }
}
