
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
#pragma warning disable CS1717 // 对同一变量进行赋值；是否希望对其他变量赋值?
            ang = ang;
#pragma warning restore CS1717 // 对同一变量进行赋值；是否希望对其他变量赋值?
            this.actorid = actorid;
        }
    }
}
