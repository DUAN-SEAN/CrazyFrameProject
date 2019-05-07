﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.NetSharp;
using Crazy.ServerBase;

namespace SampleGameServer
{
    public class VerifyContextAsyncAction:ContextAsyncAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="targetQueueUserId"></param>
        /// <param name="needResult"></param>
        /// <param name="needSeq"></param>
        public VerifyContextAsyncAction(GameServerContext context, String targetQueueUserId, Boolean needResult = false, Boolean needSeq = false)
            : base(context, GameServer.Instance.AsyncActionQueuePool.GetAdaptedQueueByUserId(targetQueueUserId), needResult, needSeq)
        {


        }

       
    }
    public class VerifyAsyncActionSequenceQueuePool
    {
        public VerifyAsyncActionSequenceQueuePool(UInt32 poolSize)
        {
            _queuePool = new VerifyAsyncActionSequenceQueue[poolSize];
            for (Int32 pos = 0; pos < poolSize; ++pos)
            {
                _queuePool[pos] = new VerifyAsyncActionSequenceQueue();
            }
        }

        public VerifyAsyncActionSequenceQueue GetAdaptedQueueByUserId(String userId)
        {
            Int32 hash = userId.GetHashCode();
            return _queuePool[Math.Abs(hash) % (_queuePool.GetLength(0))];
        }

        private VerifyAsyncActionSequenceQueue[] _queuePool;
    }

    /// <summary>
    /// 用于顺序化FriendContextAsyncAction的队列
    /// </summary>
    public class VerifyAsyncActionSequenceQueue : IContextAsyncActionSequenceQueue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public VerifyAsyncActionSequenceQueue()
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
