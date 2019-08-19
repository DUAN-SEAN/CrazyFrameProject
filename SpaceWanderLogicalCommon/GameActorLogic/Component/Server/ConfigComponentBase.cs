using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Base;
using GameServer.Configure;

namespace GameActorLogic
{
    /// <summary>
    /// 配置组件
    /// </summary>
    public class ConfigComponentBase :
        IConfigComponentBase,
        IConfigComponentInternalBase
    {
        protected GameShipConfig[] ShipArray;
        protected GameSkillConfig Skill;
        protected GameBarrierConfig BarrierArray;
        protected ILevelActorComponentBaseContainer level;

        /// <summary>
        /// 从配置文件中加载出来的Actor对象
        /// </summary>
        protected Dictionary<Int32, ActorBase> ConfigActors;

        public ConfigComponentBase(ILevelActorComponentBaseContainer level)
        {
            this.level = level;
            ConfigActors = new Dictionary<int, ActorBase>();

        }


        public void InitializeConfig(GameShipConfig[] ships, GameSkillConfig skill, GameBarrierConfig barrier)
        {
            ShipArray = ships;
            Skill = skill;
            BarrierArray = barrier;
            InitializeActor(ships, skill, barrier);
        }

        protected void InitializeActor(GameShipConfig[] ships, GameSkillConfig skill, GameBarrierConfig barrier)
        {
            #region 任务配置

            

            #endregion


            WeaponActorBase weaponactor = null;
            #region 武器

            #region 往前冲武器
            //高射炮
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.AntiAircraftGunActor, level);
            weaponactor.CreateBody(Factory.CreateRectangleBody(0, 0, 0.3, 2.725));
            weaponactor.CreateAiComponent(new GogogoAiComponent(weaponactor));
            ConfigActors.Add(ActorTypeBaseDefine.AntiAircraftGunActor, weaponactor);

            //鱼雷
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.TorpedoActor, level);
            weaponactor.CreateBody(Factory.CreateRectangleBody(0, 0, 0.3, 2.725));
            weaponactor.CreateAiComponent(new GogogoAiComponent(weaponactor));
            ConfigActors.Add(ActorTypeBaseDefine.TorpedoActor, weaponactor);

            //机关枪
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.MachineGunActor, level);
            weaponactor.CreateBody(Factory.CreateRectangleBody(0, 0, 0.3, 2.725));
            weaponactor.CreateAiComponent(new GogogoAiComponent(weaponactor));
            ConfigActors.Add(ActorTypeBaseDefine.MachineGunActor, weaponactor);
            #endregion


            //持续激光
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.ContinuousLaserActor, level);
            weaponactor.CreateBody(Factory.CreateRectangleBody(0, 0, 0.3, 2.725, isTrigger: true));
            weaponactor.CreateAiComponent(null);
            ConfigActors.Add(ActorTypeBaseDefine.ContinuousLaserActor, weaponactor);




            #region 自爆炸武器
            //定时炸弹
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.TimeBombActor, level);
            weaponactor.CreateBody(Factory.CreateRectangleBody(0, 0, 0.3, 2.725));
            weaponactor.CreateAiComponent(new DeadAiComponent(5000, weaponactor));
            ConfigActors.Add(ActorTypeBaseDefine.TimeBombActor, weaponactor);

            //触发炸弹
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.TriggerBombActor, level);
            weaponactor.CreateBody(Factory.CreateRectangleBody(0, 0, 0.3, 2.725));
            weaponactor.CreateAiComponent(new DeadAiComponent(5000, weaponactor));
            ConfigActors.Add(ActorTypeBaseDefine.TriggerBombActor, weaponactor);

            #endregion


            //跟踪导弹
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.TrackingMissileActor, level);
            weaponactor.CreateBody(Factory.CreateRectangleBody(0, 0, 0.3, 2.725));
            weaponactor.CreateAiComponent(null);
            ConfigActors.Add(ActorTypeBaseDefine.TrackingMissileActor, weaponactor);

            //蓄力激光
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.PowerLaserActor, level);
            weaponactor.CreateBody(Factory.CreateRectangleBody(0, 0, 0.3, 2.725, isTrigger: true));
            weaponactor.CreateAiComponent(new DeadAiComponent(5, weaponactor));
            ConfigActors.Add(ActorTypeBaseDefine.PowerLaserActor, weaponactor);



            #endregion



            ShipActorBase shipactor = null;
            #region 船
            //歼灭船
            shipactor = new ShipActorBase(0, ActorTypeBaseDefine.AnnihilationShipActor, level);
            shipactor.CreateBody(Factory.CreateTrapezoidBody(0, 0, 6, 14, 3));
            shipactor.InitializeFireControl(new List<int> // 跟踪导弹 机关枪
            {
                ActorTypeBaseDefine.TrackingMissileActor,
                ActorTypeBaseDefine.MachineGunActor
            });
            ConfigActors.Add(ActorTypeBaseDefine.AnnihilationShipActor, shipactor);

            //精英船A
            shipactor = new ShipActorBase(0, ActorTypeBaseDefine.EliteShipActorA, level);
            shipactor.CreateBody(Factory.CreateTrapezoidBody(0, 0, 6, 14, 3));
            shipactor.InitializeFireControl(new List<int> // 鱼雷 高射炮
            {
                ActorTypeBaseDefine.TorpedoActor,
                ActorTypeBaseDefine.AntiAircraftGunActor
            });
            ConfigActors.Add(ActorTypeBaseDefine.EliteShipActorA, shipactor);

            //精英船B
            shipactor = new ShipActorBase(0, ActorTypeBaseDefine.EliteShipActorB, level);
            shipactor.CreateBody(Factory.CreateTrapezoidBody(0, 0, 6, 14, 3));
            shipactor.InitializeFireControl(new List<int> // 跟踪导弹 高射炮
            {
                ActorTypeBaseDefine.TrackingMissileActor,
                ActorTypeBaseDefine.AntiAircraftGunActor
            });
            ConfigActors.Add(ActorTypeBaseDefine.EliteShipActorB, shipactor);

            //战斗机A
            shipactor = new ShipActorBase(0, ActorTypeBaseDefine.FighterShipActorA, level);
            shipactor.CreateBody(Factory.CreateTrapezoidBody(0, 0, 6, 14, 3));
            shipactor.InitializeFireControl(new List<int> // 机关枪 持续激光
            {
                ActorTypeBaseDefine.MachineGunActor,
                ActorTypeBaseDefine.ContinuousLaserActor
            });
            ConfigActors.Add(ActorTypeBaseDefine.FighterShipActorA, shipactor);

            //战斗机B
            shipactor = new ShipActorBase(0, ActorTypeBaseDefine.FighterShipActorB, level);
            shipactor.CreateBody(Factory.CreateTrapezoidBody(0, 0, 6, 14, 3));
            shipactor.InitializeFireControl(new List<int> // 高射炮 蓄力激光
            {
                ActorTypeBaseDefine.AntiAircraftGunActor,
                ActorTypeBaseDefine.PowerLaserActor
            });
            ConfigActors.Add(ActorTypeBaseDefine.FighterShipActorB, shipactor);

            //无人机
            shipactor = new ShipActorBase(0, ActorTypeBaseDefine.DroneShipActor, level);
            shipactor.CreateBody(Factory.CreateTrapezoidBody(0, 0, 6, 14, 3));
            shipactor.InitializeFireControl(new List<int> // 机关枪
            {
                ActorTypeBaseDefine.MachineGunActor
            });
            ConfigActors.Add(ActorTypeBaseDefine.DroneShipActor, shipactor);

            //黄蜂飞船
            shipactor = new ShipActorBase(0, ActorTypeBaseDefine.WaspShipActorA, level);
            shipactor.CreateBody(Factory.CreateTrapezoidBody(0, 0, 6, 14, 3));
            shipactor.InitializeFireControl(new List<int> // 机关枪 高射炮
            {
                ActorTypeBaseDefine.MachineGunActor,
                ActorTypeBaseDefine.AntiAircraftGunActor
            });
            ConfigActors.Add(ActorTypeBaseDefine.WaspShipActorA, shipactor);

            #endregion

      

        }



        public bool TryGetActor(Int32 key, out ActorBase actor)
        {
            return ConfigActors.TryGetValue(key, out actor);
        }

        public bool GetActorClone(Int32 key, out ActorBase actor)
        {
            bool result = ConfigActors.TryGetValue(key, out var value);
            actor = value.Clone();
            return result;
        }

    }
}
