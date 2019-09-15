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

        protected bool isThrust = false;
        protected bool isRemote = false;
        protected bool isLeft = false;
        protected bool isRight = false;

        public MoveComponentBase(IPhysicalInternalBase physical)
        {
            this.physical = physical;
        }

        public void Update()
        {
            if (physical.GetBody() == null) return;
           if(isRemote == false)
            {
                var body = physical.GetBody();
                body.ApplyAngularImpulse(-body.AngularVelocity * body.Inertia, true);
            }

            isRemote = false;
            isThrust = false;
            isLeft = false;
            isRemote = false;

        }


        public void Dispose()
       {
           physical = null;
       }

        #region IMoveBase
        public void Left(float proc)
        {
            isLeft = true;
            physical?.AddForward(proc);
            OnLeft?.Invoke(proc);
        }

        public void Right(float proc)
        {
            isRight = true;
            physical?.AddForward(proc);
            OnRight?.Invoke(proc);
        }

        public void AddThrust(float ang = 0.000001f)
        {
            isThrust = true;
            physical?.AddThrust(ang);
            OnThrust?.Invoke(ang);
        }

        public void Remote(float x, float y)
        {
            
            if (x < 0.005 && y < 0.005 && x > -0.005 && y > -0.005) return;
            if (physical?.GetBody() == null) return;
            isRemote = true;
            //Log.Trace("Remote 操作值"+x+" "+y);

            var point = new Vector2(x, y);

            //算出力的大小

            var o = Vector2.DistanceSquared(new Vector2(0, 0), point)/1f;
            
            
            physical?.AddThrust(500* o);

            physical?.GetBody().MoveForward(point,2);

            

        }

        #endregion

        #region IMoveInternalBase
        public event Action<float> OnLeft;
        public event Action<float> OnRight;
        public event Action<float> OnThrust;
        #endregion



    }
}
