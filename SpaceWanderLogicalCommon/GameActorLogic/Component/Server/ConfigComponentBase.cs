using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Configure;

namespace GameActorLogic
{
    public class ConfigComponentBase:
        IConfigComponentBase,
        IConfigComponentInternalBase
    {
        protected GameShipConfig[] ShipArray;
        protected GameSkillConfig Skill;
        protected GameBarrierConfig[] BarrierArray;

        /// <summary>
        /// 从配置文件中加载出来的Actor对象
        /// </summary>
        protected Dictionary<Int32, ActorBase> ConfigActors;

        public void InitializeConfig(GameShipConfig[] ships, GameSkillConfig skill, GameBarrierConfig[] barrier)
        {
            ShipArray = ships;
            Skill = skill;
            BarrierArray = barrier;
            InitializeActor(ships, skill, barrier);
        }

        protected void InitializeActor(GameShipConfig[] ships, GameSkillConfig skill, GameBarrierConfig[] barrier)
        {

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
