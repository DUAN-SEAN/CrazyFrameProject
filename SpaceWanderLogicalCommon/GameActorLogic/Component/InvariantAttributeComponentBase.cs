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

        public InvariantAttributeComponentBase(double maxSpeed  = 2)
        {
            this.maxSpeed = maxSpeed;
        }

        #region IInvariantAttributeinternalBase

        public double GetMaxSpeed()
        {
            return maxSpeed;
        }

        #endregion


    }
}
