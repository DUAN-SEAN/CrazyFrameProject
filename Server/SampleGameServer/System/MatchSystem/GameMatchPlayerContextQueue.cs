﻿using Crazy.NetSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer
{
    public class GameMatchPlayerContextQueue
    {
        /// <summary>
        /// 初始化匹配队列
        /// </summary>
        /// <param name="id">关卡Id</param>
        /// <param name="level">关卡等级</param>
        /// <param name="maxCount">队伍容量</param>
        public GameMatchPlayerContextQueue(int id,int level,int maxCount)
        {
            m_id = id;
            m_level = level;
            m_maxMemberCount = maxCount;
            m_matchingQue = new List<MatchTeam>();
          
        }
        /// <summary>
        /// 匹配算法 操作m_matchTeamQue
        /// 这个Update由MatchSystem进行调用  
        /// </summary>
        public void MatchUpdate()
        {
            
        }

        public void OnJoinMatchQueue(MatchTeam matchTeam)
        {
            OnEnterLock();
            //先做状态检查  再执行逻辑
            if (matchTeam.State != MatchTeam.MatchTeamState.OPEN) return;



            //将队伍添加到队列中  这个状态属于中间态  客户端感知不到这个状态的存在
            m_waitMatchQue.Enqueue(matchTeam);
            matchTeam.State = MatchTeam.MatchTeamState.WaitMatchQue;


            LeaveLock();
        }
        /// <summary>
        /// 由于队伍的主动退出或者异常状态，导致队伍离开匹配队列
        /// 队列所有操作进行上锁
        /// 
        /// </summary>
        /// <param name="playerId"></param>
        public void OnExitMatchQueue(MatchTeam matchTeam)
        {
            OnEnterLock();
            switch (matchTeam.State)
            {
                case MatchTeam.MatchTeamState.WaitMatchQue:
                    //无法退出 必须等服务器将队伍添加到匹配集合中 
                    break;
                case MatchTeam.MatchTeamState.Matching:

                    break;
                default:break;
            }
            



            LeaveLock();
        }
     
        private void OnEnterLock()
        {
            m_queueLock.Wait();

        }

        private void LeaveLock()
        {
            m_queueLock.Release();
        }

        //private Dictionary<int, List<MatchTeam>> m_matchingQueDic;//后期由于每个玩家的能力值 进行能力匹配

        private List<MatchTeam> m_matchingQue;//玩家匹配队列  主要操控这个队列进行

        private ConcurrentQueue<MatchTeam> m_waitMatchQue; 



        /// <summary>
        /// 队列锁 每次只能有一个线程去处理
        /// 这是一个混合锁，在playerContext中使用该锁锁住玩家现场
        /// 该锁将在内核模式自旋 while；然后在一定的阈值过后进行内核模式ThreadSleep
        /// </summary>
        protected SemaphoreSlim m_queueLock = new SemaphoreSlim(1);

        /// <summary>
        /// 关卡Id
        /// </summary>
        private int m_id;

        /// <summary>
        /// 关卡等级
        /// </summary>
        private int m_level;

        /// <summary>
        /// 队列中支持最多的队伍人数
        /// </summary>
        private readonly int m_maxMemberCount;


        public int Id { get => m_id; }
        public int Level { get => m_level; }
        public int MaxCount { get => m_maxMemberCount; }


    }
}
