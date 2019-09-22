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
        /// <summary>
        /// 当武器被实例化后 被组件外部开启
        /// </summary>
        void Start();

        /// <summary>
        /// 武器被组件外部关闭或结束
        /// </summary>
        void End();

        /// <summary>
        /// 武器被组件外部销毁
        /// </summary>
        void Destroy();

        /// <summary>
        /// 当武器被实例化后 被组件外部开启
        /// </summary>
        event Action<IWeaponBaseContainer> OnStartWeapon;

        /// <summary>
        /// 武器被组件外部关闭或结束
        /// </summary>
        event Action<IWeaponBaseContainer> OnEndWeapon;

        /// <summary>
        /// 武器被组件外部销毁
        /// </summary>
        event Action<IWeaponBaseContainer> OnDestroyWeapon;
    }

    /// <summary>
    /// 武器事件 对内接口
    /// </summary>
    public interface IWeaponEventinternalBase : IWeaponEventBase
    {
        /// <summary>
        /// 当武器被实例化后 被组件外部开启
        /// </summary>
        event Action OnStart;

        /// <summary>
        /// 武器被组件外部关闭或结束
        /// </summary>
        event Action OnEnd;

        /// <summary>
        /// 武器被组件外部销毁
        /// </summary>
        event Action OnDestroy;
    }
}
