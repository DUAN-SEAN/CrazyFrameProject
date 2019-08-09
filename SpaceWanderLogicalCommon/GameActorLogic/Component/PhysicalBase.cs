using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Base;
using CrazyEngine.Common;
using CrazyEngine.Core;
using GameActorLogic;

namespace GameActorLogic
{

    /// <summary>
    /// 物理信息组件
    /// 可能会与碰撞组件合并成一个类 然后继承两种接口
    /// </summary>
    public abstract class PhysicalBase:
        IPhysicalBase,
        IPhysicalinternalBase,
        IColliderBase,
        IColliderinternal
    {
        protected Body m_body;
        protected IEnvirinfoBase envirinfo;
        protected PhysicalBase()
        {
            m_body = Factory.CreateCircleBody(1, 1, 1);
        }

        /// <summary>
        /// body应该在传入之前就添加进engine中
        /// 该传入的engine用于将新的对象进行创建
        /// 应该会放入其他接口
        /// </summary>
        protected PhysicalBase(Body body, IEnvirinfoBase envirinfo)
        {
            this.envirinfo = envirinfo;
            m_body = body;
            
        }

       


        #region IPhysicalBase
        public Point GetPosition()
        {
            return m_body.Position;
        }

        public Point GetForward()
        {
            return m_body.Forward;
        }

        public double GetSpeed()
        {
            return m_body.AngularVelocity;
        }
        #endregion

        #region IPhysicalinternalBase

        /// <summary>
        /// 给飞船一个与朝向相同的推力
        /// </summary>
        public void AddThrust(float proc = 0.0001f)
        {
            m_body.Force += m_body.Mass * m_body.Forward * /*engine.World.Gravity.Scaling * */  proc;
        }

        /// <summary>
        /// 给一个弧度
        /// 左右以正负来描述
        /// 默认0.001
        /// </summary>
        public void AddForward(double angular)
        {
            m_body.AngularVelocity += angular;
        }
        #endregion


        public event Action OnColliderEnter;
        public event Action OnColliderStay;
        public event Action OnColliderExit;
    }
}
