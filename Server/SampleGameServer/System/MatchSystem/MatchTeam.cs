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
        public MatchTeam(UInt64 id,int maxCount)
        {
            Id = id;//设置Id
            State = MatchTeamState.OPEN;
            m_maxCount = maxCount;
        }




        public void Add(UInt64 playerId)
        {
            lock (Member)
            {
                Member.Add(playerId);
            }
        }

        public bool Remove(UInt64 playerId)
        {
            lock (Member)
            {
                if (Member.Remove(playerId))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 验证队伍是否满员
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            if(CurrentCount>=m_maxCount)
                return false;
            return true;
        }



        public readonly UInt64 Id;//表示队伍的唯一Id表示，不可被更改

        public int CurrentCount { get => Member.Count; }//当前队伍人数

        private int m_maxCount;//队伍最大限制人数

        /// <summary>
        /// 房间玩家列表
        /// </summary>
        private readonly HashSet<UInt64> Member = new HashSet<UInt64>();

        public MatchTeamState State { get; set; }
        public enum MatchTeamState
        {
            OPEN,//开放房间
            CLOSE,//关闭房间
            INBATTLE,//在战斗中
            Matching//在匹配中
        }
    }
}
