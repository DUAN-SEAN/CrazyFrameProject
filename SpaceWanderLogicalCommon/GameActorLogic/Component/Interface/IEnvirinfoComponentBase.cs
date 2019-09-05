using Box2DSharp.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

        ActorBase GetActorByBodyUserData(UserData userdate);


        /// <summary>
        /// 设置物理引擎delta
        /// </summary>
        void SetDelta(float delta);

        /// <summary>
        /// 获取物理引擎delta
        /// </summary>
        float GetDelta();

        /// <summary>
        /// 设置地图尺寸
        /// </summary>
        void SetMapSize(float height, float width);

    }

    /// <summary>
    /// 对内接口
    /// </summary>
    public interface IEnvirinfoInternalBase : IEnvirinfoBase
    {
        /// <summary>
        /// 获得工厂
        /// </summary>
        Factory GetFactory();

        void AddActor(ActorBase actor);

        void RemoveActor(ActorBase actor);

    }


}
