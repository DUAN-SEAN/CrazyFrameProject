using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class LevelStartTaskCondition : ITaskCondition
    {
        protected bool IsLevelStart;

        protected ILevelActorBaseContainer level;

        protected Dictionary<int, int> Currentvalue;
        public Dictionary<int, int> ConditionCurrentValues
        {
            get => Currentvalue;
            set => Currentvalue = value;
        }


        public LevelStartTaskCondition(ILevelActorBaseContainer level)
        {
            this.level = level;
            IsLevelStart = false;
            Currentvalue = new Dictionary<int, int>();
        }
        
        public void Dispose()
        {
        }

        public void StartCondition()
        {
            IsLevelStart = level.GetLevelState();
        }

        public bool TickCondition()
        {
            IsLevelStart = level.GetLevelState();
            return IsLevelStart;
        }

        public int GetCurrentValue()
        {
            return IsLevelStart ? 1 : 0;
        }

        public int GetTargetValue()
        {
            return 1;
        }
    }
}
