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




        public void Add(UInt64 playerId)
        {
            lock (Member)
            {
                Member.Add(playerId);
            }
        }

        public void Remove(UInt64 playerId)
        {
            lock (Member)
            {
                Member.Remove(playerId);
            }
        }

       




        public readonly UInt64 Id;//表示队伍的唯一Id表示，不可被更改

        /// <summary>
        /// 房间玩家列表
        /// </summary>
        public readonly List<UInt64> Member = new List<UInt64>();

        public MatchTeamState State { get; set; }
        public enum MatchTeamState
        {
            OPEN,//开放房间
            CLOSE,//关闭房间
            INBATTLE//在战斗
        }
    }
}
