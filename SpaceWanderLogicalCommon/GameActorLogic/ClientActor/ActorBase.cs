

using System;
using System.Numerics;
using Box2DSharp.Dynamics;

/*
                           _ooOoo_
                          o8888888o
                          88" . "88
                          (| -_- |)
                          O\  =  /O
                       ____/`---'\____
                     .'  \\|     |//  `.
                    /  \\|||  :  |||//  \
                   /  _||||| -:- |||||-  \
                   |   | \\\  -  /// |   |
                   | \_|  ''\---/''  |   |
                   \  .-\__  `-`  ___/-. /
                 ___`. .'  /--.--\  `. . __
              ."" '<  `.___\_<|>_/___.'  >'"".
             | | :  `- \`.;`\ _ /`;.`/ - ` : | |
             \  \ `-.   \_ __\ /__ _/   .-` /  /
        ======`-.____`-.___\_____/___.-`____.-'======
                           `=---='
        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                 佛祖保佑       永无BUG
*/

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

        public void CreateInitData(InitData data)
        {
            _invariantAttributeComponent.CreateInitData(data);
        }

        public void CreateBody(Body body)
        {
            //body.UserData = new UserData(ActorID, ActorType);
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

        public void OnCollision(UserData data)
        {
            _physicalBase.OnCollision(data);
        }


        public void OnContactEnter(UserData data)
        {
            _physicalBase.OnContactEnter(data);

        }

        public void OnContactExit(UserData data)
        {
            _physicalBase.OnContactExit(data);
        }

        public bool GetContactEnterFlag()
        {
            return _physicalBase.GetContactEnterFlag();
        }

        public bool GetContactExitFlag()
        {
            return _physicalBase.GetContactExitFlag();
        }

        public float GetRelPositionX()
        {
            return _physicalBase.GetRelPositionX();
        }

        public float GetRelPositionY()
        {
            return _physicalBase.GetRelPositionY();
        }

        public void SetRelPosition(float x, float y)
        {
            _physicalBase.SetRelPosition(x, y);
        }

        public void InitializePhysicalBase()
        {
            _physicalBase.InitializePhysicalBase();
        }

        public UserData GetBodyUserData()
        {
            return ((IPhysicalBase) _physicalBase).GetBodyUserData();
        }

        /// <summary>
        /// 获得位置坐标
        /// </summary>
        public Vector2 GetPosition()
        {
            return _physicalBase.GetPosition();
        }

      
        public float GetForwardAngle()
        {
            return _physicalBase.GetForwardAngle();
        }

        public Vector2 GetForward()
        {
            return _physicalBase.GetForward();
        }

        public Vector2 GetVelocity()
        {
            return _physicalBase.GetVelocity();
        }

        public float GetLinerDamping()
        {
            return _physicalBase.GetLinerDamping();
        }

        public void SetLinerDamping(float damp)
        {
            _physicalBase.SetLinerDamping(damp);
        }

        public float GetAngleVelocity()
        {
            return _physicalBase.GetAngleVelocity();
        }

        public Vector2 GetForce()
        {
            return _physicalBase.GetForce();
        }

        public float GetTorque()
        {
            return _physicalBase.GetTorque();
        }

        public void SetAngularVelocity(float vel)
        {
            _physicalBase.SetAngularVelocity(vel);
        }

        public void SetPhysicalValue(ulong actorId, float angleVelocity, float forceX, float forceY, float forwardAngle,
            float positionX, float positionY, float positionPrevX, float positionPrevY, float velocityX, float velocityY,
            float torque)
        {
            _physicalBase.SetPhysicalValue(actorId, angleVelocity, forceX, forceY, forwardAngle, positionX, positionY, positionPrevX, positionPrevY, velocityX, velocityY, torque);
        }

     

        /// <summary>
        /// 得到当前速度
        /// </summary>

     



        #endregion

        #region MoveComponent

        /// <summary>
        /// 向左转向接口
        /// </summary>
        public void Left(float proc)
        {
            _moveComponent.Left(proc);
        }

        /// <summary>
        /// 向右转向接口
        /// </summary>
        public void Right(float proc)
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

        public float GetActorInitPro()
        {
            return _invariantAttributeComponent.GetActorInitPro();
        }

        public void SetActorInitPro(float pro)
        {
            _invariantAttributeComponent.SetActorInitPro(pro);
        }


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
        public void SetInitData(float x, float y, float angle)
        {
            _invariantAttributeComponent.SetInitData(x, y, angle);
        }

        public InitData GetInitData()
        {
            return _invariantAttributeComponent.GetInitData();
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

        public bool GetDeadState()
        {
            return _invariantAttributeComponent.GetDeadState();
        }

        public void SetDeadState(bool dead)
        {
            _invariantAttributeComponent.SetDeadState(dead);
        }

        public ILevelActorComponentBaseContainer GetLevel()
        {
            return level;
        }









        #endregion



    }
}
