using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Base;
using CrazyEngine.Common;
using CrazyEngine.Core;
using CrazyEngine.External;
using GameActorLogic;

namespace GameActorLogic
{

    /// <summary>
    /// 物理信息组件
    /// 可能会与碰撞组件合并成一个类 然后继承两种接口
    /// </summary>
    public class PhysicalBase:
        IPhysicalBase,
        IPhysicalInternalBase,
        IColliderBase,
        IColliderInternal
    {
        protected Body m_body;
        protected Collider m_collider;
        protected IEnvirinfoBase envirinfo;

        //copy值以防物理清除值
        protected Point Force_copy = new Point();
        protected double Torque_copy;
        protected double angleVelocity_copy;
        /// <summary>
        /// 碰撞方法被
        /// </summary>
        protected bool isColliderStay;
        /// <summary>
        /// 判断这一帧碰撞方法是否被调用
        /// </summary>
        protected bool isColliderMethodEnter;

        //protected PhysicalBase()
        //{
        //m_body = Factory.CreateCircleBody(1, 1, 1);
        //    m_collider
        //}

        /// <summary>
        /// body应该在传入之前就添加进engine中
        /// 该传入的engine用于将新的对象进行创建
        /// 应该会放入其他接口
        /// </summary>
        public PhysicalBase(IEnvirinfoInternalBase envirinfo)
        {
            this.envirinfo = envirinfo;
            
            m_collider = new Collider();
        }

        /// <summary>
        /// 碰撞逻辑检测
        /// 调用相应的碰撞方法
        /// </summary>
        protected void OnCollision(Collision collision)
        {
            isColliderMethodEnter = true;
            if(isColliderStay == false)
            {
                OnColliderEnter?.Invoke();
                isColliderStay = true;
            }
            if (isColliderStay)
                OnColliderStay?.Invoke();
        }

        public void Update()
        {
           
            if (isColliderStay == true && isColliderMethodEnter == false)
            {
                isColliderStay = false;
                OnColliderExit?.Invoke();
            }

            //附上值
            Force_copy = m_body.Force;
            angleVelocity_copy = m_body.AngularVelocity;
            Torque_copy = m_body.Torque;
        }

        #region Helper

        public void CreateBody(Body body)
        {
            m_body = body;
            m_collider.OnCollisionStay += OnCollision;
        }

        #endregion


        #region IPhysicalBase

        #region 物理同步

        public Point GetForward()
        {
            return m_body.Forward;
        }

        public Point GetVelocity()
        {
            return m_body.Velocity;
        }

        public void InitializePhysicalBase()
        {
            
        }

        public Point GetPosition()
        {
            return m_body.Position;
        }

        public double GetForwardAngle()
        {
            return m_body.Angle;
        }
        
        public double GetAngleVelocity()
        {
            return angleVelocity_copy;
        }
        
        public Point GetForce()
        {
            return Force_copy;
        }

        
        public double GetTorque()
        {
            return Torque_copy;
        }

        public void SetPhysicalValue(ulong actorId, double angleVelocity, double forceX, double forceY,
            double forwardAngle, double positionX, double positionY, double velocityX, double velocityY, double torque)

        {
            m_body.Position.Set(positionX,positionY);
            m_body.Velocity.Set(velocityX,velocityY);
            m_body.Force.Set(forceX,forceY);
            m_body.InitAngle(forwardAngle);
            m_body.Angle = forwardAngle;
            m_body.AngularVelocity = angleVelocity;
            m_body.Torque = torque;
        }

        #endregion





        public double GetSpeed()
        {
            return m_body.AngularVelocity;
        }
        #endregion

        #region IPhysicalInternalBase

        /// <summary>
        /// 给飞船一个与朝向相同的推力
        /// </summary>
        public void AddThrust(float proc = 0.00001f)
        {
            m_body.Force += m_body.Mass * m_body.Forward * /*engine.World.Gravity.Scaling * */  proc;
        }

        /// <summary>
        /// 给一个弧度
        /// 左右以正负来描述
        /// 默认0.05
        /// </summary>
        public void AddForward(double angular)
        {
            m_body.AngularVelocity += angular;
        }

        public void SetForwardAngle(double angle)
        {
            m_body.InitAngle(angle);
            m_body.Angle = angle;
        }

        public Body GetBody()
        {
            return m_body;
        }

        public Collider GetCollider()
        {
            return m_collider;
        }

        #endregion


        public event Action OnColliderEnter;
        public event Action OnColliderStay;
        public event Action OnColliderExit;
    }
}
