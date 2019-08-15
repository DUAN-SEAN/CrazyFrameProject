using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class KillTaskCondition:ITaskCondition
    {

        protected readonly ITaskEvent m_event;
        protected readonly int key;

        public KillTaskCondition(ITaskEvent mEvent, int key)
        {
            m_event = mEvent;
            this.key = key;
        }
        public bool TickCondition()
        {
            //尝试从任务中获取值
            if (!m_event.TryGetValue(key, out var timevalue)) return false;
            //判断时间是否到达时间
         
            //时间到达
            return false;
        }
    }
}
