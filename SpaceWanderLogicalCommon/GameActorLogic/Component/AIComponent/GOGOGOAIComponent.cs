
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;

namespace GameActorLogic
{
    public class GogogoAiComponent : AIComponentBase
    {
      

        public GogogoAiComponent(IBaseComponentContainer container) : base(container)
        {
            //container.GetPhysicalinternalBase().GetBody().Velocity = container.GetForward() * 10;
        }

        public GogogoAiComponent(GogogoAiComponent clone, IBaseComponentContainer container) : base(clone, container)
        {
            //container.GetPhysicalinternalBase().GetBody().Velocity = container.GetForward() * 10;
        }

        public override AIComponentBase Clone(IBaseComponentContainer container)
        {
            return new GogogoAiComponent(this,container);
        }

        public int i = 0;
        public override void TickLogical()
        {
            base.TickLogical();
            //Log.Trace("武器Actor id"+container.GetActorID()+" Body id："+container.GetBodyId()+" 施加力"+"角度："+container.GetForwardAngle()+"方向:" + container.GetForward() + " 坐标" + container.GetPosition());
            if(i==0)
            {
                //container.GetPhysicalinternalBase().GetBody().Velocity = container.GetForward() * 10;
                //Log.Trace("武器Actor id" + container.GetActorID() + " Body id：" + container.GetBodyId() + " 施加力" + "角度：" + container.GetForwardAngle() + "方向:" + container.GetForward() + " 坐标" + container.GetPosition());
                i = 1;
            }
        }
    }
}
