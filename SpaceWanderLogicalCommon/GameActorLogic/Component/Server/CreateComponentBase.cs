using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class CreateComponentBase:
        ICreateComponentBase,
        ICreateInternalComponentBase
    {
        protected ulong IDs;

        public CreateComponentBase()
        {
            IDs = 0;
        }

        public ulong GetCreateID()
        {
            return IDs++;
        }

        public ActorBase CreateActor(int actortype, double point_x, double point_y,double angle)
        {
            ActorBase actor = null;
            switch (actortype)
            {
                case ActorTypeBaseDefine.ActorNone:
                    //actor = new ActorBase();
                    break;
                case ActorTypeBaseDefine.ShipActorNone:
                    actor = new ShipActorBase(GetCreateID());
                    break;
                    
            }

            return actor;
        }
    }
}
