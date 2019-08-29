

using System;
using System.Collections.Generic;

namespace GameActorLogic
{
    /// <summary>
    /// 任务事件
    /// </summary>
    public interface ITaskEvent
    {
        /// <summary>
        /// 获得任务条件类型定义
        /// 击杀数，时间
        /// </summary>
        Int32 GetTaskConditionTypeDefine();

        /// <summary>
        /// 获得任务结果类型
        /// 胜利，失败等
        /// </summary>
        Int32 GetTaskResultTypeDefine();

        /// <summary>
        /// 返回任务状态
        /// </summary>
        TaskEventState GetTaskState();


        /// <summary>
        /// 获取当前条件值
        /// </summary>
        Dictionary<int, int> ConditionCurrentValues { get; set; }

        /// <summary>
        /// 激活任务
        /// 从闲置激活到未完成状态
        /// 完成状态不能激活
        /// </summary>
        bool ActivateTask();

        /// <summary>
        /// 执行任务逻辑
        /// </summary>
        void TickTask();

        /// <summary>
        /// 获得任务ID
        /// </summary>
        /// <returns></returns>
        int GetTaskId();

        /// <summary>
        /// 向任务数据字典中添加值
        /// </summary>
        bool AddValue(int key,int value);

        /// <summary>
        /// 尝试从任务数据字典中获取值
        /// </summary>
        bool TryGetValue(int key, out int value);

        void Dispose();

        #region 同步逻辑
        /// <summary>
        /// 通过key来设置相应的值
        /// </summary>
        
        void SetValue(int key, Object value);

        void SetValue(int key, bool value);

        void SetValue(int key, Int32 value);


        void SetTaskState(TaskEventState state);

        #endregion


    }
}
