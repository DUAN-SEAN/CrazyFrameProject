﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Base;
using CrazyEngine.Core;
using CrazyEngine.External;

namespace GameActorLogic
{
    /// <summary>
    /// actor环境组件
    /// 包括物理引擎相关操作
    /// 添加了Actor对象集合
    /// 对外接口
    /// </summary>
    public interface IEnvirinfoBase
    {

        List<ActorBase> GetAllActors();

        /// <summary>
        /// 获取一个Actor
        /// </summary>
        ActorBase GetActor(ulong id);
    }

    /// <summary>
    /// 对内接口
    /// </summary>
    public interface IEnvirinfoInternalBase : IEnvirinfoBase
    {
        /// <summary>
        /// 获取物理引擎
        /// </summary>
        Engine GetEngine();

        /// <summary>
        /// 获取碰撞信息收集器
        /// </summary>
        CollisionEvent GetCollisionEvent();


        void AddActor(ActorBase actor);

        void RemoveActor(ActorBase actor);

    }


}
