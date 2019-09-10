using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DSharp.External;
using Crazy.Common;

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

        protected List<UserData> skillInitList;

        protected ILevelActorComponentBaseContainer level;

        protected IShipComponentBaseContainer container;

        protected int buttonstate = 0;

        public FireControlComponentBase(IShipComponentBaseContainer container, ILevelActorComponentBaseContainer create)
        {
            skills = new List<ISkillContainer>();
            skillInitList= new List<UserData>();
            this.container = container;
            this.level = create;
        }

        public FireControlComponentBase(IShipComponentBaseContainer container, ILevelActorComponentBaseContainer create,List<Int32> weapons)
        {
            this.container = container;
            this.skills = new List<ISkillContainer>();
            skillInitList = new List<UserData>();
            this.level = create;

            foreach (var weapon in weapons)
            {
               level.GetConfigComponentInternalBase().GetActorClone(weapon, out var actor);
               if (actor is ISkillContainer weaponBase)
                   this.skills.Add(weaponBase);
            }
            
        }

        public FireControlComponentBase(IShipComponentBaseContainer container, FireControlComponentBase clone)
        {
            this.container = container;
            this.skills = new List<ISkillContainer>();
            this.skillInitList = new List<UserData>();
            this.level = clone.level;

            foreach (var skillContainer in clone.skills)
            {
                if(skillContainer.Clone() is ISkillContainer weaponBase)
                    this.skills.Add(weaponBase);
            }
            
        }

        public void Dispose()
        {
            level = null;
            container = null;
            skillInitList.Clear();
            skillInitList = null;
            foreach (var skillContainer in skills)
            {
                skillContainer.Dispose();
            }
            skills.Clear();
            skills = null;

        }

        protected void WaitSkillDestroy(ISkillContainer weapon)
        {
            weapon.OnDestroySkill -= WaitSkillDestroy;
            skillInitList.RemoveAll(o => o.ActorID == weapon.GetActorID());
        }

        #region IFireControlBase

        public void InitializeFireControl(List<Int32> containers)
        {
            skills = new List<ISkillContainer>();
            foreach (var weapon in containers)
            {
                if(weapon == 0) continue;
                level.GetConfigComponentInternalBase().GetActorClone(weapon, out var actor);
                
                if (actor is IWeaponBaseContainer weaponBase)
                {
                    weaponBase.SetOwnerID(actor.GetActorID());
                    weaponBase.SetCamp(actor.GetCamp());
                    this.skills.Add(weaponBase);
                }
            }
        }

        protected long firetime;
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
                    case ActorTypeBaseDefine.TorpedoActor:
                    case ActorTypeBaseDefine.TrackingMissileActor:
                    //持续激光
                    case ActorTypeBaseDefine.ContinuousLaserActor:
                    //炸弹
                    case ActorTypeBaseDefine.TimeBombActor:
                    case ActorTypeBaseDefine.TriggerBombActor:
                        Fire(skilltype);
                        break;

                    case ActorTypeBaseDefine.PowerLaserActor:
                        firetime = DateTime.Now.Ticks;
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
                    case ActorTypeBaseDefine.TorpedoActor:
                    case ActorTypeBaseDefine.TrackingMissileActor:
                        break;
                    //持续激光
                    case ActorTypeBaseDefine.ContinuousLaserActor:
                    //炸弹
                    case ActorTypeBaseDefine.TimeBombActor:
                    case ActorTypeBaseDefine.TriggerBombActor:
                        Destroy(skilltype);
                        break;
                    case ActorTypeBaseDefine.PowerLaserActor:
                        firetime = DateTime.Now.Ticks - firetime;
                        Fire(skilltype);
                        firetime = 0;
                        break;

                }
            }
        }

        public void TickFire()
        {
            // 给所有的准备中武器赋值当前的Actor位置坐标和朝向
            foreach (var skillContainer in skills)
            {
                //Log.Trace( "武器箱武器 ActorType:"+skillContainer.GetActorType()+" ActorID" + skillContainer.GetActorID() + " Body id:"+skillContainer.GetBody().Id);
                //Log.Trace("当前Actor Body id" + container.GetBodyId() + " 位置" +
                //          container.GetPhysicalinternalBase().GetBody().Position + " 朝向" +
                //          container.GetPhysicalinternalBase().GetBody().Forward);
                //Log.Trace("复制前Actor Body id" + skillContainer.GetBody().Id + " 位置" +
                //          skillContainer.GetBody().Position + " 朝向" +
                //          skillContainer.GetBody().Forward);
                container.Detection(skillContainer);
                //Log.Trace("复制后Actor Body id" + skillContainer.GetBody().Id + " 位置" +
                //          skillContainer.GetBody().Position + " 朝向" +
                //          skillContainer.GetBody().Forward);
                var cd = skillContainer.GetSkillCd();
                skillContainer.SetSkillCd((cd + 1) % skillContainer.GetMaxSkillCd());
                skillContainer.SetOwnerID(container.GetActorID());
                skillContainer.SetCamp(container.GetCamp());
            }

            var weaponlist = skillInitList.Where(s => s.ActorType == ActorTypeBaseDefine.ContinuousLaserActor).ToList();
            // 给激光赋值
            foreach (var skillContainer in weaponlist)
            {
                container.RingDetection((level.GetActor(skillContainer.ActorID)));
            }
        }

        public void FireAI(int i)
        {
            //Log.Trace("FireAI: 发射槽" + i + " 发射槽总数" + skills.Count);
            if (skills.Count <= i)
            {
                //Log.Trace("FireAI: 发射槽 超过了 发射槽总数");
                return;
            }
            if (skills[i] is ISkillContainer actor)
            {

                //var weaponactor = actor.Clone();
                //weaponactor.SetActorId(level.GetCreateInternalComponentBase().GetCreateID());
                //var weapon = weaponactor as ISkillContainer;
                var id = level.GetCreateInternalComponentBase().GetCreateID();
                var weapon = actor as ISkillContainer;
                weapon.SetRelPosition(0, 0);
                //Log.Trace("当前发射者 位置：" + container.GetPhysicalinternalBase().GetBody().Position + " 朝向：" +
                //          container.GetPhysicalinternalBase().GetBody().Forward);
                //Log.Trace("当前武器箱 位置："+actor.GetBody().Position + " 朝向："+actor.GetBody().Forward);
                //Log.Trace("被发射武器"+weapon.GetBody().Id+" 位置：" + weapon.GetBody().Position + " 朝向：" + weapon.GetBody().Forward+" 朝向角度："+weapon.GetBody().Angle);


                //level.GetEnvirinfointernalBase().AddActor(weapon as ActorBase);
                var init = weapon.GetInitData();
                level.AddEventMessagesToHandlerForward(new InitEventMessage(id, actor.GetCamp(), actor.GetActorType(), init.point_x, init.point_y, init.angle,weapon.GetLinerDamping(), ownerid: weapon.GetOwnerID(), relatpoint_x: weapon.GetRelPositionX(), relatpoint_y: weapon.GetRelPositionY()));

                skillInitList.Add(new UserData(id, weapon.GetActorType()));
                OnFire?.Invoke(weapon);
            }
            
        }


        public void Fire(int i)
        {
            //Log.Trace("Fire: 发射槽" + i + " 发射槽总数"+ skills.Count);
            for (var j = 0; j < skills.Count; j++)
            {

                if (skills[j].GetActorType() == i && skills[j] is ISkillContainer actor)
                {

                    //var weaponactor = actor.Clone();
                    //weaponactor.SetActorId(level.GetCreateInternalComponentBase().GetCreateID());
                    var id = level.GetCreateInternalComponentBase().GetCreateID();
                    var weapon = actor as ISkillContainer;
                    weapon.SetRelPosition(0, 0);
                    Log.Trace("当前发射者 位置：" + container.GetPhysicalinternalBase().GetBody().GetPosition() + " 朝向：" +
                              container.GetPhysicalinternalBase().GetBody().GetAngle());
                    Log.Trace("当前武器箱 位置：" + actor.GetInitData().point_x + " " + actor.GetInitData().point_y + " 朝向：" + actor.GetInitData().angle);
                  


                    //level.GetEnvirinfointernalBase().AddActor(weapon as ActorBase);
                    //weapon.StartSkill();
                    var init = weapon.GetInitData();
                    level.AddEventMessagesToHandlerForward(new InitEventMessage(id, actor.GetCamp(), actor.GetActorType(), init.point_x, init.point_y, init.angle, weapon.GetLinerDamping(), ownerid: weapon.GetOwnerID(), relatpoint_x: weapon.GetRelPositionX(), relatpoint_y: weapon.GetRelPositionY(), time: firetime));
                    skillInitList.Add(new UserData(id,weapon.GetActorType()));
                    OnFire?.Invoke(weapon);
                }
            }
        }

        public void End(int i)
        {

            for (var j = 0; j < skillInitList.Count; j++)
            {
                if (skillInitList[j].ActorType == i)
                {
                    var actor = level.GetActor(skillInitList[j].ActorID) as ISkillContainer;
                    //actor.EndSkill();
                    level.AddEventMessagesToHandlerForward(new DestroyEventMessage(skillInitList[j].ActorID));
                    OnEnd?.Invoke(skillInitList[j].ActorID);
                }
            }
        }

        public void Destroy(int i)
        {
            List<UserData> weaponList = new List<UserData>();
            for (var j = 0; j < skillInitList.Count; j++)
            {
                if (skillInitList[j].ActorType == i)
                {
                    weaponList.Add(skillInitList[j]);
                }
            }
            //从集合中删除
            foreach (var weaponBaseContainer in weaponList)
            {
                //var actor = level.GetActor(weaponBaseContainer.ActorID) as ISkillContainer;
                skillInitList.Remove(weaponBaseContainer);
                OnDestroy?.Invoke(weaponBaseContainer.ActorID);
                level.AddEventMessagesToHandlerForward(new DestroyEventMessage(weaponBaseContainer.ActorID));

                //actor?.DestroySkill();
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
        public event Action<ulong> OnEnd;
        public event Action<ulong> OnDestroy;


        #endregion





    }
}
