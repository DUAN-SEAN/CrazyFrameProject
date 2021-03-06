﻿
using System;
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
        IHandlerComponentBase,
        ITaskEventComponentBase,
        IConfigComponentBase
    {
        /// <summary>
        /// 获得ID
        /// </summary>
        long GetLevelID();

        /// <summary>
        /// 获得战斗ID
        /// </summary>
        ulong GetBattleID();

        long GetCurrentFrame();
        void SetCurrentFrame(long frame);

        /// <summary>
        /// 获取PlayersId与ActorId的映射表
        /// </summary>
        Dictionary<string, ulong> GetPlayerDict();

        /// <summary>
        /// 设置PlayersId与ActorId的映射表
        /// </summary>
        void SetPlayerDict(Dictionary<string,ulong> dict);

        /// <summary>
        /// 开启关卡
        /// </summary>
        void Start(List<Tuple<string, int, int, int, int>> players);

        /// <summary>
        /// 由BattleSystem驱动
        /// </summary>
        void Update();
        
        /// <summary>
        /// 清理关卡现场
        /// </summary>
        void Dispose();

        /// <summary>
        /// 获得关卡状态
        /// </summary>
        bool GetLevelState();

        /// <summary>
        /// 返回玩家
        /// </summary>
        ActorBase GetPlayerActorByString(string id);

        /// <summary>
        /// 当关卡初始化完成
        /// </summary>
        event Action OnLoadingDone;

        /// <summary>
        /// 当开始方法完成
        /// </summary>
        event Action OnStartDone;



    }

    /// <summary>
    /// 关卡Actor 对内接口
    /// </summary>
    public interface ILevelActorComponentBaseContainer : ILevelActorBaseContainer
    {

        Dictionary<string, ulong> GetPlayers();


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

        /// <summary>
        /// 配置文件组件
        /// </summary>
        IConfigComponentInternalBase GetConfigComponentInternalBase();



    }
}
