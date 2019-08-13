using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        #endregion

        #region IMoveInternalBase
        public event Action<double> OnLeft;
        public event Action<double> OnRight;
        public event Action<float> OnThrust;
        #endregion



    }
}
