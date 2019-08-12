using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    public class LeftCommand: MoveCommand
    {
        public double Leftang = 0.0000001;
        public long actorid;
        public LeftCommand(long actorid)
        {
            _commandtype = CommandConstDefine.LeftCommand;
            this.actorid = actorid;
        }

        public LeftCommand(long actorid,double leftang = 0.0000001)
        {
            _commandtype = CommandConstDefine.LeftCommand;
            Leftang = leftang;
            this.actorid = actorid;
        }
    }
}
