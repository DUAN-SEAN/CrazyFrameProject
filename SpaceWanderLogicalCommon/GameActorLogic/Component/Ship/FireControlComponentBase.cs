﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 基础火控组件
    /// 每个飞船的火控组件中的武器应该不同
    /// </summary>
    public class FireControlComponentBase:
        IFireControlBase,
        IFireControlInternalBase
    {
        /// <summary>
        /// 准备好生成的武器
        /// </summary>
        protected List<ISkillContainer> weapons;

        protected List<ISkillContainer> weaponInitList;

        protected ILevelActorComponentBaseContainer level;

        protected IShipComponentBaseContainer container;

        protected int buttonstate = 0;

        public FireControlComponentBase(IShipComponentBaseContainer container, ILevelActorComponentBaseContainer create)
        {
            weapons = new List<ISkillContainer>();
            this.container = container;
            this.level = create;
        }

        public FireControlComponentBase(IShipComponentBaseContainer container, ILevelActorComponentBaseContainer create,List<Int32> weapons)
        {
            this.container = container;
            this.weapons = new List<ISkillContainer>();
            this.level = create;

            foreach (var weapon in weapons)
            {
               level.GetConfigComponentInternalBase().GetActorClone(weapon, out var actor);
               if (actor is ISkillContainer weaponBase)
                   this.weapons.Add(weaponBase);
            }
            
        }

        protected void WaitWeaponDestroy(ISkillContainer weapon)
        {
            weaponInitList.Remove(weapon);
        }

        #region IFireControlBase

        public void InitializeFireControl(List<Int32> containers)
        {
            weapons = new List<ISkillContainer>();
            foreach (var weapon in containers)
            {
                level.GetConfigComponentInternalBase().GetActorClone(weapon, out var actor);
                if (actor is IWeaponBaseContainer weaponBase)
                    this.weapons.Add(weaponBase);
            }
        }

        public void SendButtonState(ulong actorid, int skilltype, int skillcontrol)
        {
            //TODO 
            switch (skillcontrol)
            {
                
            }
        }

        //protected void TickFire()
        //{
        //    switch (skilltype)
        //    {
        //        //发射型武器
        //        case ActorTypeBaseDefine.AntiAircraftGunActor:
        //        case ActorTypeBaseDefine.MachineGunActor:
        //        case ActorTypeBaseDefine.PowerLaserActor:
        //        case ActorTypeBaseDefine.TorpedoActor:
        //        case ActorTypeBaseDefine.TrackingMissileActor:
        //            break;
        //        case ActorTypeBaseDefine.ContinuousLaserActor:
        //        case ActorTypeBaseDefine.TimeBombActor:
        //        case ActorTypeBaseDefine.TriggerBombActor:
        //            break;


        //    }
        //}
        
        

        public void Fire(int i)
        {
            for (var j = 0; j < weapons.Count; j++)
            {
                if (weapons[j].GetActorType() == i && weapons[j] is IWeaponBaseContainer actor)
                {
                    var weapon = actor.Clone() as IWeaponBaseContainer;
                    weaponInitList.Add(weapon);
                    weapon.OnDestroyWeapon += WaitWeaponDestroy;
                    level.GetEnvirinfointernalBase().AddActor(weapon as ActorBase);
                    weapon.Start();
                    OnFire?.Invoke(weapon);
                }
            }
        }

        public void End(int i)
        {

            for (var j = 0; j < weaponInitList.Count; j++)
            {
                if (weaponInitList[j].GetActorType() == i)
                {
                    //weaponInitList[j].End();
                    OnEnd?.Invoke(weaponInitList[j]);
                }
            }
        }

        public void Destroy(int i)
        {
            List<ISkillContainer> weaponList = new List<ISkillContainer>();
            for (var j = 0; j < weaponInitList.Count; j++)
            {
                if (weaponInitList[j].GetActorType() == i)
                {
                    weaponList.Add(weaponInitList[j]);
                }
            }
            //从集合中删除
            foreach (var weaponBaseContainer in weaponInitList)
            {
                weaponList.Remove(weaponBaseContainer);
                OnDestroy?.Invoke(weaponBaseContainer);
                //weaponBaseContainer.Destroy();
            }

        }

        public int GetSkillCapNum(ulong id)
        {
            throw new NotImplementedException();
        }


        public List<ISkillContainer> GetSkills()
        {
            List<ISkillContainer> skills = new List<ISkillContainer>();
            foreach (var weaponBaseContainer in weapons)
            {
                skills.Add(weaponBaseContainer);
            }
            return skills;
        }

        public int GetSkillCd(ulong id)
        {
            throw new NotImplementedException();
        }

        public void SetSkillCapNum(ulong id, int num)
        {
            throw new NotImplementedException();
        }

        public void SetWeaponCd(ulong id, int cd)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IFireControlinternalBase

        public event Action<ISkillContainer> OnFire;
        public event Action<ISkillContainer> OnEnd;
        public event Action<ISkillContainer> OnDestroy;


        #endregion





    }
}
