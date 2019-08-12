
namespace GameActorLogic
{
    public class ForwardCommand : Command
    {
        public double ang = 0.0000001;
        public long actorid;
        public ForwardCommand(long actorid)
        {
            _commandtype = CommandConstDefine.ForwardCommand;
            this.actorid = actorid;
        }

        public ForwardCommand(long actorid, double ang)
        {
            _commandtype = CommandConstDefine.ForwardCommand;
            ang = ang;
            this.actorid = actorid;
        }
    }
}
