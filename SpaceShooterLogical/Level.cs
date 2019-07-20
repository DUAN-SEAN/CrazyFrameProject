using System;
using System.Collections.Generic;

using CrazyEngine;
using SpaceShip.AI;
using SpaceShip.Factory;
using SpaceShip.System;

namespace SpaceShip.Base
{
    public sealed class Level : ISBSeanDuan,ITickable,IGetWorld
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
        private Queue<IBodyMessage> messages;


        public Level()
        {
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="players">玩家编号</param>
        public void Init(int levelID,List<string> players)
        {
            world = new World();
            weaponGameLogic = new WeaponGameLogic();
            enemyLogic = new AIEnemyLogic();
            engine = new Engine(world);
            messages = new Queue<IBodyMessage>();

            //BodyFactory.Instance.LoadShipBodyByType<PlayerInBody>(this,Label.BLUE)

            LevelDataFactory.Instance.LoadingLeveldataByID(levelID, this);
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

        public void SendCommand(Command command)
        {
            foreach (var body in world.Bodies)
            {
                if (body.Id.Value == command.ID)
                {
                    body.AddForce(command.Force);
                    body.Forward = command.Forward;
                    var player = body as PlayerInBody;
                    if(player != null)
                    {
                        if(command.attackCommand.attackState)
                            player.AttackByType(command.attackCommand.type);
                        if (command.unattackCommand.unattackState)
                            player.UnAttackByType(command.unattackCommand.type);
                    }
                    break;

                }
            }
        }


        public void ForceByShipID(int id , Vector2 vector2)
        {
            foreach(var body in world.Bodies)
            {
                if (body.Id.Value == id)
                    body.AddForce(vector2);
            }
        }

        public void ForwardByShipID(int id,Vector2 forward)
        {
            foreach(var body in world.Bodies)
            {

                if(body.Id.Value == id)      
                    body.Forward = forward;
                
            }
        }

        public void AttackByShipID(int id,int attacktype)
        {
            foreach(var body in world.Bodies)
            {
                var player = body as PlayerInBody;
                if (player == null) return;
                player.AttackByType(attacktype);

            }

        }

        public void UnAttackByShipID(int id, int attacktype)
        {
            foreach (var body in world.Bodies)
            {
                var player = body as PlayerInBody;
                if (player == null) return;
                player.UnAttackByType(attacktype);

            }
        }

        public void Dispose()
        {
            enemyLogic.Dispose();
            world.Dispose();
            weaponGameLogic.Dispose();
            engine.Dispose();
        }

        public Queue<IBodyMessage> GetBodyMessages()
        {
            return messages;
        }

      
    }






}
