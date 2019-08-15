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
        protected ILevelActorComponentBaseContainer level;
        protected int killship;

        public KillTaskCondition(ITaskEvent mEvent, int key,ILevelActorComponentBaseContainer level)
        {
            m_event = mEvent;
            this.key = key;
            this.level = level;
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
                if (actor.IsShip() && actor.GetCamp() != LevelActorBase.PlayerCamp) killship++;
            };
        }

        public bool TickCondition()
        {
            //尝试从任务中获取值
            if (!m_event.TryGetValue(key, out var killvalue)) return false;
            //判断击杀数到达
            if (killship >= killvalue) return true;
            //未到达
            return false;
        }
    }
}
