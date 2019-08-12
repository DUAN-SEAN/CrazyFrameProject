using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    public class RightCommand : Command
    {
        public double Rightang = -0.0000001;
        public long actorid;
        public RightCommand(long actorid)
        {
            _commandtype = CommandConstDefine.ForwardCommand;
            this.actorid = actorid;
        }

        public RightCommand(long actorid, double rightang)
        {
            _commandtype = CommandConstDefine.ForwardCommand;
            Rightang = rightang;
            this.actorid = actorid;
        }
    }
}
