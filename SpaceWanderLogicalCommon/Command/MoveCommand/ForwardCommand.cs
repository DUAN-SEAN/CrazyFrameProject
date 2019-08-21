
using System;

namespace GameActorLogic
{
    [Serializable]
    public class ForwardCommand : Command
    {
        /// <summary>
        /// 
        /// </summary>
        public double ang = 0.05;
        /// <summary>
        /// actorId
        /// </summary>
        public ulong actorid;
        public ForwardCommand(ulong actorid)
        {
            currenttime = DateTime.Now.Ticks;
            _commandtype = CommandConstDefine.ForwardCommand;
            this.actorid = actorid;
        }

        public ForwardCommand(ulong actorid, double ang = 0.05)
        {
            currenttime = DateTime.Now.Ticks;
            _commandtype = CommandConstDefine.ForwardCommand;
            this.ang = ang;
            this.actorid = actorid;
        }

        public override string tostring()
        {
            return base.tostring() ;
        }
    }
}
