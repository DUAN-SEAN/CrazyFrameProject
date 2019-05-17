using Crazy.NetSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class MatchTeam
    {
        public MatchTeam(UInt64 id)
        {
            Id = id;//设置Id
            State = MatchTeamState.OPEN;

        }



        





        public readonly UInt64 Id;

        /// <summary>
        /// 房间玩家列表
        /// </summary>
        public readonly List<IClientEventHandler> Member = new List<IClientEventHandler>();

        public MatchTeamState State { get; set; }
        public enum MatchTeamState
        {
            OPEN,//开放房间
            CLOSE,//关闭房间
            INBATTLE//在战斗
        }
    }
}
