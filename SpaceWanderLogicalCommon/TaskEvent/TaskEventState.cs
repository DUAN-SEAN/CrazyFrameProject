using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    [Serializable]
    public enum TaskEventState
    {
        Idle = 0,
        UnFinished = 1,
        Finished = 2,
    }
}
