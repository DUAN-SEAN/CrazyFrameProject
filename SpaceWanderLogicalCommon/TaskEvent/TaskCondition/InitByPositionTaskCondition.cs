using Crazy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class InitByPositionTaskCondition : ITaskCondition
    {
        protected readonly int key_actortype;
        protected readonly int key_x;
        protected readonly int key_y;
        private ITaskEvent taskEventBase;
        protected ILevelActorComponentBaseContainer level;

        protected bool isInitdone;

        protected Dictionary<int, int> Currentvalue;

        public Dictionary<int, int> ConditionCurrentValues
        {
            get => Currentvalue;
            set => Currentvalue = value;
        }

        public InitByPositionTaskCondition(TaskEventBase taskEventBase, ILevelActorComponentBaseContainer levelActor,int key_actortype, int key_x,int key_y)
        {
            this.taskEventBase = taskEventBase;
            this.level = levelActor;
            Currentvalue = new Dictionary<int, int>();
            isInitdone = false;
            this.key_actortype = key_actortype;
            this.key_x = key_x;
            this.key_y = key_y;
        }


        public void Dispose()
        {
            Currentvalue.Clear();
            Currentvalue = null;
            level = null;
            taskEventBase = null;
        }

        public int GetCurrentValue()
        {
            return isInitdone?1:0;
        }

        public int GetTargetValue()
        {
            return 1;
        }
        public void StartCondition()
        {
            if (!taskEventBase.TryGetValue(key_actortype, out int actortype)) return;
            if (!taskEventBase.TryGetValue(key_x, out int x)) return;
            if (!taskEventBase.TryGetValue(key_y, out int y)) return;

            ulong id = level.GetCreateInternalComponentBase().GetCreateID();
            level.AddEventMessagesToHandlerForward(new InitEventMessage(actorid: id, actortype: actortype, camp: LevelActorBase.WallCamp, point_x: x, point_y: y, angle: 0, LinerDamping: 0.1f));
            //Log.Trace("StartCondition InitActorID" + id);
            isInitdone = true;
        }

        public bool TickCondition()
        {
            return isInitdone;
        }
    }
}
