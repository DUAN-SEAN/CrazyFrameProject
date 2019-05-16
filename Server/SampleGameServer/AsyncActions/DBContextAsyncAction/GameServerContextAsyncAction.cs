using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.NetSharp;
using Crazy.ServerBase;

namespace GameServer
{
    public class SampleGameServerContextAsyncAction:ContextAsyncAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="targetQueueUserId"></param>
        /// <param name="needResult"></param>
        /// <param name="needSeq"></param>
        public SampleGameServerContextAsyncAction(GameServerContext context, String targetQueueUserId, Boolean needResult = false, Boolean needSeq = false)
            : base(context, GameServer.Instance.AsyncActionQueuePool.GetAdaptedQueueByUserId(targetQueueUserId), needResult, needSeq)
        {

            ContextUserId = context.ContextId;
        }

        public ulong ContextUserId { get; private set; }

    }
    public class SampleGameServerAsyncActionSequenceQueuePool
    {
        public SampleGameServerAsyncActionSequenceQueuePool(UInt32 poolSize)
        {
            _queuePool = new SampleGameServerAsyncActionSequenceQueue[poolSize];
            for (Int32 pos = 0; pos < poolSize; ++pos)
            {
                _queuePool[pos] = new SampleGameServerAsyncActionSequenceQueue();
            }
        }

        public SampleGameServerAsyncActionSequenceQueue GetAdaptedQueueByUserId(String userId)
        {
            if (userId == null) return _queuePool[0];//如果userid为null  表示为非现场耗时任务
            Int32 hash = userId.GetHashCode();
            return _queuePool[Math.Abs(hash) % (_queuePool.GetLength(0))];
        }

        private SampleGameServerAsyncActionSequenceQueue[] _queuePool;
    }

    /// <summary>
    /// 用于顺序化FriendContextAsyncAction的队列
    /// </summary>
    public class SampleGameServerAsyncActionSequenceQueue : IContextAsyncActionSequenceQueue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SampleGameServerAsyncActionSequenceQueue()
        {
            _queue = new ConcurrentQueue<ContextAsyncAction>();
        }

        /// <summary>
        /// 返回队首元素，但是不移除
        /// </summary>
        /// <returns></returns>
        public ContextAsyncAction FirstInActionSeqQueue()
        {
            ContextAsyncAction action;
            if (_queue.TryPeek(out action))
            {
                return action;
            }
            return null;
        }

        /// <summary>
        /// 元素入队
        /// </summary>
        /// <param name="action"></param>
        public void EnqueueActionSeqQueue(ContextAsyncAction action)
        {
            _queue.Enqueue(action);
        }

        /// <summary>
        /// 返回并移除队首元素
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool TryDequeueActionSeqQueue(out ContextAsyncAction action)
        {
            return _queue.TryDequeue(out action);
        }

        /// <summary>
        /// 队列
        /// </summary>
        private ConcurrentQueue<ContextAsyncAction> _queue;
    }
}
