using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine;
using SpaceShip.AI;
using SpaceShip.Factory;
using SpaceShip.System;

namespace SpaceShip.Base
{
    public sealed class SeanD : ISBSeanDuan,ITickable,IGetWorld
    {
        /// <summary>
        /// 物理引擎世界集合
        /// </summary>
        private World world;

        /// <summary>
        /// 物理引起驱动器
        /// </summary>
        private Engine engine;


        /// <summary>
        /// 武器逻辑驱动
        /// </summary>
        private WeaponGameLogic weaponGameLogic;

        /// <summary>
        /// AI逻辑驱动
        /// </summary>
        private AIEnemyLogic enemyLogic;

        /// <summary>
        /// body生成信息
        /// </summary>
        private List<IBodyMessage> messages;


        public SeanD()
        {
           
        }

        public void Init()
        {
            world = new World();
            weaponGameLogic = new WeaponGameLogic();
            enemyLogic = new AIEnemyLogic();
            messages = new List<IBodyMessage>();
        }
        public void Tick()
        {
            enemyLogic.Tick();
            weaponGameLogic.Tick();
            engine.Tick();
        }

        public World GetCurrentWorld()
        {

            return world;
        }

        #region AI集合
        public IAILog GetAILog()
        {
            return enemyLogic;
        }
        #endregion

        public List<ITickable> GetWeanponList()
        {
            return weaponGameLogic.moveWeaponsList;
        }

        public IGetWorld GetWorld()
        {
            return this;
        }




        public void Dispose()
        {
            enemyLogic.Dispose();
            world.Dispose();
            weaponGameLogic.Dispose();
            engine.Dispose();
        }

        public List<IBodyMessage> GetBodyMessages()
        {
            return messages;
        }

      
    }



}
