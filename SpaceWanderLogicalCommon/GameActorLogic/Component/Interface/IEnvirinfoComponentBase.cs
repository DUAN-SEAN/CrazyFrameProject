using System;
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
        /// <summary>
        /// 获取所有Actor
        /// </summary>
        List<ActorBase> GetAllActors();

        /// <summary>
        /// 获取所有玩家
        /// </summary>
        List<ActorBase> GetPlayerActors();

        /// <summary>
        /// 获取所有船Actor
        /// </summary>
        List<ActorBase> GetShipActors();

        /// <summary>
        /// 获取所有武器Actor
        /// </summary>
        List<ActorBase> GetWeaponActors();
        /// <summary>
        /// 获取一个Actor
        /// </summary>
        ActorBase GetActor(ulong id);

        ActorBase GetActorByBodyId(int bodyid);

        Engine GetEngine();

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
