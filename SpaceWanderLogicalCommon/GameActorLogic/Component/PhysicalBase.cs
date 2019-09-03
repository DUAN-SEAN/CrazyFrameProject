using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
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

        /// <summary>
        /// 相对坐标
        /// 只在武器发射时使用
        /// 武器移动时不使用
        /// </summary>
        protected double RelpositionX;
        protected double RelpositionY;
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

        public PhysicalBase(PhysicalBase clone)
        {
            this.envirinfo = clone.envirinfo;
            this.m_collider = new Collider();
            // 朱颖 clone body
            this.m_body = Factory.CreateCloneBody(clone.m_body);
            //Log.Trace("物理引擎复制角度值："+m_body.Angle);
            m_collider.OnCollisionStay += OnCollision;

        }

        /// <summary>
        /// 碰撞逻辑检测
        /// 调用相应的碰撞方法
        /// </summary>
        protected void OnCollision(Collision collision)
        {
            isColliderMethodEnter = true;
            var body = m_body.Id != collision.BodyA.Id ? collision.BodyA : collision.BodyB;
            if(isColliderStay == false)
            {
                OnColliderEnter?.Invoke(body);
                isColliderStay = true;
            }
            if (isColliderStay)
                OnColliderStay?.Invoke(body);
        }

        public void Update()
        {
           
            if (isColliderStay == true && isColliderMethodEnter == false)
            {
                isColliderStay = false;
                OnColliderExit?.Invoke();
            }

            //附上值
            Force_copy.X = m_body.Force.X;
            Force_copy.Y = m_body.Force.Y;
            angleVelocity_copy = m_body.AngularVelocity;
            Torque_copy = m_body.Torque;
            isColliderMethodEnter = false;
        }

        public void Dispose()
        {
            envirinfo = null;

            m_body.Dispose();
            m_body = null;
           
            m_collider.OnCollisionStay -= OnCollision;
            m_collider = null;
            
            Force_copy = null;
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

       
        public double GetRelPositionX()
        {
            return RelpositionX;
        }

        public double GetRelPositionY()
        {
            return RelpositionY;
        }

        public void SetRelPosition(double x, double y)
        {
            RelpositionX = x;
            RelpositionY = y;
        }

        public void InitializePhysicalBase()
        {
            
        }

        public int GetBodyId()
        {
            return m_body.Id.Value;
        }

        public Point GetPosition()
        {
            return m_body.Position;
        }

        public Point GetPositionPrev()
        {
            return m_body.PositionPrev;
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

        public void SetAngularVelocity(double vel)
        {
            m_body.AngularVelocity = vel;
        }

        public void SetPhysicalValue(ulong actorId, double angleVelocity, double forceX, double forceY,
            double forwardAngle, double positionX, double positionY, double positionPrevX, double positionPrevY, double velocityX, double velocityY, double torque)

        {
            m_body.Position.Set(positionX,positionY);
            m_body.PositionPrev.Set(positionPrevX, positionPrevY);
            m_body.Velocity.Set(velocityX,velocityY);
            m_body.Force.Set(forceX,forceY);
            m_body.SetForward(forwardAngle);
            m_body.Angle = forwardAngle;
            m_body.AngularVelocity = angleVelocity;
            m_body.Torque = torque;
        }

        #endregion





        public double GetSpeed()
        {
            return m_body.AngularVelocity;
        }

        public double GetMass()
        {
            return m_body.Mass;
        }

        public void SetMass(double mass)
        {
            m_body.Mass = mass;
        }

        #endregion

        #region IPhysicalInternalBase

        /// <summary>
        /// 给飞船一个与朝向相同的推力
        /// </summary>
        public void AddThrust(float proc = 10000000f)
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
            m_body.AngularVelocity = angular;
        }

        public void SetForwardAngle(double angle)
        {
            m_body.SetForward(angle);
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


        public event Action<Body> OnColliderEnter;
        public event Action<Body> OnColliderStay;
        public event Action OnColliderExit;
    }
}
