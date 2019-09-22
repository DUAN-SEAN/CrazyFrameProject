using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 放入Box2D的UserData类型
    /// </summary>
    public class UserData
    {
        public ulong ActorID;
        public Int32 ActorType;

        public UserData(ulong ActorID,Int32 ActorType)
        {
            this.ActorID = ActorID;
            this.ActorType = ActorType;
        }
        public UserData(UserData clone)
        {
            ActorID = clone.ActorID;
            ActorType = clone.ActorType;
        }

        public override string ToString()
        {
            return "UserData :ActorID " + ActorID + " ActorType:" + ActorType;
        }
    }
}
