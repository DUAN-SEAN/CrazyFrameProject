using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    [Serializable]
    public class RightCommand : Command
    {
        public double Rightang = -0.0000001;
        public ulong actorid;
        public RightCommand(ulong actorid)
        {
            _commandtype = CommandConstDefine.ForwardCommand;
            this.actorid = actorid;
        }

        public RightCommand(ulong actorid, double rightang)
        {
            _commandtype = CommandConstDefine.ForwardCommand;
            Rightang = rightang;
            this.actorid = actorid;
        }
    }
}
