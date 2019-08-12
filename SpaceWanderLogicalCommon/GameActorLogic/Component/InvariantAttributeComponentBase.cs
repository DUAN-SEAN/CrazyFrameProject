using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    /// <summary>
    /// 基础不变类型组件
    /// </summary>
    public class InvariantAttributeComponentBase : 
        IInvariantAttributeBase,
        IInvariantAttributeinternalBase
    {
        /// <summary>
        /// 移动的最大速度
        /// </summary>
        protected double maxSpeed;

        /// <summary>
        /// 移动最大推力
        /// </summary>
        protected float maxForceProc;


        public InvariantAttributeComponentBase(double maxSpeed  = 2,float maxForceProc = 0.0002f)
        {
            this.maxSpeed = maxSpeed;
            this.maxForceProc = maxForceProc;
        }

        #region IInvariantAttributeinternalBase

        public double GetMaxSpeed()
        {
            return maxSpeed;
        }

        public float GetMaxForceProc()
        {
            return maxForceProc;
        }

        #endregion


    }
}
