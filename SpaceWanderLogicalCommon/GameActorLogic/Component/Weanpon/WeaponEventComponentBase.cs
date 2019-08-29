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
        IWeaponEventinternalBase,
        ISkillEventComponent,
        ISkillEventInternalComponent
    {
        protected IWeaponBaseComponentContainer weapon;

        public WeaponEventComponentBase(IWeaponBaseComponentContainer weapon)
        {
            this.weapon = weapon;
        }
        public WeaponEventComponentBase(IWeaponBaseComponentContainer weapon, WeaponEventComponentBase clone)
        {
            
            this.weapon = weapon;
            OnStart = clone.OnStart;
            OnEnd = clone.OnEnd;
            OnDestroy = clone.OnDestroy;
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


        public void StartSkill()
        {
            Start();
        }

        public void EndSkill()
        {
            End();
        }

        public void DestroySkill()
        {
            Destroy();
        }

        public event Action<IWeaponBaseContainer> OnStartSkill
        {
            add => OnStartWeapon += value;
            remove => OnStartWeapon -= value;
        }
        public event Action<IWeaponBaseContainer> OnEndSkill
        {
            add => OnEndWeapon += value;
            remove => OnEndWeapon -= value;
        }
        public event Action<IWeaponBaseContainer> OnDestroySkill
        {
            add => OnDestroyWeapon += value;
            remove => OnDestroyWeapon -= value;
        }




        public event Action OnStartSkillInternal
        {
            add => OnStart += value;
            remove => OnStart -= value;
        }
        public event Action OnEndSkillInternal
        {
            add => OnEnd += value;
            remove => OnEnd -= value;
        }
        public event Action OnDestroySkillInternal
        {
            add => OnDestroy += value;
            remove => OnDestroy -= value;
        }
    }
}
