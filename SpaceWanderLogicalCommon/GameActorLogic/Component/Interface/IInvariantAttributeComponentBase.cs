﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public interface IInvariantAttributeBase
    {
        /// <summary>
        /// 获得最大速度
        /// 与物理组件中的速度对应
        /// </summary>
        double GetMaxSpeed();
    }

    public interface IInvariantAttributeinternalBase : IInvariantAttributeBase
    {
        



    }
}