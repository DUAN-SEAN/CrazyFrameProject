

using System;

namespace GameActorLogic
{
    /// <summary>
    /// 任务事件
    /// </summary>
    public interface ITaskEvent
    {
        /// <summary>
        /// 获得任务类型定义
        /// </summary>
        Int32 GetTaskTypeDefine();

        /// <summary>
        /// 返回任务状态
        /// </summary>
        TaskEventState GetTaskState();
        /// <summary>
        /// 执行任务逻辑
        /// </summary>
        void TickTask();

        ulong GetTaskId();

        #region 同步逻辑
        /// <summary>
        /// 通过key来设置相应的值
        /// </summary>
        
        void SetValue(int key, Object value);

        void SetValue(int key, bool value);

        void SetValue(int key, Int32 value);

        #endregion


    }
}
