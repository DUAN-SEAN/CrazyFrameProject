using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class ThrustCommand : Command
    {
        public float Thrustproc = 0.00001f;
        public ulong actorid;

        public ThrustCommand(ulong actorid)
        {
            _commandtype = CommandConstDefine.ThrustCommand;
            this.actorid = actorid;
        }

        public ThrustCommand(ulong actorid, float thrustproc)
        {
            _commandtype = CommandConstDefine.ThrustCommand;
            Thrustproc = thrustproc;
            this.actorid = actorid;
        }
    }
}
