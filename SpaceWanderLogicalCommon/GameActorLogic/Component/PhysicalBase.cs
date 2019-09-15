using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Box2DSharp.External;
using Crazy.Common;

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
        protected IEnvirinfoBase envirinfo;

        //copy值以防物理清除值
        protected Vector2 Force_copy = new Vector2();
        //之前那个被拿去计算合力 现在这个再次copy
        protected Vector2 Force_copy_copy = new Vector2();

        protected float Torque_copy;
        protected float angleVelocity_copy;
        protected Vector2 LinearVelocity_copy;
        protected float Damping_copy;



        /// <summary>
        /// 碰撞方法被
        /// </summary>
        protected bool isColliderStay;
        /// <summary>
        /// 判断这一帧碰撞方法是否被调用
        /// </summary>
        protected bool isColliderMethodEnter;

        /// <summary>
        /// 间隔时间内是否有碰撞进入标志
        /// </summary>
        protected bool isContactEnterFlag;

        /// <summary>
        /// 间隔时间内是否有碰撞退出标志
        /// </summary>
        protected bool isContactExitFlag;

        /// <summary>
        /// 相对坐标
        /// 只在武器发射时使用
        /// 武器移动时不使用
        /// </summary>
        protected float RelpositionX;
        protected float RelpositionY;

        /// <summary>
        /// 上一次Tick标志的帧
        /// </summary>
        protected long lastFlagframe;
        /// <summary>
        /// 标志tick间隔
        /// </summary>
        protected long Flagdely = 100000;


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
            contactActors = new List<UserData>();


        }

        public PhysicalBase(PhysicalBase clone)
        {
            this.envirinfo = clone.envirinfo;
            // 朱颖 clone body
            //this.m_body = Factory.CreateCloneBody(clone.m_body);

            contactActors = new List<UserData>();

            //Log.Trace("物理引擎复制角度值："+m_body.Angle);

        }

        /// <summary>
        /// 碰撞逻辑检测
        /// 调用相应的碰撞方法
        /// </summary>
        public void OnCollision(UserData collision)
        {
            isColliderMethodEnter = true;
          
            if(isColliderStay == false)
            {
                OnColliderEnter?.Invoke(collision);
                isColliderStay = true;
            }
            if (isColliderStay)
                OnColliderStay?.Invoke(collision);
        }
        protected List<UserData> contactActors;

        public void OnContactEnter(UserData data)
        {
            
            OnColliderEnter?.Invoke(data);
            contactActors.Add(data);
            isContactEnterFlag = true;
        }

        public void OnContactExit(UserData data)
        {
            OnColliderExit?.Invoke(data);
            contactActors.Remove(data);
            isContactExitFlag = true;
        }


        public void Update()
        {
           
            //老版 物理碰撞
            if (isColliderStay == true && isColliderMethodEnter == false)
            {
                isColliderStay = false;
                OnColliderExit?.Invoke(null);
            }

            //新版物理碰撞
            if(contactActors.Count > 0)
            {
                foreach(var i in contactActors)
                    //持续伤害
                    OnColliderStay?.Invoke(i);
            }
            //附上值
            angleVelocity_copy = m_body.AngularVelocity;
            LinearVelocity_copy = m_body.LinearVelocity;
            Torque_copy = m_body.GetTorque();
            Damping_copy = m_body.LinearDamping;
            isColliderMethodEnter = false;
            //Force_copy = m_body.GetForce();
            //if(Force_copy.LengthSquared() > 1000000)
            //{
            //    Force_copy.Normalize();
            //    Force_copy = Force_copy * 31.62277660168379f;
            //}
           
            Force_copy_copy = Force_copy;
            
            m_body.AddForce(Force_copy);


            //Log.Trace("已经添加力量:" + m_body.GetForce()+" "+Force_copy);
            //Log.Trace("Update Mass" + m_body.Mass);
            Force_copy = Vector2.Zero;

        TickFlag();
        }

        public void Dispose()
        {
            envirinfo = null;

            //m_body?.Dispose();
            m_body = null;
        }
      

        public void TickFlag()
        {
            if(DateTime.Now.Ticks - lastFlagframe > Flagdely)
            {
                isContactEnterFlag = false;
                isContactExitFlag = false;
                lastFlagframe = DateTime.Now.Ticks;
            }

        }

        #region Helper

        public void CreateBody(Body body)
        {
            m_body = body;
          
        }

        #endregion


        #region IPhysicalBase

        #region 物理同步

        public Vector2 GetForward()
        {
            return m_body.GetForward();
        }

        public Vector2 GetVelocity()
        {
            return m_body.LinearVelocity;
        }

       
        public float GetRelPositionX()
        {
            return RelpositionX;
        }

        public float GetRelPositionY()
        {
            return RelpositionY;
        }

        public void SetRelPosition(float x, float y)
        {
            RelpositionX = x;
            RelpositionY = y;
        }

        public void InitializePhysicalBase()
        {
            
        }

        public UserData GetBodyUserData()
        {
            return m_body.UserData as UserData;
        }

        public Vector2 GetPosition()
        {
            return m_body.GetPosition();
        }


        public float GetForwardAngle()
        {
            return m_body.GetTransform().Rotation.Angle;
        }
        
        public float GetAngleVelocity()
        {
            return angleVelocity_copy;
        }
        
        public Vector2 GetForce()
        {
            return Force_copy_copy;
        }

        
        public float GetTorque()
        {
            return Torque_copy;
        }

        public void SetAngularVelocity(float vel)
        {
            m_body.SetAngularVelocity(vel);
        }

        public void SetPhysicalValue(ulong actorId, float angleVelocity, float forceX, float forceY,
            float angle, float positionX, float positionY, float positionPrevX, float positionPrevY, float velocityX, float velocityY, float torque)

        {
            Force_copy_copy = new Vector2(forceX, forceY);
            Torque_copy = torque;
            LinearVelocity_copy = new Vector2(velocityX, velocityY);
            angleVelocity_copy = angleVelocity;
            if (m_body == null) return;
            m_body.SetTransform(new Vector2(positionX, positionY), angle);
            m_body.SetLinearVelocity(new Vector2(velocityX, velocityY));
            m_body.AddForce(new Vector2(forceX, forceY));
            m_body.SetAngularVelocity(angleVelocity);
            m_body.AddTorque(torque);
        }

        #endregion





    

       
        #endregion

        #region IPhysicalInternalBase

        /// <summary>
        /// 给飞船一个与朝向相同的推力
        /// </summary>
        public void AddThrust(float pro)
        {
            Force_copy += m_body.GetForward() * pro;
            //Log.Trace("AddThrust:"+pro + " Forward"+m_body.GetForward());
            //m_body.AddForce(m_body.GetForward() * pro);
        }

        /// <summary>
        /// 给一个弧度
        /// 左右以正负来描述
        /// 默认0.05
        /// </summary>
        public void AddForward(float angular)
        {
            m_body.AddTorque(angular);
        }

       

        public Body GetBody()
        {
            return m_body;
        }

        public bool GetContactEnterFlag()
        {
            return isContactEnterFlag;
        }

        public bool GetContactExitFlag()
        {
            return isContactExitFlag;
        }


        public float GetLinerDamping()
        {
            return Damping_copy;
        }

        public void SetLinerDamping(float damp)
        {
            Damping_copy = damp;
            if(m_body != null)
                m_body.LinearDamping = damp;
     
        }




        #endregion


        public event Action<UserData> OnColliderEnter;
        public event Action<UserData> OnColliderStay;
        public event Action<UserData> OnColliderExit;
    }
}
