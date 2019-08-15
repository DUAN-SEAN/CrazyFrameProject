using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    /// <summary>
    /// 武器事件组件基础实现
    /// </summary>
    public class WeaponEventComponentBase : 
        IWeaponEventBase,
        IWeaponEventinternalBase
    {
        protected IWeaponBaseComponentContainer weapon;

        public WeaponEventComponentBase(IWeaponBaseComponentContainer weapon)
        {
            this.weapon = weapon;
        }

        #region IWeaponEventBaes
        public void Start()
        {
            OnStartWeapon?.Invoke(weapon);
            OnStart?.Invoke();
        }

        public void End()
        {
            OnEndWeapon?.Invoke(weapon);
            OnEnd?.Invoke();
        }

        public void Destroy()
        {
            OnDestroyWeapon?.Invoke(weapon);
            OnDestroy?.Invoke();
        }

        public event Action<IWeaponBaseContainer> OnStartWeapon;
        public event Action<IWeaponBaseContainer> OnEndWeapon;
        public event Action<IWeaponBaseContainer> OnDestroyWeapon;

        #endregion

        #region IWeaponEventinternalBase

        public event Action OnStart;
        public event Action OnEnd;
        public event Action OnDestroy;

        #endregion



    }
}
