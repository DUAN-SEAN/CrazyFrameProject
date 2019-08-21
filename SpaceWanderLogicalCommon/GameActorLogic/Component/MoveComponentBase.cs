using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Common;
using CrazyEngine.Core;
using GameActorLogic;

namespace GameActorLogic
{
    /// <summary>
    /// 基础移动组件
    /// </summary>
    public class MoveComponentBase : 
        IMoveBase,
        IMoveInternalBase
    {
        protected IPhysicalInternalBase physical;

        public MoveComponentBase(IPhysicalInternalBase physical)
        {
            this.physical = physical;
        }



        #region IMoveBase
        public void Left(double proc)
        {
            physical?.AddForward(proc);
            OnLeft?.Invoke(proc);
        }

        public void Right(double proc)
        {
            physical?.AddForward(proc);
            OnRight?.Invoke(proc);
        }

        public void AddThrust(float ang = 0.0001f)
        {
            physical?.AddThrust(ang);
            OnThrust?.Invoke(ang);
        }

        public void Remote(float x, float y)
        {
            if (x < 0.005 && y < 0.005 && x > -0.005 && y > -0.005) return;
            if (physical?.GetBody() == null) return;
            var point = new Point(x, y);

            var cross = point.Cross(physical?.GetBody().Forward);
            //算出力的大小
            var cos = point.IncludedAngle(physical?.GetBody().Forward);
            float angle = (float)Math.Acos(cos);
            float anglepro = (float) (angle / Math.PI);
            float forcepro = (float)Helper.DistanceNoSqrt(Point.Zero(), point);
            physical?.AddThrust(0.00001f * 5 * anglepro * forcepro);
            if ( cross > 0)
            {
                physical?.AddForward(0.05);
            }
            else if (cross < 0)
            {
                physical?.AddForward(-0.05);
            }
        }

        #endregion

        #region IMoveInternalBase
        public event Action<double> OnLeft;
        public event Action<double> OnRight;
        public event Action<float> OnThrust;
        #endregion



    }
}
