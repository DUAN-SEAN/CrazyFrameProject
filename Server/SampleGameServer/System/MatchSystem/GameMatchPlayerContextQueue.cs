using Crazy.NetSharp;
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
        }
        /// <summary>
        /// 匹配算法 操作m_matchTeamQue
        /// </summary>
        public void MatchUpdate()
        {

        }


        /// <summary>
        /// 一个玩家现场选择退出匹配队列 那么整个团队就退出
        /// 
        /// </summary>
        /// <param name="playerId"></param>
        public void OnExitMatchQueue(ulong playerId)
        {
            OnEnterLock();





            LeaveLock();
        }
        public void ReleasePlayerContext(ulong playerId)
        {
            IClientEventHandler playerContext;



            if (m_playerDic.TryRemove(playerId, out playerContext))
            {
                playerContext.OnMessage(null);//向玩家现场发送从队列字典中清除的消息
            }


        }

        private void OnEnterLock()
        {
            m_queueLock.Wait();

        }

        private void LeaveLock()
        {
            m_queueLock.Release();
        }


        private List<MatchTeam> m_matchTeamQue;//玩家匹配队列

        /// <summary>
        /// 队列锁 每次只能有一个线程去处理
        /// 这是一个混合锁，在playerContext中使用该锁锁住玩家现场
        /// 该锁将在内核模式自旋 while；然后在一定的阈值过后进行内核模式ThreadSleep
        /// </summary>
        protected SemaphoreSlim m_queueLock = new SemaphoreSlim(1);

        /// <summary>
        /// 该匹配队列所拥有的所有玩家现场
        /// </summary>
        private ConcurrentDictionary<ulong, IClientEventHandler> m_playerDic;

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
