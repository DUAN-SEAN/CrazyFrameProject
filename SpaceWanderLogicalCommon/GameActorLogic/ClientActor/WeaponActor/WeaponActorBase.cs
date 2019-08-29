﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Base;


namespace GameActorLogic
{
    public class WeaponActorBase : ActorBase,
        IWeaponBaseContainer,
        IWeaponBaseComponentContainer
    {
        protected AIComponentBase _aiComponent;
        protected WeaponEventComponentBase _weaponEventComponent;
        protected WeaponAttributeComponentBase _weaponAttributeComponent;

        public WeaponActorBase(ulong id,Int32 type,ILevelActorComponentBaseContainer level) : base(id, type, level)
        {
            CreateBaseComponent();
        }

        protected override void CreateBaseComponent()
        {
            base.CreateBaseComponent();
            _weaponEventComponent = new WeaponEventComponentBase(this);
            _weaponAttributeComponent = new WeaponAttributeComponentBase();


        }

        public void CreateAiComponent(AIComponentBase ai)
        {
            _aiComponent = ai;
        }

        public override void Update()
        {
            base.Update();

            _aiComponent?.Update();
        }

        public override ActorBase Clone()
        {
            var clone = this.MemberwiseClone() as WeaponActorBase;
            clone._invariantAttributeComponent = new InvariantAttributeComponentBase(clone._invariantAttributeComponent);
            clone._physicalBase = new PhysicalBase(clone._physicalBase);
            clone._moveComponent = new MoveComponentBase(clone._physicalBase);

            clone._aiComponent = clone._aiComponent.Clone(clone);
            clone._weaponAttributeComponent = new WeaponAttributeComponentBase(clone._weaponAttributeComponent);
            clone._weaponEventComponent = new WeaponEventComponentBase(clone, clone._weaponEventComponent);
            return clone;
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

        AIComponentBase IAIBase.Clone(IBaseComponentContainer container)
        {
            return _aiComponent.Clone(container);
        }


        public IAIInternalBase GetAIinternalBase()
        {
            return _aiComponent;
        }
        #endregion

        #region 武器事件组件
        public void Start()
        {
            var position = GetPosition();
            level.AddEventMessagesToHandlerForward(new InitEventMessage(ActorID, GetCamp(), ActorType, position.X,
                position.Y, GetForwardAngle()));
            _weaponEventComponent.Start();
        }

        public void End()
        {
            _weaponEventComponent.End();
        }

        public void Destroy()
        {
            level.AddEventMessagesToHandlerForward(new DestroyEventMessage(ActorID));
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

        public int GetSkillCapacity()
        {
            return _weaponAttributeComponent.GetSkillCapacity();
        }

        public void SetSkillCapacity(int cap)
        {
            _weaponAttributeComponent.SetSkillCapacity(cap);
        }

        public int GetSkillCd()
        {
            return _weaponAttributeComponent.GetSkillCd();
        }

        public void SetSkillCd(int cd)
        {
            _weaponAttributeComponent.SetSkillCd(cd);
        }

        public int GetMaxSkillCd()
        {
            return _weaponAttributeComponent.GetMaxSkillCd();
        }

        public void SetMaxSkillCd(int cd)
        {
            _weaponAttributeComponent.SetMaxSkillCd(cd);
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


       

        public ISkillAttributeInternalComponent GetSkillAttributeInternalComponent()
        {
            return _weaponAttributeComponent;
        }

        public ISkillEventInternalComponent GetSkillEventInternalComponent()
        {
            return _weaponEventComponent;
        }

        #region ISkillEventComponent

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

        public event Action<IWeaponBaseContainer> OnStartSkill;
        public event Action<IWeaponBaseContainer> OnEndSkill;
        public event Action<IWeaponBaseContainer> OnDestroySkill;
        public Body GetBody()
        {
            return _physicalBase.GetBody();
        }

        #endregion

    }
}
