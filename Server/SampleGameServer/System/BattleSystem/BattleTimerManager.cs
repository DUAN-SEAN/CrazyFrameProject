using Crazy.Common;
using Crazy.ServerBase;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OnTimerCallBack = Crazy.ServerBase.TimerManager.OnTimerCallBack;
namespace GameServer.Battle
{
    public class BattleTimerManager
    {
      
        public BattleTimerManager()
        {

        }
        /// <summary>
        /// 设置循环Timer
        /// </summary>
        /// <param name="period">循环间隔时间，单位：毫秒</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="key">回调参数</param>
        /// <returns>该循环Timer唯一ID Int64.MinValue表示失败</returns>
        public Int64 SetLoopTimer(Int32 period, OnTimerCallBack callBack, Int32 int32Data = 0, Int64 int64Data = 0, Object objData = null)
        {
            if (period < 100 || m_state != 1)
                return Int64.MinValue;

            // 获取该循环Timer唯一ID
            Int64 timerId = Interlocked.Increment(ref m_timerIdGenerator);

            CancellationTokenSource cts = new CancellationTokenSource();

            // 创建该循环Timer
            TimerNode timerNode = new TimerNode(timerId, period, cts, callBack, int32Data, int64Data, objData);

            // 创建该Timer循环任务
            timerNode.m_task = TimerWorkerProc(cts.Token, timerNode);

            // 尝试添加
            Boolean bRet = m_loopTimers.TryAdd(timerId, timerNode);
            if (!bRet)
            {
                throw new Exception("fail to TryAdd timerPair in _loopTimers timerid = " + timerId.ToString());
            }

            // _log.InfoFormat("SetLoopTimer {0} {1} is start", timerId, period.ToString());
            return timerId;
        }

        /// <summary>
        /// 移除一个循环Timer
        /// </summary>
        /// <param name="id">该循环Timer唯一ID</param>
        /// <returns>成功：该循环Timer唯一ID，失败：负值</returns>
        public Boolean UnsetLoopTimer(Int64 timerId)
        {
            TimerNode timerNode = null;

            // 先从容器内移除
            Boolean ret = m_loopTimers.TryRemove(timerId, out timerNode);

            // 对timer执行cancel操作
            if (null != timerNode)
            {
                timerNode.CancelTask();
            }

            return ret;
        }


      

        /// <summary>
        /// 开启Timer
        /// </summary>
        /// <returns>0:成功</returns>
        public Int32 Start()
        {
            Log.Info("TimerManager::Start");
            Log.Info("TimerManager::Start is checking state flag");
            if (0 != Interlocked.CompareExchange(ref m_state, 1, 0))
            {
                Log.Error("TimerManager::Start checked state flag failed");
                return -1;
            }
            
            Log.Info("TimerManager::Start end of being called successful");
            return 0;
        }

        /// <summary>
        /// 结束Timer
        /// </summary>
        public Boolean Stop()
        {
            // 检查标志
            if (1 != Interlocked.CompareExchange(ref m_state, 2, 1))
                return false;

           
            // 清空循环timer容器
            // 先清空循环timer任务
            foreach (KeyValuePair<Int64, TimerNode> value in m_loopTimers)
            {
                if (value.Value == null)
                    continue;
                value.Value.CancelTask();
            }
            // 再清空循环timer容器
            m_loopTimers.Clear();

            return true;
        }



        /// <summary>
        /// 单个TimerNode的循环Timer工作
        /// </summary>
        /// <param name="cancelToken">控制取消对象</param>
        /// <param name="timerNode">单个TimerNode对象</param>
        /// <returns></returns>
        private async Task TimerWorkerProc(CancellationToken cancelToken, TimerNode timerNode)
        {
            // 检查工作状态
            if (1 != m_state)
                return;

            try
            {
                Int64 currentTimeTick = 0;
                Int64 delayMillSeconds = 0;

                // 没有收到取消请求时
                while (!cancelToken.IsCancellationRequested)
                {
                    // 获取处理开始时间毫微秒
                    currentTimeTick = DateTime.Now.Ticks;

                    // 第一次设置时间
                    if (timerNode.m_nextCallTime == 0)
                    {
                        timerNode.m_nextCallTime = currentTimeTick + timerNode.m_period * 10000;//*10000 转换成tick单位
                    }

                    if (currentTimeTick >= timerNode.m_nextCallTime)
                    {
                        try
                        {
                            timerNode.MakeTimerCall();
                        }
                        catch (Exception ex)
                        {
                            Log.Error("TimerManager::TimerWorkerProc catch Exception0: " + ex.ToString());
                        }
                        timerNode.m_nextCallTime = currentTimeTick + timerNode.m_period * 10000;
                    }

                    // 获取处理结束时间毫秒
                    delayMillSeconds = timerNode.m_nextCallTime - DateTime.Now.Ticks;
                    if (delayMillSeconds > 10000000) // 大于1秒休息一秒
                    {
                        await Task.Delay(1000);
                    }
                    else if (delayMillSeconds > 10000)// 否则休息指定时间
                    {
                        await Task.Delay((int)delayMillSeconds / 10000);
                    }
                }
                Log.Info("TimerManager::TimerWorkerProc stop timerid=" + timerNode.m_id);
            }
            catch (Exception ex)
            {
                Log.Error("TimerManager::TimerWorkerProc catch Exception1: " + ex.ToString());
            }
        }



        /// <summary>
        /// Timer状态 0：idle 1：工作 2：结束
        /// </summary>
        private Int32 m_state = 0;
        public Int32 State
        {
            get { return m_state; }
        }

        /// <summary>
        /// 当前分配的TimerID
        /// </summary>
        private Int64 m_timerIdGenerator = 0;

        /// <summary>
        /// 无限循环的Timer容器
        /// </summary>
        private ConcurrentDictionary<Int64, TimerNode> m_loopTimers;

        /// <summary>
        /// Task引用的CancellationToken
        /// </summary>
        private CancellationTokenSource m_playerCtxTimersCancelTokenSource;

        

    }
}
