using System.Collections;
using System.Collections.Generic;
using GameActorLogic;


namespace  GameActorLogic
{
    /// <summary>
    /// 对外接口
    /// 实现各种组件对外接口
    /// </summary>
    public interface IBaseContainer:
        IViewBase,
        IPhysicalBase,
        IMoveBase,
        IInvariantAttributeBase,
        IColliderBase
    {

    }

    /// <summary>
    /// 对内接口
    /// </summary>
    public interface IBaseComponentContainer : IBaseContainer
    {
        /// <summary>
        /// 获得对内显示组件接口
        /// </summary>
        IViewinternalBase GetViewinternalBase();

        /// <summary>
        /// 获得对内物理组件接口
        /// </summary>
        IPhysicalinternalBase GetPhysicalinternalBase();

        /// <summary>
        /// 获得对内移动组件接口
        /// </summary>
        IMoveinternalBase GeMoveinternalBase();

        /// <summary>
        /// 获得对内静态属性接口
        /// </summary>
        IInvariantAttributeinternalBase GeInvariantAttributeinternalBase();

        /// <summary>
        /// 获取碰撞机
        /// 有很多碰撞事件
        /// </summary>
        IColliderinternal GetIColliderinternal();
    }

  


}
