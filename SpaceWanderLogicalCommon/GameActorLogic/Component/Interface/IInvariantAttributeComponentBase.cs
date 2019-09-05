﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public interface IInvariantAttributeBase
    {

        void InitializeInvariantAttributeBase(int camp, double maxSpeed, float maxForceProc);

        /// <summary>
        /// 获得最大速度
        /// 与物理组件中的速度对应
        /// </summary>
        double GetMaxSpeed();

        /// <summary>
        /// 最大力比率
        /// </summary>
        float GetMaxForceProc();

        /// <summary>
        /// 获得阵营
        /// </summary>
        /// <returns></returns>
        int GetCamp();

        /// <summary>
        /// 设置阵营
        /// </summary>
        void SetCamp(int camp);

        /// <summary>
        /// 设置生成数据
        /// </summary>
        void SetInitData(float x,float y ,float angle);

        /// <summary>
        /// 获取生成数据
        /// </summary>
        InitData GetInitData();
    }

    public interface IInvariantAttributeInternalBase : IInvariantAttributeBase
    {
        



    }
}