using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Common;

namespace GameActorLogic { 
    /// <summary>
    /// 针对物理引擎对象的操作 对外接口
    /// //TODO 物理引擎会根据最新的物理引擎更新接口
    /// </summary>
    public interface IPhysicalBase
    {
        /// <summary>
        /// 获得位置坐标
        /// </summary>
        Point GetPosition();

        /// <summary>
        /// 获取朝向
        /// </summary>
        Point GetForward();

        /// <summary>
        /// 得到当前速度
        /// </summary>
        double GetSpeed();
    }


    /// <summary>
    /// 针对物理引擎对象的操作 对内接口
    /// </summary>
    public interface IPhysicalinternalBase : IPhysicalBase
    {
        /// <summary>
        /// 给飞船一个与朝向相同的推力
        /// </summary>
        void AddThrust(float pro);

        /// <summary>
        /// 给一个弧度表示转向弧度
        /// 正负表示方向
        /// 默认0.001
        /// </summary>
        void AddForward(double ang);
    }
}
