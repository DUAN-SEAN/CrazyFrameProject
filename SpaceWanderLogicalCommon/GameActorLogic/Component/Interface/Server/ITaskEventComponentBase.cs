﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 任务事件
    /// 会引发一系列操作的事件
    /// 任务会引发任务
    /// 任务引发的条件很多
    /// </summary>
    public interface ITaskEventComponentBase
    {
        /// <summary>
        /// 添加一个任务
        /// </summary>
        void AddTaskEvent(ITaskEvent task);

        /// <summary>
        /// 获取已添加的所有任务
        /// </summary>
        List<ITaskEvent> GetAllTaskEvents();

        /// <summary>
        /// 获得所有正在Tick且未完成的任务
        /// </summary>
        List<ITaskEvent> GetUnFinishTaskEvents();

    }

    public interface ITaskEventComponentInternalBase : ITaskEventComponentBase
    {

    } 
}
