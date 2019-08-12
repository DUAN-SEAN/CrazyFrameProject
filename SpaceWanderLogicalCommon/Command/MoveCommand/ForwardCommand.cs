
using System;

namespace GameActorLogic
{
    [Serializable]
    public class ForwardCommand : Command
    {
        public double ang = 0.0000001;
        public ulong actorid;
        public ForwardCommand(ulong actorid)
        {
            _commandtype = CommandConstDefine.ForwardCommand;
            this.actorid = actorid;
        }

        public ForwardCommand(ulong actorid, double ang)
        {
            _commandtype = CommandConstDefine.ForwardCommand;
            ang = ang;
            this.actorid = actorid;
        }
    }
}
