using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 船事件 对外接口
    /// </summary>
    public interface IShipEventBase
    {
        /// <summary>
        /// 动态初始化生成调用
        /// </summary>
        void Init();

        /// <summary>
        /// 动态销毁调用
        /// </summary>
        void Destroy();

    }


    /// <summary>
    /// 船事件对内接口
    /// </summary>
    public interface IShipEventinternalBase : IShipEventBase
    {
        event Action OnInit;

        event Action OnDestroy;
    }
}
