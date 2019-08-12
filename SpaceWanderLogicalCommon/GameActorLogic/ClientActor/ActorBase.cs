

using CrazyEngine.Common;

namespace GameActorLogic
{
    public abstract class ActorBase:
        IBaseContainer,
        IBaseComponentContainer
    {
        protected IViewinternalBase _viewinternal;
        protected PhysicalBase _physicalBase;
        protected MoveComponentBase _moveComponent;
        protected InvariantAttributeComponentBase _invariantAttributeComponent;
        protected ColliderComponentBase _colliderComponent;

        

        protected virtual void  CreateComponent()
        {
            //显示组件 只做demo

            //TODO 物理引擎与碰撞的关系还未完善不能初始化

            //目前只引发移动事件
            _moveComponent = new MoveComponentBase(_physicalBase);

            //目前只有最大速度
            _invariantAttributeComponent = new InvariantAttributeComponentBase();

        }


        #region IBaseContainer

        #region PhysicalBase

        /// <summary>
        /// 获得位置坐标
        /// </summary>
        public Point GetPosition()
        {
            return _physicalBase.GetPosition();
        }

        /// <summary>
        /// 获取朝向
        /// </summary>
        public Point GetForward()
        {
            return _physicalBase.GetForward();
        }

        /// <summary>
        /// 得到当前速度
        /// </summary>
        public double GetSpeed()
        {
            return _physicalBase.GetSpeed();
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
        #endregion

        #region InvariantAttributeComponentBase
        public double GetMaxSpeed()
        {
            return _invariantAttributeComponent.GetMaxSpeed();
        }


        #endregion
        #endregion


        #region IBaseComponentContainer
        /// <summary>
        /// 获得对内显示组件接口
        /// </summary>
        IViewinternalBase IBaseComponentContainer.GetViewinternalBase()
        {
            return _viewinternal;
        }

        /// <summary>
        /// 获得对内物理组件接口
        /// </summary>
        IPhysicalinternalBase IBaseComponentContainer.GetPhysicalinternalBase()
        {
            return _physicalBase;
        }

        /// <summary>
        /// 获得对内移动组件接口
        /// </summary>
        IMoveinternalBase IBaseComponentContainer.GeMoveinternalBase()
        {
            return _moveComponent;
        }

        /// <summary>
        /// 获得对内静态属性接口
        /// </summary>
        IInvariantAttributeinternalBase IBaseComponentContainer.GeInvariantAttributeinternalBase()
        {
            return _invariantAttributeComponent;
        }

        /// <summary>
        /// 获取碰撞机
        /// 有很多碰撞事件
        /// </summary>
        IColliderinternal IBaseComponentContainer.GetColliderinternal()
        {
            return _colliderComponent;
        }
        #endregion


       
    }
}
