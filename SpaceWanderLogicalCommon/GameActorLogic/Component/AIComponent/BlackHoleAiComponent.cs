using Box2DSharp.Dynamics;
using Box2DSharp.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class BlackHoleAiComponent : AIComponentBase
    {
        private ILevelActorBaseContainer level;
        
        /// <summary>
        /// 吸力半径
        /// </summary>
        private float attractRadius;

        /// <summary>
        /// 吸力
        /// </summary>
        private float attractForce;
        public BlackHoleAiComponent(IBaseComponentContainer container,float attractradius,float attractforce) : base(container)
        {
            level = container.GetLevel();
            attractRadius = attractradius;
            attractForce = attractforce;
        }

        public BlackHoleAiComponent(BlackHoleAiComponent clone, IBaseComponentContainer container) : base(clone, container)
        {
            level = container.GetLevel();
            attractRadius = clone.attractRadius;
            attractForce = clone.attractForce;
        }

        public override AIComponentBase Clone(IBaseComponentContainer container)
        {
            return new BlackHoleAiComponent(this, container);
        }

        public override void TickLogical()
        {
            if (level == null) return;
            Body body = container.GetPhysicalinternalBase().GetBody();
            if (body == null) return;
            var list = body.CircleDetection(level.GetAllActors().ToBodyList(), attractRadius);

            foreach(var b in list)
            {
                b.Attract(body.GetPosition(), attractForce);
            }



        }




    }
}
