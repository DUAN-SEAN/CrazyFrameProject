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

        }

        public GogogoAiComponent(GogogoAiComponent clone, IBaseComponentContainer container) : base(clone, container)
        {
            
        }

        public override AIComponentBase Clone(IBaseComponentContainer container)
        {
            return new GogogoAiComponent(this,container);
        }

        public override void TickLogical()
        {
            base.TickLogical();
            container.AddThrust();
        }
    }
}
