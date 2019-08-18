using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.External;

namespace GameActorLogic
{
    public class DeadAiComponent:AIComponentBase
    {
        protected long time;
        protected long inittime;

        public DeadAiComponent(long time ,IBaseComponentContainer container) : base(container)
        {
            inittime = DateTime.Now.Ticks;
            this.time = time;
        }


        public override void TickLogical()
        {
            base.TickLogical();

            if (inittime + time > DateTime.Now.Ticks)
            {
                if(container is IShipComponentBaseContainer ship)
                {
                    ship.Destroy();
                }
                if(container is IWeaponBaseComponentContainer weapon)
                    weapon.Destroy();
            }

        }
    }
}
