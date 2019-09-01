

using System;
using CrazyEngine.Base;
using CrazyEngine.Common;
using CrazyEngine.External;

namespace GameActorLogic
{
    public abstract class ActorBase:
        IBaseContainer,
        IBaseComponentContainer
    {
        //protected IViewInternalBase _viewinternal;
        protected PhysicalBase _physicalBase;
        protected MoveComponentBase _moveComponent;
        protected InvariantAttributeComponentBase _invariantAttributeComponent;
        protected ILevelActorComponentBaseContainer level;

        protected ulong ActorID;
        protected Int32 ActorType;
        protected string Actorname;
        protected ActorBase(ulong id,Int32 actortype,ILevelActorComponentBaseContainer level)
        {
            ActorID = id;
            //this.envir = envir;
            this.level = level;
            ActorType = actortype;
        }

        protected virtual void  CreateBaseComponent()
        {
            _physicalBase = CreatePhysicalBase();
            //目前只引发移动事件
            _moveComponent = new MoveComponentBase(_physicalBase);
            //目前只有最大速度
            _invariantAttributeComponent = new InvariantAttributeComponentBase();
        }

        public virtual void Update()
        {
            _physicalBase.Update();
        }

        public virtual void Dispose()
        {
            level = null;
            _physicalBase.Dispose();
            _physicalBase = null;
            _moveComponent.Dispose();
            _moveComponent = null;
            _invariantAttributeComponent = null;
        }

        #region 相关方法
        /// <summary>
        /// 添加碰撞函数
        /// </summary>
        protected virtual void AddColliderFunction()
        {

        }

        public virtual ActorBase Clone()
        {
            var clone = this.MemberwiseClone() as ActorBase;
            clone._invariantAttributeComponent = new InvariantAttributeComponentBase(clone._invariantAttributeComponent);
            clone._physicalBase = new PhysicalBase(clone._physicalBase);
            clone._moveComponent = new MoveComponentBase(clone._physicalBase);
            return clone;
        }

        public void SetActorId(ulong id)
        {
            ActorID = id;
        }

        public void SetActorName(string name)
        {
            Actorname = name;
        }
        #region Helper

        public void CreateBody(Body body)
        {
            _physicalBase.CreateBody(body); 
        }
        #endregion
        
        #endregion

        #region 创建组件

        /// <summary>
        /// 创建不同的物理对象
        /// </summary>
        protected PhysicalBase CreatePhysicalBase()
        {
            return new PhysicalBase(level.GetEnvirinfointernalBase());
        }

        #endregion


        #region IBaseContainer
        public ulong GetActorID()
        {
            return ActorID;
        }

        public string GetActorName()
        {
            return Actorname;
        }

        public int GetActorType()
        {
            return ActorType;
        }


        #region PhysicalBase

        public double GetRelPositionX()
        {
            return _physicalBase.GetRelPositionX();
        }

        public double GetRelPositionY()
        {
            return _physicalBase.GetRelPositionY();
        }

        public void SetRelPosition(double x, double y)
        {
            _physicalBase.SetRelPosition(x, y);
        }

        public void InitializePhysicalBase()
        {
            _physicalBase.InitializePhysicalBase();
        }

        public int GetBodyId()
        {
            return ((IPhysicalBase) _physicalBase).GetBodyId();
        }

        /// <summary>
        /// 获得位置坐标
        /// </summary>
        public Point GetPosition()
        {
            return _physicalBase.GetPosition();
        }

        public Point GetPositionPrev()
        {
            return ((IPhysicalBase) _physicalBase).GetPositionPrev();
        }

        public double GetForwardAngle()
        {
            return _physicalBase.GetForwardAngle();
        }

        public Point GetForward()
        {
            return _physicalBase.GetForward();
        }

        public Point GetVelocity()
        {
            return _physicalBase.GetVelocity();
        }



        public double GetAngleVelocity()
        {
            return _physicalBase.GetAngleVelocity();
        }

        public Point GetForce()
        {
            return _physicalBase.GetForce();
        }

        public double GetTorque()
        {
            return _physicalBase.GetTorque();
        }

        public void SetAngularVelocity(double vel)
        {
            _physicalBase.SetAngularVelocity(vel);
        }

        public void SetPhysicalValue(ulong actorId, double angleVelocity, double forceX, double forceY, double forwardAngle,
            double positionX, double positionY, double positionPrevX, double positionPrevY, double velocityX, double velocityY,
            double torque)
        {
            _physicalBase.SetPhysicalValue(actorId, angleVelocity, forceX, forceY, forwardAngle, positionX, positionY, positionPrevX, positionPrevY, velocityX, velocityY, torque);
        }

     

        /// <summary>
        /// 得到当前速度
        /// </summary>
        public double GetSpeed()
        {
            return _physicalBase.GetSpeed();
        }

        public double GetMass()
        {
            return _physicalBase.GetMass();
        }

        public void SetMass(double mass)
        {
            _physicalBase.SetMass(mass);
        }

        #endregion

        #region MoveComponent

        /// <summary>
        /// 向左转向接口
        /// </summary>
        public void Left(double proc)
        {
            _moveComponent.Left(proc);
        }

        /// <summary>
        /// 向右转向接口
        /// </summary>
        public void Right(double proc)
        {
            _moveComponent.Right(proc);
        }

        /// <summary>
        /// 向前加速接口
        /// </summary>
        public void AddThrust(float ang)
        {
            _moveComponent.AddThrust(ang);
        }

        public void Remote(float x, float y)
        {
            _moveComponent.Remote(x, y);
        }

        #endregion

        #region InvariantAttributeComponentBase

        public void InitializeInvariantAttributeBase(int camp, double maxSpeed, float maxForceProc)
        {
            _invariantAttributeComponent.InitializeInvariantAttributeBase(camp, maxSpeed, maxForceProc);
        }

        public double GetMaxSpeed()
        {
            return _invariantAttributeComponent.GetMaxSpeed();
        }
        public float GetMaxForceProc()
        {
            return _invariantAttributeComponent.GetMaxForceProc();
        }

        public int GetCamp()
        {
            return _invariantAttributeComponent.GetCamp();
        }

        public void SetCamp(int camp)
        {
            _invariantAttributeComponent.SetCamp(camp);
        }

        #endregion
        #endregion


        #region IBaseComponentContainer
        ///// <summary>
        ///// 获得对内显示组件接口
        ///// </summary>
        //IViewInternalBase IBaseComponentContainer.GetViewinternalBase()
        //{
        //    return _viewinternal;
        //}

        /// <summary>
        /// 获得对内物理组件接口
        /// </summary>
        IPhysicalInternalBase IBaseComponentContainer.GetPhysicalinternalBase()
        {
            return _physicalBase;
        }

        /// <summary>
        /// 获得对内移动组件接口
        /// </summary>
        IMoveInternalBase IBaseComponentContainer.GeMoveinternalBase()
        {
            return _moveComponent;
        }

        /// <summary>
        /// 获得对内静态属性接口
        /// </summary>
        IInvariantAttributeInternalBase IBaseComponentContainer.GeInvariantAttributeinternalBase()
        {
            return _invariantAttributeComponent;
        }

        /// <summary>
        /// 获取碰撞机
        /// 有很多碰撞事件
        /// </summary>
        IColliderInternal IBaseComponentContainer.GetColliderinternal()
        {
            return _physicalBase;
        }

     
        #endregion



    }
}
