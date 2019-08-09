
namespace GameActorLogic
{
    /// <summary>
    /// 关卡Actor 对外接口
    /// </summary>
    public interface ILevelActorBaseContainer:
        IEventComponentBase,
        IEnvirinfoBase
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
        void Start();

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
        IEventComponentBase GetEventComponentBase();

        /// <summary>
        /// 世界环境信息
        /// </summary>
        IEnvirinfointernalBase GetEnvirinfointernalBase();

        

    }
}
