using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;

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
            //Log.Trace("杀人计数注册");
        }
        /// <summary>
        /// 去handler组件注册击杀事件
        /// </summary>
        protected void RegisterHandler()
        {
            level.GetHandlerComponentInternalBase().OnDestroyMessageHandler += RegisterDestroy;
            //Log.Trace("杀人计数注册成功");

        }

        protected void RegisterDestroy(ulong obj)
        {
            var actor = level.GetEnvirinfointernalBase().GetActor(obj);
            if (actor == null)
            {
                //Log.Trace("计数对象为 null" + obj);
                return;
            }

            //Log.Trace("有计数对象" + actor.GetActorID() + "类型" + actor.GetActorType());

            if (actor.IsShip() && actor.GetCamp() != LevelActorBase.PlayerCamp)
            {
                Currentvalue[key]++;
                m_event.TryGetValue(key, out int i);
                //Log.Trace("计数加一" + actor.GetActorID() + " 当前数量："+Currentvalue[key] + " 需要击杀数量："+i);
            }

        }

        public bool TickCondition()
        {
            //Log.Trace("tick 条件"+key+" 条件集合："+Currentvalue.Count);
            //尝试从任务中获取值
            if (!m_event.TryGetValue(key, out var killvalue)) return false;
            //Log.Trace("任务条件 击杀：" + Currentvalue[key]);
            //判断击杀数到达
            if (Currentvalue[key] >= killvalue)
            {
                Log.Trace("任务完成:" + m_event.GetTaskId() + " 当前击杀数：" + Currentvalue[key] + " 任务条件：" + killvalue);
                return true;
            }
            
            //未到达 
            return false;
        }

        public void Dispose()
        {
            level.GetHandlerComponentInternalBase().OnDestroyMessageHandler -= RegisterDestroy;
            Currentvalue.Clear();
            Currentvalue = null;
            level = null;
            m_event = null;
        }

        public void StartCondition()
        {
            RegisterHandler();
        }

        public int GetCurrentValue()
        {
            return Currentvalue[key];
        }

        public int GetTargetValue()
        {
            m_event.TryGetValue(key, out var killvalue);
            return killvalue;

        }

        public Dictionary<int, int> ConditionCurrentValues
        {
            get => Currentvalue;
            set => Currentvalue = value;
        }
    }
}
