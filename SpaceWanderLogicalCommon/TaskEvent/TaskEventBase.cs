using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 目前没有特化类型
    /// </summary>
    public class TaskEventBase : ITaskEvent
    {
        protected Int32 m_tasktypedefine;
        protected TaskEventState m_taskEventState;
        protected ulong taskid;
        protected Dictionary<int, int> taskcondition;

        protected ILevelActorComponentBaseContainer levelActor;
        public TaskEventBase(ILevelActorComponentBaseContainer levelActor)
        {
            taskcondition = new Dictionary<int, int>();
            this.levelActor = levelActor;
        }
        public int GetTaskTypeDefine()
        {
            return m_tasktypedefine;
        }

        public TaskEventState GetTaskState()
        {
            return m_taskEventState;
        }

        public void TickTask()
        {
            //TODO 进行任务逻辑判断
        }

        public ulong GetTaskId()
        {
            return taskid;
        }

        public void SetValue(int key, object value)
        {

        }

        public void SetValue(int key, bool value)
        {
        }

        /// <summary>
        /// 用此方法设置值时会向关卡发布 任务信息更改的信息
        /// </summary>
        public void SetValue(int key, Int32 value)
        {
            if (taskcondition.ContainsKey(key))
            {
                taskcondition[key] = value;
                levelActor.GetEventComponentBase()
                    .AddForWardEventMessages(new TaskUpdateEventMessage(taskid, m_taskEventState, key, value));
            }

        }
    }



    
}
