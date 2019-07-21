using CrazyEngine;
using SpaceShip.System;
using System.Collections.Generic;
using SpaceShip.AI;
using SpaceShip.Factory;
namespace SpaceShip.Base
{
    /// <summary>
    /// 将系统对象化 用于生成和结束世界系统对象等
    /// </summary>
    public interface ISBZhuying
    {
       
        
        /// <summary>
        /// 获得该对象中的武器驱动列表
        /// </summary>
        List<ITickable> GetWeanponList();

        IGetWorld GetWorld();

        /// <summary>
        /// 获取该对象的AI接口集合
        /// </summary>
        #region AI系统接口
        IAILog GetAILog();
        #endregion

        Queue<IBodyMessage> GetBodyMessages();
        

        

    }
}
