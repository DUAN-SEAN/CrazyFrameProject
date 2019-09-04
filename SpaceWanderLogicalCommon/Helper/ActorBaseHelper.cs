using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using CrazyEngine.Base;
using CrazyEngine.Common;
using CrazyEngine.External;

namespace GameActorLogic
{
    public static class ActorBaseHelper
    {
        public static void PrepareActor(this ActorBase actor, double point_x, double point_y, double angle)
        {
            var container = actor as IBaseComponentContainer;
            var body = container.GetPhysicalinternalBase().GetBody();
            body.Position = new Point(point_x, point_y);
            body.SetForward(angle);
            //Log.Trace("PrepareActor 角度值:" + body.Angle);
            //body.Angle = angle;
        }


        public static List<Body> ToBodyList(this List<ActorBase> actors)
        {
            return actors.ConvertAll(o => ((IBaseComponentContainer) o).GetPhysicalinternalBase().GetBody());
        }

       
    }
}
