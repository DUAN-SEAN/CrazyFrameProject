using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class KillTaskCondition:ITaskCondition
    {

        protected ITaskEvent m_event;
        protected readonly int key;
        protected ILevelActorComponentBaseContainer level;

        protected Dictionary<int, int> Currentvalue;
        public KillTaskCondition(ITaskEvent mEvent, int key,ILevelActorComponentBaseContainer level)
        {
            Currentvalue = new Dictionary<int, int>();
            m_event = mEvent;
            this.key = key;
            this.level = level;
            Currentvalue.Add(key, 0);
            RegisterHandler();
        }
        /// <summary>
        /// 去handler组件注册击杀事件
        /// </summary>
        protected void RegisterHandler()
        {
            level.GetHandlerComponentInternalBase().OnDestroyMessageHandler += obj =>
            {
                var actor = level.GetEnvirinfointernalBase().GetActor(obj);
                if (actor.IsShip() && actor.GetCamp() != LevelActorBase.PlayerCamp) Currentvalue[key]++;
            };
        }

        public bool TickCondition()
        {
            //尝试从任务中获取值
            if (!m_event.TryGetValue(key, out var killvalue)) return false;
            //判断击杀数到达
            if (Currentvalue[key] >= killvalue) return true;
            //未到达
            return false;
        }

        public void Dispose()
        {
            level = null;
            m_event = null;
            Currentvalue.Clear();
            Currentvalue = null;

        }

        public Dictionary<int, int> ConditionCurrentValues
        {
            get => Currentvalue;
            set => Currentvalue = value;
        }
    }
}
