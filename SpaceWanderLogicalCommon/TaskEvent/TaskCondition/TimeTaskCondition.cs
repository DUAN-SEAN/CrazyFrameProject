using Crazy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class TimeTaskCondition:ITaskCondition
    {
        protected ITaskEvent m_event;
        protected readonly int key;
        protected long Inittime;

        protected Dictionary<int, int> CurrentValue;
        public TimeTaskCondition(ITaskEvent mEvent,int key)
        {
            CurrentValue = new Dictionary<int, int>();
            m_event = mEvent;
            this.key = key;
            Inittime = DateTime.Now.Ticks;
            CurrentValue.Add(key, 0);
        }

        public void Dispose()
        {
            m_event = null;
            CurrentValue.Clear();
            CurrentValue = null;
        }
        public bool TickCondition()
        {
            //Log.Trace("TickCondition 时间逻辑正在运行");
            //尝试从任务中获取值
            if (!m_event.TryGetValue(key, out var timevalue)) return false;
            //Log.Trace("TickCondition 取到时间值:" + timevalue);
            CurrentValue[key] = (int)((DateTime.Now.Ticks - Inittime));

            //Log.Trace("TickCondition 当前已达到时间：" + (Inittime + timevalue * 6 * 1e7) + " 当前时间：" + DateTime.Now.Ticks);
            //判断时间是否到达时间
            if (Inittime + timevalue * 60 * 1e7 < DateTime.Now.Ticks)
            {
                Log.Trace("TickCondition 时间到达");
                return true;
            }
            //时间到达
            return false;
        }

        public void StartCondition()
        {
            Inittime = DateTime.Now.Ticks;
        }

        public int GetCurrentValue()
        {
            return (int)((DateTime.Now.Ticks - Inittime) / 60 / 1e7);
        }

        public int GetTargetValue()
        {
            m_event.TryGetValue(key, out var timevalue);
            return timevalue;
        }

        public Dictionary<int, int> ConditionCurrentValues
        {
            get => CurrentValue;
            set => CurrentValue = value;
        }
    }
}
