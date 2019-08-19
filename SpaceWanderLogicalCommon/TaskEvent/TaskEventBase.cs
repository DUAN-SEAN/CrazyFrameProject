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
        #region 任务字段
        /// <summary>
        /// 任务条件类型
        /// </summary>
        protected Int32 m_taskconditiontypedefine;

        /// <summary>
        /// 任务结果类型
        /// </summary>
        protected Int32 m_taskresulttypedefine;

        /// <summary>
        /// 任务当前状态
        /// </summary>
        protected TaskEventState m_taskEventState;
        /// <summary>
        /// 任务ID
        /// </summary>
        protected int taskid;

        /// <summary>
        /// 任务数据
        /// </summary>
        protected Dictionary<int, int> taskcondition;

        /// <summary>
        /// 关卡对象引用
        /// </summary>
        protected ILevelActorComponentBaseContainer levelActor;

        #endregion

        #region 任务策略对象
        /// <summary>
        /// 任务条件
        /// </summary>
        protected ITaskCondition taskCondition;
        /// <summary>
        /// 任务结果
        /// </summary>
        protected ITaskResult taskResult;
        
        #endregion




        public TaskEventBase(int taskid,ILevelActorComponentBaseContainer levelActor,Int32 condition,Int32 result)
        {
            this.taskid = taskid;
            taskcondition = new Dictionary<int, int>();
            this.levelActor = levelActor;
            m_taskconditiontypedefine = condition;
            m_taskresulttypedefine = result;
            taskCondition = CreateTaskCondition(m_taskconditiontypedefine);
            taskResult = CreateTaskResult(m_taskresulttypedefine);
        }
        /// <summary>
        /// 返回对应类型的条件
        /// </summary>
        protected ITaskCondition CreateTaskCondition(Int32 condition)
        {
            ITaskCondition taskcondition = null;
            switch (condition)
            {
                case TaskConditionTypeConstDefine.TaskEventNone:
                    break;
                case TaskConditionTypeConstDefine.TimeTaskEvent:
                    taskcondition = new TimeTaskCondition(this,0);
                    break;
                case TaskConditionTypeConstDefine.KillTaskEvent:
                    taskcondition = new KillTaskCondition(this, 0,levelActor);
                    break;
            }
            return taskcondition;
        }

        protected ITaskResult CreateTaskResult(Int32 result)
        {
            ITaskResult taskresult = null;

            switch (result)
            {
                case TaskResultTypeConstDefine.TaskEventNone:
                    break;
                case TaskResultTypeConstDefine.Victory:
                    taskresult = new VictoryTaskResult(levelActor);
                    break;
                case TaskResultTypeConstDefine.ActivateTask:
                    break;
                case TaskResultTypeConstDefine.Fail:
                    taskresult = new VictoryTaskResult(levelActor);
                    break;
            }
            return taskresult;
        }

        #region ITaskEvent
        public int GetTaskConditionTypeDefine()
        {
            return m_taskconditiontypedefine;
        }

        public int GetTaskResultTypeDefine()
        {
            return m_taskresulttypedefine;
        }

        public TaskEventState GetTaskState()
        {
            return m_taskEventState;
        }

        public Dictionary<int, int> ConditionCurrentValues {
            get => taskCondition.ConditionCurrentValues;
            set => taskCondition.ConditionCurrentValues = value;

        }

        public bool ActivateTask()
        {
            if (m_taskEventState == TaskEventState.Finished) return false;
            if (m_taskEventState != TaskEventState.Idle) return false;
            m_taskEventState = TaskEventState.UnFinished;
            return true;

        }

        public void TickTask()
        {
 
            if(m_taskEventState == TaskEventState.Idle) return;
            if(m_taskEventState == TaskEventState.Finished) return;
            //任务已激活 但是未完成 判断条件是否达成
            if(taskCondition == null) return;
            if (!taskCondition.TickCondition()) return;
            //任务条件已达成
            //如果任务结果未执行 任务不显示已完成
            if(taskResult == null) return;
            taskResult.Execute();
            m_taskEventState = TaskEventState.Finished;
        }

        public int GetTaskId()
        {
            return taskid;
        }

        public bool AddValue(int key, int value)
        {
            if (taskcondition.ContainsKey(key)) return false;
            taskcondition.Add(key, value);
            return true;
        }

        public bool TryGetValue(int key, out int value)
        {
            return taskcondition.TryGetValue(key, out value);
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

        public void SetTaskState(TaskEventState state)
        {
            m_taskEventState = state;
        }

        #endregion


    }



    
}
