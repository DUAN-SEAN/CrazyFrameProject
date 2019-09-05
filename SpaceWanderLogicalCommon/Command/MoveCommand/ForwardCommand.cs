
using System;

namespace GameActorLogic
{
    [Serializable]
    public class ForwardCommand : Command
    {
        /// <summary>
        /// 
        /// </summary>
        public float ang = 0.005f;
        /// <summary>
        /// actorId
        /// </summary>
        public ulong actorid;
        public ForwardCommand(ulong actorid)
        {
            _commandtype = CommandConstDefine.ForwardCommand;
            this.actorid = actorid;
        }

        public ForwardCommand(ulong actorid, float ang = 0.005f)
        {
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
