using Crazy.Common;
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
        public GameMatchPlayerContextQueue(int id,int level,int maxCount,ILocalMessageClient matchSystemClient)
        {
            m_id = id;
            m_level = level;
            m_maxMemberCount = maxCount;
            m_matchingQue = new List<MatchTeam>();
            m_matchSystemClient = matchSystemClient;//保留一个向系统发送消息的句柄
          
        }
        /// <summary>
        /// 匹配算法 操作m_matchTeamQue
        /// 这个Update由MatchSystem进行调用  
        /// </summary>
        public void MatchUpdate()
        {
            //TODO:首先要检测是否能够进行匹配
            if (m_matchingQue.Count < 1) return;


            //这里执行匹配算法
            //1 关卡容量maxCount
            //2 队伍人数  
            //3 贪心算法
            // 注意 Queue 除了on开头的方法以外 其他的方法都不修改队伍的状态

            // 初始化
            List<MatchBucket> matchBuckets = new List<MatchBucket>();
            List<MatchTeamArg> matchTeamArgs = new List<MatchTeamArg>();
            foreach (var item in m_matchingQue)
            {
                MatchTeamArg matchTeamArg = new MatchTeamArg { MatchTeam = item, index = -1 };
                matchTeamArgs.Add(matchTeamArg);
            }
            
            //装桶
            for (int i = 0; i < matchTeamArgs.Count;i++)
            {
                MatchTeamArg matchTeamArg = matchTeamArgs[i];
                if (matchTeamArg.index != -1) continue;
                int j = 0;
                for (; j < matchBuckets.Count; j++)
                {
                    MatchBucket matchBucket = matchBuckets[j];
                    int count = matchBucket.CurrentVolume;
                    if ((matchBucket.Capacity-matchBucket.CurrentVolume)>=matchTeamArg.MatchTeam.CurrentCount)//如果容量足够 则添加
                    {
                        matchBucket.matchTeams.Add(matchTeamArg.MatchTeam);
                        matchTeamArg.index = j;//设置当前所在桶编号
                    }
                }
                if (matchTeamArg.index < 0)//需要新的桶
                {

                    var matchBucket =  MatchBucketPool.Instance.Fetch(m_maxMemberCount);
                    matchBucket.matchTeams.Add(matchTeamArg.MatchTeam);
                    matchTeamArg.index = j;
                    matchBuckets.Add(matchBucket);

                    if (j != matchBuckets.Count - 1)
                    {
                        Log.Error("索引出现问题");
                        return;
                    }            
                }

            }

            // 把桶遍历
            for(int i = 0; i < matchBuckets.Count; i++)
            {
                var matchBucket = matchBuckets[i];
                if (matchBucket.CurrentVolume == matchBucket.Capacity)//桶的容量满了
                {
                    Log.Info($"第 {i} 桶满足条件 向GameServer发送开启战斗请求");
                    //向matchSystem发送这几个队伍的id

                    MatchQueueCompleteSingleMessage matchQueueCompleteSingleMessage = new MatchQueueCompleteSingleMessage();


                    // 将matchteam从匹配列表中删除
                    foreach(var item in matchBucket.matchTeams)
                    {
                        matchQueueCompleteSingleMessage.teamIds.Add(item.Id);
                        matchQueueCompleteSingleMessage.barrierId = m_id;
                        m_matchingQue.Remove(item);//从匹配队列中清除
                    }
                    //向MatchSystem发送队伍匹配完成消息
                    m_matchSystemClient.PostLocalMessage(matchQueueCompleteSingleMessage);

                }
                else
                {
                    //不符合 这里可以再进行一次桶合并策略，目前不做 进一步的算法优化
                }
            }
            // 释放所有的桶
            for(int i = 0; i < matchBuckets.Count; i++){
                var matchBucket = matchBuckets[i];
                matchBucket.Dispose();
            }
           




        }

        public void OnJoinMatchQueue(MatchTeam matchTeam)
        {
            OnEnterLock();
            //先做状态检查  再执行逻辑
            if (matchTeam.State != MatchTeam.MatchTeamState.OPEN) return;
            //将队伍添加到队列中  这个状态属于中间态  客户端感知不到这个状态的存在
            m_matchingQue.Add(matchTeam);
            matchTeam.State = MatchTeam.MatchTeamState.Matching;


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
                case MatchTeam.MatchTeamState.Matching:
                    //这个状态是客户端能够感知
                    m_matchingQue.Remove(matchTeam);

                    matchTeam.State = MatchTeam.MatchTeamState.OPEN;//退出匹配队列后 队伍开放

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

        private ILocalMessageClient m_matchSystemClient;


        public int Id { get => m_id; }
        public int Level { get => m_level; }
        public int MaxCount { get => m_maxMemberCount; }


    }

    struct MatchTeamArg {
        public MatchTeam MatchTeam;
        public int index;
    }

}
