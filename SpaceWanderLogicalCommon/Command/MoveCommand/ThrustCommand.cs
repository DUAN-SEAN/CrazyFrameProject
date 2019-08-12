using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class ThrustCommand : Command
    {
        public float Thrustproc = 0.0001f;
        public long actorid;

        public ThrustCommand(long actorid)
        {
            _commandtype = CommandConstDefine.ThrustCommand;
            this.actorid = actorid;
        }

        public ThrustCommand(long actorid, float thrustproc)
        {
            _commandtype = CommandConstDefine.ThrustCommand;
            Thrustproc = thrustproc;
            this.actorid = actorid;
        }
    }
}
