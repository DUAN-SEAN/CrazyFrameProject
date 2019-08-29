using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class GogogoAiComponent : AIComponentBase
    {
      

        public GogogoAiComponent(IBaseComponentContainer container) : base(container)
        {
            container.GetPhysicalinternalBase().GetBody().Velocity = container.GetForward() * 2;

        }

        public GogogoAiComponent(GogogoAiComponent clone, IBaseComponentContainer container) : base(clone, container)
        {
            
            container.GetPhysicalinternalBase().GetBody().Velocity = container.GetForward() * 2;
        }

        public override AIComponentBase Clone(IBaseComponentContainer container)
        {
            return new GogogoAiComponent(this,container);
        }

        public override void TickLogical()
        {
            base.TickLogical();
        }
    }
}
