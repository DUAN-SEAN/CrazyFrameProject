using Crazy.NetSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    /// <summary>
    /// 创建战斗关卡
    /// 
    /// </summary>
    public class CreateBattleBarrierMessage : ILocalMessage
    {
        public int MessageId => throw new NotImplementedException();
        public int BarrierId;//关卡Id
        public List<string> Players;//玩家Id集合
    }
}
