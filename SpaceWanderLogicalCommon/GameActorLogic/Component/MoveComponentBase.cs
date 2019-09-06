using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Box2DSharp.Common;
using Box2DSharp.External;
using Crazy.Common;
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

       public void Dispose()
       {
           physical = null;
       }

        #region IMoveBase
        public void Left(float proc)
        {
            physical?.AddForward(proc);
            OnLeft?.Invoke(proc);
        }

        public void Right(float proc)
        {
            physical?.AddForward(proc);
            OnRight?.Invoke(proc);
        }

        public void AddThrust(float ang = 0.000001f)
        {
            physical?.AddThrust(ang);
            OnThrust?.Invoke(ang);
        }

        public void Remote(float x, float y)
        {
            if (x < 0.005 && y < 0.005 && x > -0.005 && y > -0.005) return;
            if (physical?.GetBody() == null) return;
            //Log.Trace("Remote 操作值"+x+" "+y);


            var point = new Vector2(x, y);

            Vector2 forward = physical.GetBody().GetForward();

            //算出力的大小
            var cos = CrazyUtils.IncludedAngleCos(point, forward);
            float angle = (float)Math.Acos(cos);
            float anglepro = (float) (angle / Math.PI);
            float forcepro = (float)Vector2.DistanceSquared(Vector2.Zero, point);


            physical?.AddThrust(100f* 5  * anglepro * forcepro);

            physical?.GetBody().MoveForward(point);

            

        }

        #endregion

        #region IMoveInternalBase
        public event Action<float> OnLeft;
        public event Action<float> OnRight;
        public event Action<float> OnThrust;
        #endregion



    }
}
