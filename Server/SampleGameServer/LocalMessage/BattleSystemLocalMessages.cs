using Crazy.NetSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic.Command;

namespace GameServer
{
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



    public class CommandBattleLocalMessage:ILocalMessage
    {
        public int MessageId => GameServerConstDefine.BattleSystemCommandUpLoad;

        public ulong battleId;//战斗id

        public ICommand ICommand;//战斗指令


    }
}
