using Crazy.Common;
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

        /// <summary>
        /// 任务描述
        /// </summary>
        protected string taskDescription;



        public TaskEventBase(int taskid,ILevelActorComponentBaseContainer levelActor,Int32 condition,Int32 result,Dictionary<int,int> values, string desc)
        {
            this.taskid = taskid;
            this.levelActor = levelActor;
            taskcondition = new Dictionary<int, int>();
            foreach (var i in values)
            {
                AddValue(i.Key, i.Value);
            }
            m_taskconditiontypedefine = condition;
            m_taskresulttypedefine = result;

            taskCondition = CreateTaskCondition(m_taskconditiontypedefine);
            taskResult = CreateTaskResult(m_taskresulttypedefine);

            taskDescription = desc;
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
                case TaskConditionTypeConstDefine.EnemyInitTask:
                    taskcondition = new EnemyInitTaskCondition(this,levelActor);
                    break;
                case TaskConditionTypeConstDefine.InitByPositionTask:
                    taskcondition = new InitByPositionTaskCondition(this, levelActor, 0, 1, 2);
                    break;
                case TaskConditionTypeConstDefine.LevelStartEvent:
                    taskcondition = new LevelStartTaskCondition(levelActor);
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
                    if (!taskcondition.TryGetValue(1, out int value)){
                        Log.Trace("TaskEventBase: task id" + taskid + " 未获取到激活任务的ID");
                    }
                    taskresult = new ActivateTaskResult(levelActor,value);
                    break;
                case TaskResultTypeConstDefine.Fail:
                    taskresult = new FailTaskResult(levelActor);
                    break;
                case TaskResultTypeConstDefine.InitActor:
                    taskresult = new InitActorTaskResult(this, levelActor, 0, 1, 2);
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
            StartTaskEvent();
            return true;

        }

        public void TickTask()
        {
            //Log.Trace("tick task:" + taskid);
            if (m_taskEventState == TaskEventState.Idle) return;
            if(m_taskEventState == TaskEventState.Finished) return;
            //Log.Trace("task:"+taskid + "开始tick");
            //任务已激活 但是未完成 判断条件是否达成
            if (taskCondition == null) return;
            if (!taskCondition.TickCondition()) return;
            //Log.Trace("任务完成 任务id" + taskid);
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
            //Log.Trace("添加值成功 key:" + key + " value:" + value);
            return true;
        }

        public bool TryGetValue(int key, out int value)
        {
            return taskcondition.TryGetValue(key, out value);
        }

        public Dictionary<int, int> GetTaskValues()
        {
            return taskcondition;
        }

        public void Dispose()
        {
            levelActor = null;
            taskCondition?.Dispose();
            taskCondition = null;
            taskResult?.Dispose();
            taskResult = null;
            taskcondition?.Clear();
            taskcondition = null;
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

        public void StartTaskEvent()
        {
            taskCondition?.StartCondition();
            taskResult?.StartResult();
            m_taskEventState = TaskEventState.UnFinished;
        }

        public string GetTaskDescription()
        {
            return taskDescription;
        }

        public int GetCurrentValue()
        {
            return taskCondition.GetCurrentValue();
        }

        public int GetTargetValue()
        {
            return taskCondition.GetTargetValue();
        }

        #endregion


    }



    
}
