using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 移动物体组件 对外接口
    /// //TODO 所有接口参数未定
    /// </summary>
    public interface IMoveBase
    {
        /// <summary>
        /// 向左转向接口
        /// </summary>
        void Left(double proc);

        /// <summary>
        /// 向右转向接口
        /// </summary>
        void Right(double proc);

        /// <summary>
        /// 向前加速接口
        /// </summary>
        void AddThrust(float ang);
    }

    /// <summary>
    /// 移动物体组件 对内接口
    /// </summary>
    public interface IMoveInternalBase : IMoveBase
    {
        /// <summary>
        /// 当向左转向时调用
        /// </summary>
        event Action<double> OnLeft;

        /// <summary>
        /// 当向右转向时调用
        /// </summary>
        event Action<double> OnRight;

        /// <summary>
        /// 当向前加速时调用
        /// </summary>
        event Action<float> OnThrust;
    }
}
