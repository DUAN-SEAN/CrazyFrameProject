using Crazy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class TaskEventComponentBase:
        ITaskEventComponentBase,
        ITaskEventComponentInternalBase
    {
        protected ILevelActorComponentBaseContainer levelcontainer;
        protected List<ITaskEvent> taskEvents;


        public TaskEventComponentBase(ILevelActorComponentBaseContainer level)
        {
            levelcontainer = level;
            taskEvents = new List<ITaskEvent>();
        }

        public void Dispose()
        {
            levelcontainer = null;
            foreach (var taskEvent in taskEvents)
            {
                taskEvent.Dispose();
            }
            taskEvents.Clear();
            taskEvents = null;
        }
        public void AddTaskEvent(ITaskEvent task)
        {
            taskEvents.Add(task);
        }

        public List<ITaskEvent> GetAllTaskEvents()
        {
            return taskEvents;
        }

        public List<ITaskEvent> GetUnFinishTaskEvents()
        {
            List<ITaskEvent> events = new List<ITaskEvent>();
            foreach (var taskEvent in taskEvents)
            {
                if (taskEvent.GetTaskState() == TaskEventState.UnFinished)
                {
                    events.Add(taskEvent);
                }
            }
            return events;
        }

        public ITaskEvent GetTaskEvent(int id)
        {
            return taskEvents.Find(t => t.GetTaskId() == id);
        }

        public void SetTaskConditionAndState(int id, int state, Dictionary<int, int> values)
        {
            var taskevent = taskEvents.Find(t => t.GetTaskId() == id);
            taskevent.ConditionCurrentValues = values;
            taskevent.SetTaskState((TaskEventState) state);
        }

        public void ActivateTask(int id)
        {
            var task = taskEvents.Find(t => t.GetTaskId() == id);
            if(task == null)
            {
                Log.Trace("TaskEventComponentBase: ActivateTask 未找到要激活的任务 id：" + id);
                return;
            }
            task.ActivateTask();
        }

        public void Update()
        {
            foreach (var taskEvent in taskEvents)
            {
                if(taskEvent.GetTaskState() == TaskEventState.UnFinished)
                {
                    //Log.Trace("task event 任务tick" + taskEvent.GetTaskId());
                    taskEvent.TickTask();

                }
            }
        }

        public void StartTaskEvents()
        {
            foreach (var taskEvent in taskEvents)
            {
                if (taskEvent.GetTaskConditionTypeDefine() == TaskConditionTypeConstDefine.LevelStartEvent)
                {
                    taskEvent.StartTaskEvent();
                }
            }
        }
    }
}
