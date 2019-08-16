using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class TimeTaskCondition:ITaskCondition
    {
        protected readonly ITaskEvent m_event;
        protected readonly int key;
        protected readonly long Inittime;

        protected Dictionary<int, int> CurrentValue;
        public TimeTaskCondition(ITaskEvent mEvent,int key)
        {
            CurrentValue = new Dictionary<int, int>();
            m_event = mEvent;
            this.key = key;
            Inittime = DateTime.Now.Ticks;
            CurrentValue.Add(key, 0);
        }
        public bool TickCondition()
        {
            //尝试从任务中获取值
            if (!m_event.TryGetValue(key, out var timevalue)) return false;
            CurrentValue[key] = (int)((DateTime.Now.Ticks - Inittime) / 1000);
            //判断时间是否到达时间
            if (Inittime + timevalue < DateTime.Now.Ticks) return true;
            //时间到达
            return false;
        }

        public Dictionary<int, int> ConditionCurrentValues => CurrentValue;
    }
}
