using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
   


    /// <summary>
    /// 匹配 桶 池子
    /// 减少GC 给每个匹配队列中使用的水桶创建对象池
    /// </summary>
    public class MatchBucketPool
    {
        static MatchBucketPool()
        {
            m_instance = new MatchBucketPool();
        }
        public MatchBucketPool()
        {
           
        }
        public MatchBucket Fetch(int capacity)
        {
            MatchBucket matchBucket = null;
            if (m_queue.Count > 0)
            {
                matchBucket = m_queue.Dequeue();
            }
            else
            {
                matchBucket = new MatchBucket();
            }
            matchBucket.Init(capacity);
            return matchBucket;

        }
        public void Recycle(MatchBucket matchBucket)
        {
            matchBucket.Dispose();
            m_queue.Enqueue(matchBucket);
        }
        /// <summary>
        /// 队列字典  key 桶的容量 value 容量队列
        /// </summary>
        private Queue<MatchBucket> m_queue = new Queue<MatchBucket>();

        private static MatchBucketPool m_instance;

        public static MatchBucketPool Instance;
    }
    /// <summary>
    /// 匹配桶 桶的容量等于关卡容量
    /// </summary>
    public class MatchBucket{
        
        public void Init(int capacity)
        {
            Capacity = capacity;
            matchTeams = new List<MatchTeam>();
        }

        public void Dispose()
        {
            matchTeams.Clear();

        }
        public int Capacity;
        public List<MatchTeam> matchTeams { get; set; } 
        public int CurrentVolume { get {
                int v = 0;
                foreach (var item in matchTeams)
                {
                    v += item.CurrentCount;
                }
                return v;

            } }

    }


}
