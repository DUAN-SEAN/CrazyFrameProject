﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 任务条件接口
    /// 用于将任务和任务条件分离
    /// </summary>
    public interface ITaskCondition
    {

        /// <summary>
        /// 开启任务条件
        /// </summary>
        void StartCondition();

        /// <summary>
        /// 检查条件是否达成
        /// </summary>
        bool TickCondition();

        void Dispose();

        /// <summary>
        /// 获取当前条件值
        /// </summary>
        Dictionary<int, int> ConditionCurrentValues { get; set; }



        /// <summary>
        /// 获取当前值
        /// </summary>
        int GetCurrentValue();

        /// <summary>
        /// 获取目标值
        /// </summary>
        int GetTargetValue();
    }
}
