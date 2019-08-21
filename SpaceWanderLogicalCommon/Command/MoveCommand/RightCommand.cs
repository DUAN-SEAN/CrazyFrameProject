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
            currenttime = DateTime.Now.Ticks;
            _commandtype = CommandConstDefine.ForwardCommand;
            this.actorid = actorid;
        }

        public RightCommand(ulong actorid, double rightang)
        {
            currenttime = DateTime.Now.Ticks;

            _commandtype = CommandConstDefine.ForwardCommand;
            Rightang = rightang;
            this.actorid = actorid;
        }
    }
}
