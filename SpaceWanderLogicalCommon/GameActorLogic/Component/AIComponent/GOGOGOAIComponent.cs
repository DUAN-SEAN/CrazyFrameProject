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


        public override void TickLogical()
        {
            base.TickLogical();
            container.AddThrust();
        }
    }
}
