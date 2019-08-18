using Crazy.Common;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using GameActorLogic;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace GameServer.Battle
{
    /// <summary>
    /// 表示一场战斗的实体
    /// </summary> 
    public class Battle : BEntity, IBroadcastHandler
    {
        public override void Start(ulong id)
        {
            base.Start(id);
            m_startTime = DateTime.Now;
            m_level = new LevelActorBase();
            m_binaryFormatter = new BinaryFormatter();
            m_level.OnLoadingDone += OnReadyBattleFromLevel;

        }
        /// <summary>
        /// 向客户端发送可以开启战斗了
        /// </summary>
        private void OnReadyBattleFromLevel()
        {
            levelReady = true;
        }

        /// <summary>
        /// 初始化战斗副本实体
        /// </summary>
        /// <param name="players">玩家集合</param>
        /// <param name="barrierId">关卡Id</param>
        /// <param name="handler">通讯句柄</param>
        public void Init(List<string> players, int barrierId, IBattleSystemHandler handler)
        {
            List<Tuple<string,int,int,int,int>> playerShips = new List<Tuple<string, int, int, int, int>>();
            readState = new List<int>(players.Count+1);
            foreach (var plyaerId in players)
            {
                readState.Add(0);

                var plx = GameServer.Instance.PlayerCtxManager.FindPlayerContextByString(plyaerId) as GameServerPlayerContext;

                var shipInfo =  plx.m_gameServerDBPlayer.playerShip;
                playerShips.Add(new Tuple<string, int, int, int, int>(plyaerId, shipInfo.shipId, shipInfo.shipType, shipInfo.weapon_a, shipInfo.weapon_b));

            }
            
            m_players = players;
            m_netHandler = handler;

            //初始化配置
            m_level.InitConfig(m_netHandler.GetGameBarrierConfigs(),m_netHandler.GetGameShipConfigs(),m_netHandler.GetGameSkillConfig());
            //关卡开启战斗
            m_level.Start(playerShips, barrierId);

            m_startTime = DateTime.Now;
        }
        
        /// <summary>
        /// 由时间管理器进行驱动
        /// </summary>
        public override void Update()
        {
            base.Update();
            //获取是否可以开启战斗
            CheckReadyState();
            //0 获取同步状态
            OnSyncState();

            //1 接收逻辑事件(生成、销毁、状态)
            OnEventMessage();
            
            //2 驱动物理引擎
            if (m_level == null)
                return;
            m_level.Update();


        }
        /// <summary>
        /// 检查是否可以开始游戏
        /// </summary>
        private void CheckReadyState()
        {
            if (readState != null)
            {
                bool flag = true;
                foreach (var i in readState)
                {
                    if (i == 0)
                        flag = false;
                }

                if (flag)
                {
                    if (levelReady)
                    {
                        Log.Debug("服务器确认所有客户端关卡加载完毕完成第二次握手，可以开启战斗 ，发起第三次握手");
                        BroadcastMessage(new S2CM_ReadyBattleBarrierAck { BattleId = Id });
                        readState.Clear();
                        readState = null;
                    }
                        


                }
            }

        }

        private void OnSyncState()
        {
            OnSyncTaskState();

            OnSyncActor();
           
          
            //分组件封装所有的值

        }
        /// <summary>
        /// 同步Actor，除了关卡actor
        /// </summary>
        private void OnSyncActor()
        {
            //获取所有关卡内的actor
            var actors = m_level.GetAllActors();
            foreach (var actor in actors)
            {
                actor.IsShip();
                actor.IsPlayer();
                switch (actor.GetActorType())
                {
                    case ActorTypeBaseDefine.ShipActorNone:
                        ShipActorBase shipActorBase = actor as ShipActorBase;

                    {
                        S2C_SyncHpShieldStateBattleMessage syncHpShield = new S2C_SyncHpShieldStateBattleMessage
                        {
                            BattleId = Id,
                            ActorId = shipActorBase.GetActorID(),
                            ActorType = shipActorBase.GetActorType(),
                            Hp = shipActorBase.GetHP(),
                            Shield = shipActorBase.GetShieldNum()
                        };

                        BroadcastMessage(syncHpShield);
                    }

                    {
                        S2C_SyncPhysicsStateBattleMessage syncPhysics = new S2C_SyncPhysicsStateBattleMessage
                        {
                            BattleId = Id,
                            ActorId = shipActorBase.GetActorID(),
                            ActorType = shipActorBase.GetActorType(),
                            AngleVelocity = shipActorBase.GetAngleVelocity(),
                            ForceX = shipActorBase.GetForce().X,
                            ForceY = shipActorBase.GetForce().Y,
                            ForwardAngle = shipActorBase.GetForwardAngle(),
                            PositionX = shipActorBase.GetPosition().X,
                            PositionY = shipActorBase.GetPosition().Y,
                            VelocityX = shipActorBase.GetVelocity().X,
                            VelocityY = shipActorBase.GetVelocity().Y,
                            Torque = shipActorBase.GetTorque()
                        };

                        BroadcastMessage(syncPhysics);
                    }
                      
                    {
                        S2C_SyncSkillStateBattleMessage syncSkillMsg = new S2C_SyncSkillStateBattleMessage
                        {
                            BattleId = Id, ActorId = shipActorBase.GetActorID()
                        };
                        foreach (var s in shipActorBase.GetSkills())
                        {
                            S2C_SyncSkillStateBattleMessage.Types.SkillState skill =
                                new S2C_SyncSkillStateBattleMessage.Types.SkillState
                                {
                                    ActorId = s.GetActorID(),
                                    SkillType = s.GetActorType(),
                                    CD = s.GetSkillCd(),
                                    Count = s.GetSkillCapacity()
                                };


                            //获取技能Id
                            //获取技能类型 

                        }
                        BroadcastMessage(syncSkillMsg);
                        //syncSkillMsg.Skills.Add();

                    }


                        break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// 同步任务状态
        /// </summary>
        private void OnSyncTaskState()
        {
            S2C_SyncLevelTaskBattleMessage syncLevelTask = new S2C_SyncLevelTaskBattleMessage
            {
                ActorId = 0, BattleId = Id
            };
            syncLevelTask.Tasks.Clear();//如果从池子中取的话可能涉及到未清理干净的现象
            foreach (var task in m_level.GetAllTaskEvents())
            {
                S2C_SyncLevelTaskBattleMessage.Types.TaskState taskItem =
                    new S2C_SyncLevelTaskBattleMessage.Types.TaskState
                    {
                        Id = task.GetTaskId(), State = (int) task.GetTaskState()
                    };

                foreach (var taskConditionCurrentValue in task.ConditionCurrentValues)
                {

                    taskItem.Conditions.Add(taskConditionCurrentValue.Key, taskConditionCurrentValue.Value);
                }

            }
            syncLevelTask.BattleId = Id;
            BroadcastMessage(syncLevelTask);
        }


        /// <summary>
        /// 处理事件队列，广播游戏事件
        /// 1 玩家生成飞机绑定
        /// 2 攻击事件（击中）
        /// 3 AI事件（武器、道具）
        /// 4 同步事件
        /// n 玩家飞机解除绑定
        /// </summary>
        private void OnEventMessage()
        {
            var eventList =  m_level.GetForWardEventMessages();



            foreach (var e in eventList)
            {
                S2C_EventBattleMessage eventBattleMessage = new S2C_EventBattleMessage();
                using (MemoryStream ms = new MemoryStream())
                {
                    m_binaryFormatter.Serialize(ms,e);
                    eventBattleMessage.Event = ByteString.CopyFrom(ms.GetBuffer());
                    eventBattleMessage.BattleId = Id;
                    BroadcastMessage(eventBattleMessage);
                }

            }



        }
        /// <summary>
        /// 玩家发来的准备确认
        /// </summary>
        /// <param name="player"></param>
        public void OnReadyBattle(string player)
        {
            if (readState == null) return;

            var index = Players.IndexOf(player);

            readState[index] = 1;

            
        }
       
        public void SetTimer(long timerId)
        {
            m_timerId = timerId;
        }
        /// <summary>
        /// 返回TimerId
        /// </summary>
        /// <returns></returns>
        public long GetTimerId()
        {
            return m_timerId;
        }
     

        #region IBroadcastHandler
        /// <summary>
        /// 广播战斗消息
        /// </summary>
        /// <param name="message"></param>
        public void BroadcastMessage(IBattleMessage message)
        {
            if (message.BattleId == default)
            {
                Log.Error("BroadcastMessage::BattleId is default ");
                return;
            }
            if (m_players == null)
            {
                Log.Debug("BroadcastMessage::m_players is NULL");
            }
            //Log.Info(message.ToJson());
            //Log.Info("message size = "+(message as Google.Protobuf.IMessage).CalculateSize());
            m_netHandler.SendMessageToClient(message, m_players);
        }
        /// <summary>
        /// 获取战斗实体Id
        /// </summary>
        /// <returns></returns>
        public ulong GetBattleId()
        {
            return Id;
        }
        #endregion

        public override void Dispose()
        {
            Log.Info("Dispose Battle Id = " + Id);

            m_players.Clear();

            m_level.Dispose();

            m_players = null;
            m_level = null;
            m_netHandler = null;

            base.Dispose();
        }

        #region BattleSystem
        public void SendCommandToLevel(ICommand commandMsg)
        {
            m_level.PostCommand(commandMsg);
        }

        public void OnExitBattle(string playerId,S2C_ExitBattleMessage response)
        {
            //1 检查
            if (!m_players.Contains(playerId))
                return;
            //2 todo:通知关卡实体移除对应的飞船实体

            //3 广播
            response.State = S2C_ExitBattleMessage.Types.State.Ok;
            response.BattleId = Id;
            BroadcastMessage(response);
            //4 从集合删除
            m_players.Remove(playerId);
        }

        #endregion

        #region 属性

        public List<string> Players;

        #endregion

        #region 字段

        /// <summary>
        /// 战斗开始时间
        /// </summary>
        private DateTime m_startTime;
        /// <summary>
        /// timeId
        /// </summary>
        private long m_timerId;

        /// <summary>
        /// 逻辑数据层实体
        /// </summary>
        private LevelActorBase m_level;

        /// <summary>
        /// 玩家集合
        /// </summary>
        private List<string> m_players;

        /// <summary>
        /// 网路通信句柄
        /// </summary>
        private IBattleSystemHandler m_netHandler;

        private BinaryFormatter m_binaryFormatter;

        private List<int> readState;

        private bool levelReady = false;

        #endregion


    }

    public interface IBroadcastHandler
    {
        void BroadcastMessage(IBattleMessage message);
        ulong GetBattleId();

    }
}
