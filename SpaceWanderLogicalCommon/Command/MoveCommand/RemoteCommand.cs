using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    [Serializable]
    public class RemoteCommand : Command
    {
        public float remote_x = 0f;
        public float remote_y = 0f;
        public ulong actorid;
        public RemoteCommand(ulong actorid)
        {
            _commandtype = CommandConstDefine.RemoteCommand;
            this.actorid = actorid;
        }

        public RemoteCommand(ulong actorid, float x,float y)
        {
            _commandtype = CommandConstDefine.RemoteCommand;
            remote_x = x;
            remote_y = y;
            this.actorid = actorid;
        }
    }
}
