using Crazy.NetSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    /// <summary>
    /// 匹配队伍的类型，用于保留一份队伍Id名单和队伍最大容量
    /// 后期匹配队伍可能需要状态机控制，但目前不使用状态机，简单。
    /// 匹配队伍目前的状态为线性态
    /// 注意：该类型的实例不是线程安全，出现异常现状优先检测它
    /// </summary>
    public class MatchTeam
    {
        public MatchTeam(UInt64 id,int maxCount)
        {
            
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



        public UInt64 Id { get => Member.ElementAt(0); }//表示队伍的唯一Id表示，不可被更改

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
            Matching,//在匹配中
            WaitMatchQue//在匹配等待队列中
        }
    }
}
