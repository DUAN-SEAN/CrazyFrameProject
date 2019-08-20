
using System;

namespace GameActorLogic
{
    [Serializable]
    public class ForwardCommand : Command
    {
        public double ang = 0.05;
        public ulong actorid;
        public ForwardCommand(ulong actorid)
        {
            _commandtype = CommandConstDefine.ForwardCommand;
            this.actorid = actorid;
        }

        public ForwardCommand(ulong actorid, double ang = 0.05)
        {
            _commandtype = CommandConstDefine.ForwardCommand;
            this.ang = ang;
            this.actorid = actorid;
        }
    }
}
