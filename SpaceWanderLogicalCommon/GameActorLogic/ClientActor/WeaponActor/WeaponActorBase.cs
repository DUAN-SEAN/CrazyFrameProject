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

        protected WeaponActorBase(ulong id,IEnvirinfointernalBase envir) : base(id,envir)
        {

        }

        protected override void CreateComponent()
        {
            base.CreateComponent();
            _weaponEventComponent = new WeaponEventComponentBase();
            _weaponAttributeComponent = new WeaponAttributeComponentBase();
            _aiComponent = CreateAiComponent();


        }

        public abstract AIComponentBase CreateAiComponent();

        
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

        public IAIinternalBase GetAIinternalBase()
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
        #endregion

        #region 武器属性组件

        public int GetBulletNum()
        {
            return _weaponAttributeComponent.GetBulletNum();
        }

        public int GetWeaponType()
        {
            return _weaponAttributeComponent.GetWeaponType();
        }

        #endregion

        #endregion


        #region IWeaponBaseComponentContainer
        public IWeaponEventinternalBase GetWeaponEventinternalBase()
        {
            return _weaponEventComponent;
        }

        public IWeanponAttributeinternalBase GetWeaponAttributeinternalBase()
        {
            return _weaponAttributeComponent;
        }
        #endregion






    }
}
