using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        void GetPosition();

        /// <summary>
        /// 获取朝向
        /// </summary>
        void GetForward();

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
        /// 未定
        /// </summary>
        void AddForce();

        /// <summary>
        /// 未定
        /// </summary>
        void AddForward();
    }
}
