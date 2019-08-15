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

        public ITaskEvent GetTaskEvent(ulong id)
        {
            return taskEvents.Find(t => t.GetTaskId() == id);
        }

        public void ActivateTask(ulong id)
        {
            taskEvents.Find(t => t.GetTaskId() == id).ActivateTask();
        }

        public void Update()
        {
            foreach (var taskEvent in taskEvents)
            {
                if(taskEvent.GetTaskState() == TaskEventState.UnFinished)
                    taskEvent.TickTask();
            }
        }

    }
}
