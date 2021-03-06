﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DSharp.External;
using Crazy.Common;

namespace GameActorLogic
{
    public class GogogoAiComponent : AIComponentBase
    {
        protected float aiforce;

        public GogogoAiComponent(IBaseComponentContainer container,float aiforce) : base(container)
        {
            this.aiforce = aiforce;
        }

        public GogogoAiComponent(GogogoAiComponent clone, IBaseComponentContainer container) : base(clone, container)
        {
            this.aiforce = clone.aiforce;
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
                container.AddThrust(aiforce);
                //Log.Trace("武器Actor id" + container.GetActorID() + " 施加力"+ aiforce + " 角度：" + container.GetForwardAngle() + "方向:" + container.GetForward() + " 坐标" + container.GetPosition());
                i = 1;
            }
        }
    }
}
