﻿
/**
  *_________________#####_________________________ 
  *_______________#########_______________________ 
  *______________############_____________________ 
  *_____________##############____________________ 
  *_____________##__###########___________________ 
  *____________###__######_#####__________________ 
  *____________###_#######___####_________________ 
  *___________###__##########_####________________ 
  *__________####__###########_####_______________ 
  *________#####___###########__#####_____________ 
  *_______######___###_########___#####___________ 
  *_______#####___###___########___######_________ 
  *______######___###__###########___######_______ 
  *_____######___####_##############__######______ 
  *____#######__#####################_#######_____ 
  *____#######__######################_#######____ 
  *___#######__######_################__#######___ 
  *___#######__######_####_____####______######___ 
  *___#######____##___###_______###_______######___ 
  *___#######_________###_______###_______#####____ 
  *____######_________###_______###_______####_____ 
  *_____#####_________###_______###_______###______ 
  *______#####_______###________###______#________ 
  *________##________####________####______________ 
  */

using System;

namespace GameActorLogic
{
    /// <summary>
    /// 对外接口
    /// 实现各种组件对外接口
    /// </summary>
    public interface IBaseContainer:
        //IViewBase,
        IPhysicalBase,
        IMoveBase,
        IInvariantAttributeBase,
        IColliderBase
    {
        ulong GetActorID();

        string GetActorName();

        /// <summary>
        /// 返回具体类型的相对应常量
        /// </summary>
        Int32 GetActorType();

        /// <summary>
        /// 获得该对象的深拷贝
        /// </summary>
        ActorBase Clone();

        void Dispose();
    }

    /// <summary>
    /// 对内接口
    /// </summary>
    public interface IBaseComponentContainer : IBaseContainer
    {
        ///// <summary>
        ///// 获得对内显示组件接口
        ///// </summary>
        //IViewInternalBase GetViewinternalBase();

        /// <summary>
        /// 获得对内物理组件接口
        /// </summary>
        IPhysicalInternalBase GetPhysicalinternalBase();

        /// <summary>
        /// 获得对内移动组件接口
        /// </summary>
        IMoveInternalBase GeMoveinternalBase();

        /// <summary>
        /// 获得对内静态属性接口
        /// </summary>
        IInvariantAttributeInternalBase GeInvariantAttributeinternalBase();

        /// <summary>
        /// 获取碰撞机
        /// 有很多碰撞事件
        /// </summary>
        IColliderInternal GetColliderinternal();

        ILevelActorComponentBaseContainer GetLevel();

    }



 

}
