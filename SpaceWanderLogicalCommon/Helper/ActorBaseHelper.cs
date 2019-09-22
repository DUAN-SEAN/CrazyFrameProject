using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Box2DSharp.Dynamics;
using Box2DSharp.External;
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

        public static void Detection(this IBaseComponentContainer body,IBaseContainer actor)
        {
            var body1 = body.GetPhysicalinternalBase().GetBody();
            var nose = body1.GetSpaceShipNosePosition(body.GetGameModelByActorType());
            var result = nose;

            actor.SetInitData(result.X, result.Y, body.GetForwardAngle());
        }

        public static void RingDetection(this IBaseComponentContainer body, IBaseComponentContainer actor)
        {
            var body1 = body.GetPhysicalinternalBase().GetBody();
            var nose = body1.GetSpaceShipNosePosition(body.GetGameModelByActorType());
            //var height = 
            actor.GetPhysicalinternalBase().GetBody().SetTransform(nose  , body.GetForwardAngle());
        }
    }
}
