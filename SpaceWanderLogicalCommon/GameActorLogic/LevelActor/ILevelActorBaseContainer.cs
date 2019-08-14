
using System.Collections.Generic;

namespace GameActorLogic
{
    /// <summary>
    /// 关卡Actor 对外接口
    /// </summary>
    public interface ILevelActorBaseContainer:
        IEventComponentBase,
        ICommandComponentBase,
        IEnvirinfoBase,
        ICreateComponentBase,
        IHandlerComponentBase
    {
        /// <summary>
        /// 获得ID
        /// </summary>
        long GetLevelID();

        /// <summary>
        /// 获得战斗ID
        /// </summary>
        ulong GetBattleID();

        /// <summary>
        /// 开启关卡
        /// </summary>
        void Start(List<string> players,int barrierId);

        /// <summary>
        /// 由BattleSystem驱动
        /// </summary>
        void Update();
        
        /// <summary>
        /// 清理关卡现场
        /// </summary>
        void Dispose();

    }

    /// <summary>
    /// 关卡Actor 对内接口
    /// </summary>
    public interface ILevelActorComponentBaseContainer
    {
        /// <summary>
        /// 获取事件组件
        /// </summary>
        IEventInternalComponentBase GetEventComponentBase();

        /// <summary>
        /// 获取指令组件
        /// </summary>
        ICommandInternalComponentBase GetCommandComponentBase();

        /// <summary>
        /// 世界环境信息
        /// </summary>
        IEnvirinfoInternalBase GetEnvirinfointernalBase();

        /// <summary>
        /// 获取Actor生成组件
        /// </summary>
        ICreateInternalComponentBase GetCreateInternalComponentBase();

        /// <summary>
        /// 获取事件处理组件
        /// </summary>
        IHandlerComponentInternalBase GetHandlerComponentInternalBase();

        /// <summary>
        /// 获取任务处理组件
        /// </summary>
        ITaskEventComponentInternalBase GeTaskEventComponentInternalBase();

    }
}
