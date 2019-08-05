using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 武器事件 对外接口
    /// </summary>
    public interface IWeaponEventBase
    {
        void Start();

        void End();

        void Destroy();
    }

    /// <summary>
    /// 武器事件 对内接口
    /// </summary>
    public interface IWeaponEventinternalBase : IWeaponEventBase
    {
        /// <summary>
        /// 当武器被实例化后 被外部开启
        /// </summary>
        event Action OnStart;

        /// <summary>
        /// 武器被外部关闭或结束
        /// </summary>
        event Action OnEnd;

        /// <summary>
        /// 武器被外部销毁
        /// </summary>
        event Action OnDestroy;
    }
}
