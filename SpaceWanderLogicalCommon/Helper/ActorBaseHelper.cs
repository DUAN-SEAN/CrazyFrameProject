using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Common;

namespace GameActorLogic
{
    public static class ActorBaseHelper
    {
        public static void PrepareActor(this ActorBase actor, double point_x, double point_y, double angle)
        {
            var container = actor as IBaseComponentContainer;
            var body = container.GetPhysicalinternalBase().GetBody();
            body.Position = new Point(point_x, point_y);
            body.InitAngle(angle);
            body.Angle = angle;
        }
    }
}
