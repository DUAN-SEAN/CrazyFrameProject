using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public abstract class WeaponActorBase : ActorBase,
        IWeaponBaseContainer,
        IWeaponBaseComponentContainer
    {
        protected AIComponentBase _aiComponent;
        protected WeaponEventComponentBase _weaponEventComponent;
        protected WeaponAttributeComponentBase _weaponAttributeComponent;

        protected WeaponActorBase(ulong id,ILevelActorComponentBaseContainer level) : base(id,level)
        {

        }

        protected override void CreateComponent()
        {
            base.CreateComponent();
            _weaponEventComponent = new WeaponEventComponentBase(this);
            _weaponAttributeComponent = new WeaponAttributeComponentBase();
            _aiComponent = CreateAiComponent();


        }

        public abstract AIComponentBase CreateAiComponent();

        public override void Update()
        {
            _aiComponent.Update();
        }



        #region IWeaponBaseContainer

        #region AI组件
        public bool StartAILogic()
        {
            return _aiComponent.StartAILogic();
        }

        public bool PauseAILogic()
        {
            return _aiComponent.PauseAILogic();
        }

        public IAIInternalBase GetAIinternalBase()
        {
            return _aiComponent;
        }
        #endregion

        #region 武器事件组件
        public void Start()
        {
            _weaponEventComponent.Start();
        }

        public void End()
        {
            _weaponEventComponent.End();
        }

        public void Destroy()
        {
            _weaponEventComponent.Destroy();
        }

        public event Action<IWeaponBaseContainer> OnStartWeapon
        {
            add => ((IWeaponEventBase) _weaponEventComponent).OnStartWeapon += value;
            remove => ((IWeaponEventBase) _weaponEventComponent).OnStartWeapon -= value;
        }

        public event Action<IWeaponBaseContainer> OnEndWeapon
        {
            add => ((IWeaponEventBase) _weaponEventComponent).OnEndWeapon += value;
            remove => ((IWeaponEventBase) _weaponEventComponent).OnEndWeapon -= value;
        }

        public event Action<IWeaponBaseContainer> OnDestroyWeapon
        {
            add => ((IWeaponEventBase) _weaponEventComponent).OnDestroyWeapon += value;
            remove => ((IWeaponEventBase) _weaponEventComponent).OnDestroyWeapon -= value;
        }

        #endregion

        #region 武器属性组件

        public int GetBulletNum()
        {
            return _weaponAttributeComponent.GetBulletNum();
        }

        public void SetBulletNum(int num)
        {
            _weaponAttributeComponent.SetBulletNum(num);
        }


        public int GetWeaponType()
        {
            return _weaponAttributeComponent.GetWeaponType();
        }

        public int GetWeaponCd()
        {
            return ((IWeaponAttributeBase) _weaponAttributeComponent).GetWeaponCd();
        }

        public void SetWeaponCd(int cd)
        {
            ((IWeaponAttributeBase) _weaponAttributeComponent).SetWeaponCd(cd);
        }

        #endregion

        #endregion


        #region IWeaponBaseComponentContainer
        public IWeaponEventinternalBase GetWeaponEventinternalBase()
        {
            return _weaponEventComponent;
        }

        public IWeaponAttributeInternalBase GetWeaponAttributeinternalBase()
        {
            return _weaponAttributeComponent;
        }
        #endregion






    }
}
