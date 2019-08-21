using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    [Serializable]
    public class LeftCommand: Command
    {
        public double Leftang = 0.0000001;
        public ulong actorid;
        public LeftCommand(ulong actorid)
        {
            currenttime = DateTime.Now.Ticks;
            _commandtype = CommandConstDefine.ForwardCommand;
            this.actorid = actorid;
        }

        public LeftCommand(ulong actorid,double leftang = 0.0000001)
        {
            currenttime = DateTime.Now.Ticks;
            _commandtype = CommandConstDefine.ForwardCommand;
            Leftang = leftang;
            this.actorid = actorid;
        }
    }
}
