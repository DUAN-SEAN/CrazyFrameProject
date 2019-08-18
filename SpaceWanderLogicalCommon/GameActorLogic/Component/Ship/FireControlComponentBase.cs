using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.External;
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
            //TODO 技能控制 0 按下 1抬起
            if (skillcontrol == 0)
            {
                switch (skilltype)
                {
                    //发射型武器
                    case ActorTypeBaseDefine.AntiAircraftGunActor:
                    case ActorTypeBaseDefine.MachineGunActor:
                    case ActorTypeBaseDefine.PowerLaserActor:
                    case ActorTypeBaseDefine.TorpedoActor:
                    case ActorTypeBaseDefine.TrackingMissileActor:
                    //持续激光
                    case ActorTypeBaseDefine.ContinuousLaserActor:
                    //炸弹
                    case ActorTypeBaseDefine.TimeBombActor:
                    case ActorTypeBaseDefine.TriggerBombActor:
                        Fire(skilltype);
                        break;


                }
            }
            else if (skillcontrol == 1)
            {
                switch (skilltype)
                {
                    //发射型武器
                    case ActorTypeBaseDefine.AntiAircraftGunActor:
                    case ActorTypeBaseDefine.MachineGunActor:
                    case ActorTypeBaseDefine.PowerLaserActor:
                    case ActorTypeBaseDefine.TorpedoActor:
                    case ActorTypeBaseDefine.TrackingMissileActor:
                        break;
                    //持续激光
                    case ActorTypeBaseDefine.ContinuousLaserActor:
                    case ActorTypeBaseDefine.TimeBombActor:
                    case ActorTypeBaseDefine.TriggerBombActor:
                        Destroy(skilltype);
                        break;


                }
            }
        }

        public void TickFire()
        {
            // 给所有的准备中武器赋值当前的Actor位置坐标和朝向
            foreach (var skillContainer in skills)
            {
                container.GetPhysicalinternalBase().GetBody().TriggerDetection(skillContainer.GetBody(), 1);
                var cd = skillContainer.GetSkillCd();
                skillContainer.SetSkillCd((cd + 1) % skillContainer.GetMaxSkillCd());
            }

            var weaponlist = skillInitList.Where(s => s.GetActorType() == ActorTypeBaseDefine.ContinuousLaserActor).ToList();
            // 给激光赋值
            foreach (var skillContainer in weaponlist)
            {
                container.GetPhysicalinternalBase().GetBody().TriggerDetection(skillContainer.GetBody(), 1);
            }
        }



        public void Fire(int i)
        {
            for (var j = 0; j < skills.Count; j++)
            {
                if (skills[j].GetActorType() == i && skills[j] is ISkillContainer actor)
                {
                    var weapon = actor.Clone() as ISkillContainer;
                    skillInitList.Add(weapon);
                    weapon.OnDestroySkill += WaitSkillDestroy;
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

        public int GetSkillCapNum(int type)
        {
            return skills.Find(s => s.GetActorType() == type).GetSkillCapacity();
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

        public int GetSkillCd(int type)
        {
            return skills.Find(s => s.GetActorType() == type).GetSkillCd();
        }

        public void SetSkillCapNum(int type, int num)
        {
            skills.Find(s => s.GetActorType() == type).SetSkillCapacity(num);
        }

        public void SetSkillCd(int type, int cd)
        {
            skills.Find(s => s.GetActorType() == type).SetSkillCd(cd);
        }

        #endregion

        #region IFireControlinternalBase

        public event Action<ISkillContainer> OnFire;
        public event Action<ISkillContainer> OnEnd;
        public event Action<ISkillContainer> OnDestroy;


        #endregion





    }
}
