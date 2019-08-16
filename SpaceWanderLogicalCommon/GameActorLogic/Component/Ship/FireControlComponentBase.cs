using System;
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
        protected List<ISkillContainer> skills;

        protected List<ISkillContainer> skillInitList;

        protected ILevelActorComponentBaseContainer level;

        protected IShipComponentBaseContainer container;

        protected int buttonstate = 0;

        public FireControlComponentBase(IShipComponentBaseContainer container, ILevelActorComponentBaseContainer create)
        {
            skills = new List<ISkillContainer>();
            this.container = container;
            this.level = create;
        }

        public FireControlComponentBase(IShipComponentBaseContainer container, ILevelActorComponentBaseContainer create,List<Int32> weapons)
        {
            this.container = container;
            this.skills = new List<ISkillContainer>();
            this.level = create;

            foreach (var weapon in weapons)
            {
               level.GetConfigComponentInternalBase().GetActorClone(weapon, out var actor);
               if (actor is ISkillContainer weaponBase)
                   this.skills.Add(weaponBase);
            }
            
        }

        protected void WaitSkillDestroy(ISkillContainer weapon)
        {
            skillInitList.Remove(weapon);
        }

        #region IFireControlBase

        public void InitializeFireControl(List<Int32> containers)
        {
            skills = new List<ISkillContainer>();
            foreach (var weapon in containers)
            {
                level.GetConfigComponentInternalBase().GetActorClone(weapon, out var actor);
                if (actor is IWeaponBaseContainer weaponBase)
                    this.skills.Add(weaponBase);
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
            for (var j = 0; j < skills.Count; j++)
            {
                if (skills[j].GetActorType() == i && skills[j] is ISkillContainer actor)
                {
                    var weapon = actor.Clone() as IWeaponBaseContainer;
                    skillInitList.Add(weapon);
                    weapon.OnDestroyWeapon += WaitSkillDestroy;
                    level.GetEnvirinfointernalBase().AddActor(weapon as ActorBase);
                    weapon.StartSkill();
                    OnFire?.Invoke(weapon);
                }
            }
        }

        public void End(int i)
        {

            for (var j = 0; j < skillInitList.Count; j++)
            {
                if (skillInitList[j].GetActorType() == i)
                {
                    skillInitList[j].EndSkill();
                    OnEnd?.Invoke(skillInitList[j]);
                }
            }
        }

        public void Destroy(int i)
        {
            List<ISkillContainer> weaponList = new List<ISkillContainer>();
            for (var j = 0; j < skillInitList.Count; j++)
            {
                if (skillInitList[j].GetActorType() == i)
                {
                    weaponList.Add(skillInitList[j]);
                }
            }
            //从集合中删除
            foreach (var weaponBaseContainer in skillInitList)
            {
                weaponList.Remove(weaponBaseContainer);
                OnDestroy?.Invoke(weaponBaseContainer);
                weaponBaseContainer.DestroySkill();
            }

        }

        public int GetSkillCapNum(ulong id)
        {
            throw new NotImplementedException();
        }


        public List<ISkillContainer> GetSkills()
        {
            List<ISkillContainer> skills = new List<ISkillContainer>();
            foreach (var weaponBaseContainer in this.skills)
            {
                skills.Add(weaponBaseContainer);
            }
            return skills;
        }

        public int GetSkillCd(ulong id)
        {
            return skills.Find(s => s.GetActorID() == id).GetSkillCd();
        }

        public void SetSkillCapNum(ulong id, int num)
        {
            skills.Find(s=>s.GetActorID() == id).SetSkillCapacity(num);
        }

        public void SetWeaponCd(ulong id, int cd)
        {
            skills.Find(s => s.GetActorID() == id).SetSkillCd(cd);
        }

        #endregion

        #region IFireControlinternalBase

        public event Action<ISkillContainer> OnFire;
        public event Action<ISkillContainer> OnEnd;
        public event Action<ISkillContainer> OnDestroy;


        #endregion





    }
}
