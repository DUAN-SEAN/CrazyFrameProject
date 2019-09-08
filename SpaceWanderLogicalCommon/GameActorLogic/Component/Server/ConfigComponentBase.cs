using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DSharp.External;
using Crazy.Common;
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
            InitializeMap();
        }

        protected void InitializeActor(GameShipConfig[] ships, GameSkillConfig skill, GameBarrierConfig barrier)
        {

            var factory = level.GetEnvirinfointernalBase().GetFactory();

            #region 任务配置

            foreach (var barrierTaskConfig in barrier.TaskConfigs)
            {
                int id = barrierTaskConfig.Id;
                int condition = barrierTaskConfig.StartCondition;
                int result = barrierTaskConfig.Result;
                var dict = new Dictionary<int, int>();
                string des = barrierTaskConfig.Description;
                foreach (var itme in barrierTaskConfig.TaskConditionItemConfig)
                {
                    dict.Add(itme.ConditionTarget, itme.ConditionValue);
                    //Log.Trace("加载配置 任务id：" + id + " key" + itme.ConditionTarget + " value" + itme.ConditionValue);
                }

                var task1 = level.GetCreateInternalComponentBase().CreateTaskEvent(
                    condition, result, id, dict, des);
                level.AddTaskEvent(task1);
            }

            #endregion


            WeaponActorBase weaponactor = null;
            #region 武器

            #region 往前冲武器
            //高射炮
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.AntiAircraftGunActor, level);
            //weaponactor.CreateBody(factory.CreateRectangleBody(0, 0, 0.3f, 2.725f,isSensor:true));
            weaponactor.CreateInitData(new InitData {});
            weaponactor.CreateAiComponent(new GogogoAiComponent(weaponactor));
            ConfigActors.Add(ActorTypeBaseDefine.AntiAircraftGunActor, weaponactor);

            //鱼雷
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.TorpedoActor, level);
            //weaponactor.CreateBody(factory.CreateRectangleBody(0, 0, 0.3f, 2.725f, isSensor: true));
            weaponactor.CreateInitData(new InitData());
            weaponactor.CreateAiComponent(new GogogoAiComponent(weaponactor));
            ConfigActors.Add(ActorTypeBaseDefine.TorpedoActor, weaponactor);

            //机关枪
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.MachineGunActor, level);
            //weaponactor.CreateBody(factory.CreateRectangleBody(0, 0, 0.3f, 2.725f, isSensor: true));
            weaponactor.CreateInitData(new InitData());
            weaponactor.CreateAiComponent(new GogogoAiComponent(weaponactor));
            ConfigActors.Add(ActorTypeBaseDefine.MachineGunActor, weaponactor);
            #endregion


            //持续激光
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.ContinuousLaserActor, level);
            //weaponactor.CreateBody(factory.CreateRectangleBody(0, 0, 0.3f, 2.725f, isSensor: true));
            weaponactor.CreateInitData(new InitData());
            weaponactor.CreateAiComponent(null);
            ConfigActors.Add(ActorTypeBaseDefine.ContinuousLaserActor, weaponactor);




            #region 自爆炸武器
            //定时炸弹
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.TimeBombActor, level);
            //weaponactor.CreateBody(factory.CreateRectangleBody(0, 0, 0.3f, 2.725f, isSensor: true));
            weaponactor.CreateInitData(new InitData());
            weaponactor.CreateAiComponent(new DeadAiComponent(5000000000, weaponactor));
            ConfigActors.Add(ActorTypeBaseDefine.TimeBombActor, weaponactor);

            //触发炸弹
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.TriggerBombActor, level);
            //weaponactor.CreateBody(factory.CreateRectangleBody(0, 0, 0.3f, 2.725f, isSensor: true));
            weaponactor.CreateInitData(new InitData());
            weaponactor.CreateAiComponent(new DeadAiComponent(5000000000, weaponactor));
            ConfigActors.Add(ActorTypeBaseDefine.TriggerBombActor, weaponactor);

            #endregion


            //跟踪导弹
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.TrackingMissileActor, level);
            //weaponactor.CreateBody(factory.CreateRectangleBody(0, 0, 0.3f, 2.725f, isSensor: true));
            weaponactor.CreateInitData(new InitData());
            weaponactor.CreateAiComponent(null);
            ConfigActors.Add(ActorTypeBaseDefine.TrackingMissileActor, weaponactor);

            //蓄力激光
            weaponactor = new WeaponActorBase(0, ActorTypeBaseDefine.PowerLaserActor, level);
            //weaponactor.CreateBody(factory.CreateRectangleBody(0, 0, 0.3f, 2.725f, isSensor: true));
            weaponactor.CreateInitData(new InitData());
            weaponactor.CreateAiComponent(new DeadAiComponent(5000000000, weaponactor));
            ConfigActors.Add(ActorTypeBaseDefine.PowerLaserActor, weaponactor);



            #endregion



            ShipActorBase shipactor = null;
            #region 船
            //歼灭船
            shipactor = new ShipActorBase(0, ActorTypeBaseDefine.AnnihilationShipActor, level);
            //shipactor.CreateBody(factory.CreateTrapezoidBody(0, 0, 6, 14, 3));
            shipactor.CreateInitData(new InitData());
            shipactor.InitializeFireControl(new List<int> // 跟踪导弹 机关枪
            {
                ActorTypeBaseDefine.TrackingMissileActor,
                ActorTypeBaseDefine.MachineGunActor
            });
            shipactor.CreateAiComponent(new ShipEnemyAiComponent(level, shipactor));
            ConfigActors.Add(ActorTypeBaseDefine.AnnihilationShipActor, shipactor);

            //精英船A
            shipactor = new ShipActorBase(0, ActorTypeBaseDefine.EliteShipActorA, level);
            //shipactor.CreateBody(factory.CreateTrapezoidBody(0, 0, 6, 14, 3));
            shipactor.CreateInitData(new InitData());
            shipactor.CreateAiComponent(new ShipEnemyAiComponent(level, shipactor));
            shipactor.InitializeFireControl(new List<int> // 鱼雷 高射炮
            {
                ActorTypeBaseDefine.TorpedoActor,
                ActorTypeBaseDefine.AntiAircraftGunActor
            });
            ConfigActors.Add(ActorTypeBaseDefine.EliteShipActorA, shipactor);

            //精英船B
            shipactor = new ShipActorBase(0, ActorTypeBaseDefine.EliteShipActorB, level);
            //shipactor.CreateBody(factory.CreateTrapezoidBody(0, 0, 6, 14, 3));
            shipactor.CreateInitData(new InitData());
            shipactor.CreateAiComponent(new ShipEnemyAiComponent(level, shipactor));
            shipactor.InitializeFireControl(new List<int> // 跟踪导弹 高射炮
            {
                ActorTypeBaseDefine.TrackingMissileActor,
                ActorTypeBaseDefine.AntiAircraftGunActor
            });
            ConfigActors.Add(ActorTypeBaseDefine.EliteShipActorB, shipactor);

            //战斗机A
            shipactor = new ShipActorBase(0, ActorTypeBaseDefine.FighterShipActorA, level);
            //shipactor.CreateBody(factory.CreateTrapezoidBody(0, 0, 6, 14, 3));
            shipactor.CreateInitData(new InitData());
            shipactor.CreateAiComponent(new ShipEnemyAiComponent(level, shipactor));
            shipactor.InitializeFireControl(new List<int> // 机关枪 持续激光
            {
                ActorTypeBaseDefine.MachineGunActor,
                ActorTypeBaseDefine.ContinuousLaserActor
            });
            ConfigActors.Add(ActorTypeBaseDefine.FighterShipActorA, shipactor);

            //战斗机B
            shipactor = new ShipActorBase(0, ActorTypeBaseDefine.FighterShipActorB, level);
            //shipactor.CreateBody(factory.CreateTrapezoidBody(0, 0, 6, 14, 3));
            shipactor.CreateInitData(new InitData());
            shipactor.CreateAiComponent(new ShipEnemyAiComponent(level, shipactor));
            shipactor.InitializeFireControl(new List<int> // 高射炮 蓄力激光s
            {
                ActorTypeBaseDefine.AntiAircraftGunActor,
                ActorTypeBaseDefine.PowerLaserActor
            });
            ConfigActors.Add(ActorTypeBaseDefine.FighterShipActorB, shipactor);

            //无人机
            shipactor = new ShipActorBase(0, ActorTypeBaseDefine.DroneShipActor, level);
            //shipactor.CreateBody(factory.CreateTrapezoidBody(0, 0, 6, 14, 3));
            shipactor.CreateInitData(new InitData());
            shipactor.CreateAiComponent(new ShipEnemyAiComponent(level, shipactor));
            shipactor.InitializeFireControl(new List<int> // 机关枪
            {
                ActorTypeBaseDefine.MachineGunActor
            });
            ConfigActors.Add(ActorTypeBaseDefine.DroneShipActor, shipactor);

            //黄蜂飞船
            shipactor = new ShipActorBase(0, ActorTypeBaseDefine.WaspShipActorA, level);
            //shipactor.CreateBody(factory.CreateTrapezoidBody(0, 0, 6, 14, 3));
            shipactor.CreateInitData(new InitData());
            shipactor.CreateAiComponent(new ShipEnemyAiComponent(level, shipactor));
            shipactor.InitializeFireControl(new List<int> // 机关枪 高射炮
            {
                ActorTypeBaseDefine.MachineGunActor,
                ActorTypeBaseDefine.AntiAircraftGunActor
            });
            ConfigActors.Add(ActorTypeBaseDefine.WaspShipActorA, shipactor);

            #endregion

      

        }

        protected void InitializeMap()
        {
            level.SetMapSize(1000, 1000);
        }
        #region Helper

        protected Dictionary<int, int> CreateValues(int condition,Dictionary<int,int> valuesdic)
        {
            var values = valuesdic;
            switch (condition)
            {
                case TaskConditionTypeConstDefine.TimeTaskEvent:
                    foreach (var value in values)
                    {
                        values[value.Key] = value.Value * 60000;
                    }
                    break;
            }

            return values;
        }

        #endregion


        public bool TryGetActor(Int32 key, out ActorBase actor)
        {
            return ConfigActors.TryGetValue(key, out actor);
        }

        public bool GetActorClone(Int32 key, out ActorBase actor)
        {
            bool result = ConfigActors.TryGetValue(key, out var value);
            if(value == null)
            {
                Log.Trace("ConfigComponent: 克隆对象为空 key：" + key);
                actor = null;
                return false;
            }
            actor = value.Clone();
            //((IBaseComponentContainer)(actor)).GetPhysicalinternalBase().GetBody().UserData = new UserData(actor.GetActorID(), actor.GetActorType());

            return result;
        }

        public void Dispose()
        {
            level = null;
            ShipArray = null;
            Skill = null;
            BarrierArray = null;
            ConfigActors = null;
        }
    }
}
