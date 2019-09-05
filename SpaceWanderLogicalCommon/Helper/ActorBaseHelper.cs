using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Box2DSharp.Dynamics;
using Crazy.Common;


namespace GameActorLogic
{
    public static class ActorBaseHelper
    {
        public static void PrepareActor(this ActorBase actor, float point_x, float point_y, float angle)
        {
            var container = actor as IBaseComponentContainer;
            container.SetInitData(point_x, point_y, angle);
            //Log.Trace("PrepareActor 角度值:" + body.Angle);
            //body.Angle = angle;
        }


        public static List<Body> ToBodyList(this List<ActorBase> actors)
        {
            return actors.ConvertAll(o => ((IBaseComponentContainer) o).GetPhysicalinternalBase().GetBody());
        }

        public static void Detection(this Body body,IBaseContainer actor)
        {
            var point = body.GetPosition();
            actor.SetInitData(point.X, point.Y, body.GetAngle());
        }


    }
}
