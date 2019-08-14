using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Common;

namespace GameActorLogic
{
    public class CreateComponentBase :
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

        public ActorBase CreateActor(int actortype, double point_x, double point_y, double angle)
        {
            return CreateActor(actortype, point_x, point_y, angle, GetCreateID());
        }

        public ActorBase CreateActor(int actortype, double point_x, double point_y, double angle,ulong Id)
        {
            ActorBase actor = null;
            switch (actortype)
            {
                case ActorTypeBaseDefine.ActorNone:
                    //actor = new ActorBase();
                    break;

                #region 飞船Actor
                case ActorTypeBaseDefine.ShipActorNone:
                    actor = new ShipActorBase(Id);
                    actor.PrepareActor(point_x,point_y,angle);
                    break;

                #endregion
            }

            return actor;
        }

    }

  

}
