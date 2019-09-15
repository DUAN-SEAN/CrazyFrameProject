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
        protected List<long> skillcd;

        protected List<UserData> skillInitList;

        protected ILevelActorComponentBaseContainer level;

        protected IShipComponentBaseContainer container;

        protected long lastframe;

        protected int buttonstate = 0;

        public FireControlComponentBase(IShipComponentBaseContainer container, ILevelActorComponentBaseContainer create)
        {
            skills = new List<ISkillContainer>();
            skillcd = new List<long>();
            skillInitList = new List<UserData>();
            this.container = container;
            this.level = create;
                lastframe = DateTime.Now.Ticks;
        }

        public FireControlComponentBase(IShipComponentBaseContainer container, ILevelActorComponentBaseContainer create,List<Int32> weapons)
        {
            this.container = container;
            this.skills = new List<ISkillContainer>();
            skillcd = new List<long>();
            skillInitList = new List<UserData>();
            this.level = create;

            foreach (var weapon in weapons)
            {
               level.GetConfigComponentInternalBase().GetActorClone(weapon, out var actor);
                if (actor is ISkillContainer weaponBase)
                {
                    this.skills.Add(weaponBase);
                    this.skillcd.Add(0);
                }
            }
                lastframe = DateTime.Now.Ticks;

        }

        public FireControlComponentBase(IShipComponentBaseContainer container, FireControlComponentBase clone)
        {
            this.container = container;
            this.skills = new List<ISkillContainer>();
            skillcd = new List<long>();
            this.skillInitList = new List<UserData>();
            this.level = clone.level;

            foreach (var skillContainer in clone.skills)
            {
                if(skillContainer.Clone() is ISkillContainer weaponBase)
                {
                    this.skills.Add(weaponBase);
                    //Log.Trace("Clone: type:" + weaponBase.GetActorType() + " cd" + weaponBase.GetSkillCd());
                    this.skillcd.Add(0);
                }
            }
                lastframe = DateTime.Now.Ticks;

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
            skillcd.Clear();
            skillcd = null;

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
            skillcd = new List<long>();
            foreach (var weapon in containers)
            {
                if(weapon == 0) continue;
                level.GetConfigComponentInternalBase().GetActorClone(weapon, out var actor);
                
                if (actor is IWeaponBaseContainer weaponBase)
                {
                    weaponBase.SetOwnerID(actor.GetActorID());
                    weaponBase.SetCamp(actor.GetCamp());
                    this.skills.Add(weaponBase);
                    skillcd.Add(0);
                }
            }
                lastframe = DateTime.Now.Ticks;
        }

        protected long firetime;
        protected float firepro;
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
                   
                        Fire(skilltype);
                        break;

                    case ActorTypeBaseDefine.PowerLaserActor:
                        firetime = DateTime.Now.Ticks;
                        Log.Trace("SendButtonState 按钮按下" + firetime);
                        break;

                    case ActorTypeBaseDefine.TriggerBombActor:
                        //var bomblist = skillInitList.Where(o => o.ActorType == ActorTypeBaseDefine.TriggerBombActor);
                        List<UserData> bomblist = new List<UserData>();
                        //找到已生成集合中所有触发炸弹的User信息
                        for (var j = 0; j < skillInitList.Count; j++)
                        {
                            if (skillInitList[j].ActorType == ActorTypeBaseDefine.TriggerBombActor)
                            {
                                //Log.Trace("SendButtonState: 已找到触发炸弹" + skillInitList[j].ActorID + " 类型" + skillInitList[j].ActorType);
                                //判断该已生成列表中的对象是否已经在世界中了
                                if(level.ContainsID(skillInitList[j].ActorID))
                                    bomblist.Add(skillInitList[j]);
                            }
                        }
                        //如果有触发炸弹则销毁
                        if (bomblist.Count() >0)
                        {
                            Destroy(bomblist);
                        }
                        else
                        {
                            Fire(skilltype);
                        }
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
                    //炸弹
                    case ActorTypeBaseDefine.TimeBombActor:
                        //TODO 触发式炸弹单独完成一个状态
                    case ActorTypeBaseDefine.TriggerBombActor:
                        break;
                    //持续激光
                    case ActorTypeBaseDefine.ContinuousLaserActor:
                  
                        Destroy(skilltype);
                        break;
                    case ActorTypeBaseDefine.PowerLaserActor:
                        var now = DateTime.Now.Ticks;
                        Log.Trace("SendButtonState: 按钮抬起" + now);
                        firetime = now - firetime;
                        Log.Trace("SendButtonState: ticks delay" + firetime);
                        firetime = firetime > 1.5 * 1e7 ? (long)(1.5 * 1e7) : firetime;
                        Log.Trace("SendButtonState: ticks delay 矫正" + firetime);
                        firepro = (float)(firetime / (1.5 * 1e7));
                        Log.Trace("SendButtonState: fire pro" + firepro);

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
                skillContainer.SetOwnerID(container.GetActorID());
                skillContainer.SetCamp(container.GetCamp());
            }
            //设置技能cd
            //更新cd集合中的当前cd
            //Tick集合中所有cd时间
            for (int i = 0; i < skillcd.Count; i++)
            {
                if (skillcd[i] == 0) continue;
                if (skills[i] == null) continue;
                //技能cd时间
                var cd = skills[i].GetSkillCd();
                if (cd == 0) cd = 1;
                //当前技能cd等于 当前时间减去发射时时间
                var currentcd = (DateTime.Now.Ticks - skillcd[i]) / 1e4;
                //如果当前cd时间大于等于技能cd 则当前cd设置为0
                if (currentcd >= cd) skillcd[i] = 0;
                //Log.Trace("TickFire skilltype:" + skills[i].GetActorType() + " MAX CD:" + cd + " " + skills[i].GetSkillCd() + " 发射时间：" + skillcd[i] + " 当前时间" + currentcd);
            }
            //lastframe = DateTime.Now.Ticks;

            var weaponlist = skillInitList.Where(s => s.ActorType == ActorTypeBaseDefine.ContinuousLaserActor).ToList();
            // 给激光赋值
            foreach (var skillContainer in weaponlist)
            {
                container.RingDetection((level.GetActor(skillContainer.ActorID)));
            }


        }

        /// <summary>
        /// AI使用发射槽来控制发射武器
        /// 普通发射使用发射类型
        /// </summary>
        /// <param name="i"></param>
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
                if (skillcd[i] != 0) return;
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
                skillcd[i] = DateTime.Now.Ticks;
            }
            
        }


        public void Fire(int i)
        {
            //Log.Trace("Fire: 发射槽" + i + " 发射槽总数"+ skills.Count);
            for (var j = 0; j < skills.Count; j++)
            {

                if (skills[j].GetActorType() == i && skills[j] is ISkillContainer actor)
                {
                if (skillcd[j] != 0) return;

                    //var weaponactor = actor.Clone();
                    //weaponactor.SetActorId(level.GetCreateInternalComponentBase().GetCreateID());
                    var id = level.GetCreateInternalComponentBase().GetCreateID();
                    var weapon = actor as ISkillContainer;
                    weapon.SetRelPosition(0, 0);
                    //Log.Trace("当前发射者 位置：" + container.GetPhysicalinternalBase().GetBody().GetPosition() + " 朝向：" +
                              //container.GetPhysicalinternalBase().GetBody().GetAngle());
                    //Log.Trace("当前武器箱 位置：" + actor.GetInitData().point_x + " " + actor.GetInitData().point_y + " 朝向：" + actor.GetInitData().angle);
                  


                    //level.GetEnvirinfointernalBase().AddActor(weapon as ActorBase);
                    //weapon.StartSkill();
                    var init = weapon.GetInitData();
                    level.AddEventMessagesToHandlerForward(new InitEventMessage(id, actor.GetCamp(), actor.GetActorType(), init.point_x, init.point_y, init.angle, weapon.GetLinerDamping(), ownerid: weapon.GetOwnerID(), relatpoint_x: weapon.GetRelPositionX(), relatpoint_y: weapon.GetRelPositionY(), time: firepro));
                    skillInitList.Add(new UserData(id,weapon.GetActorType()));
                    OnFire?.Invoke(weapon);
                    skillcd[j] = DateTime.Now.Ticks;
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

        /// <summary>
        /// 触发删除该类型的所有Actor武器
        /// </summary>
        public void Destroy(int i)
        {
            List<UserData> weaponList = new List<UserData>();
            for (var j = 0; j < skillInitList.Count; j++)
            {
                if (skillInitList[j].ActorType == i)
                {
                    //判断该生成列表中的对象是否已经在世界中
                    if (level.ContainsID(skillInitList[j].ActorID))
                        weaponList.Add(skillInitList[j]);
                }
            }
            //从集合中删除
            foreach (var weaponBaseContainer in weaponList)
            {
                //var actor = level.GetActor(weaponBaseContainer.ActorID) as ISkillContainer;
                skillInitList.Remove(weaponBaseContainer);
                OnDestroy?.Invoke(weaponBaseContainer.ActorID);
                if (level.GetActor(weaponBaseContainer.ActorID) is ISkillComponentContainer weapon)
                {
                    weapon.DestroySkill();
                }

                //actor?.DestroySkill();
            }

        }

        /// <summary>
        /// 触发删除该集合中的actor武器
        /// </summary>
        public void Destroy(IEnumerable<UserData> list)
        {

            for(int i=0;i<list.Count(); i++)
            {
                var weaponBaseContainer = list.ElementAt(i);
                
                OnDestroy?.Invoke(weaponBaseContainer.ActorID);
                
                if(level.GetActor(weaponBaseContainer.ActorID) is ISkillComponentContainer weapon)
                {
                    weapon.DestroySkill();
                }
                //Log.Trace("Destory 火控组件销毁一个武器"+weaponBaseContainer.ActorID+" 类型"+weaponBaseContainer.ActorType);
                skillInitList.Remove(weaponBaseContainer);
            }
            //foreach (var weaponBaseContainer in list)
            //{
            //    //var actor = level.GetActor(weaponBaseContainer.ActorID) as ISkillContainer;
            //    skillInitList.Remove(weaponBaseContainer);
            //    OnDestroy?.Invoke(weaponBaseContainer.ActorID);
            //    level.AddEventMessagesToHandlerForward(new DestroyEventMessage(weaponBaseContainer.ActorID));

            //    //actor?.DestroySkill();
            //}
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
