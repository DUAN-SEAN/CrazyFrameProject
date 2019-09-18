using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class EliteShipAAiComponent : AIComponentBase
    {
        private ILevelActorBaseContainer level;

        protected int delytime;
        protected int initnum;

        protected long currenttime;
        public EliteShipAAiComponent(IBaseComponentContainer container, int delytime,int initnum) : base(container)
        {
            level = container.GetLevel();
            this.delytime = delytime;
            this.initnum = initnum;
            currenttime = DateTime.Now.Ticks;
        }

        public EliteShipAAiComponent(EliteShipAAiComponent clone, IBaseComponentContainer container) : base(clone, container)
        {
            level = container.GetLevel();
            delytime = clone.delytime;
            initnum = clone.initnum;
            currenttime = DateTime.Now.Ticks;
        }

        public override AIComponentBase Clone(IBaseComponentContainer container)
        {
            return new EliteShipAAiComponent(this, container);
        }

        public override void TickLogical()
        {
            if(currenttime + delytime * 1e4 < DateTime.Now.Ticks)
            {
                var p = container.GetPosition();
                for(int i = 0;i < initnum; i++)
                {
                    level.AddEventMessagesToHandlerForward(new InitEventMessage(((ILevelActorComponentBaseContainer)level).GetCreateInternalComponentBase().GetCreateID(), container.GetCamp(), ActorTypeBaseDefine.DroneShipActor, p.X, p.Y, container.GetForwardAngle(), container.GetLinerDamping()));
                }
                currenttime = DateTime.Now.Ticks;
            }

        }

    }
}
