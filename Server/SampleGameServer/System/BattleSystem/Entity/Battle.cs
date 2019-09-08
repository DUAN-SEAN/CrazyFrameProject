using Crazy.Common;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using GameActorLogic;
using GameServer.Configure;
using Google.Protobuf;
using Google.Protobuf.Collections;
using SpaceWanderEngine;

namespace GameServer.Battle
{
    /// <summary>
    /// 表示一场战斗的实体
    /// </summary> 
    public class Battle : BEntity, IBroadcastHandler
    {
        private long t;
        public override void Start(ulong id)
        {
            base.Start(id);
            m_isDispose = false;
            m_startTime = DateTime.Now;
            m_battleInterval = 0L;
            m_oldIntervalTime = DateTime.Now.Ticks;
            m_level = new LevelActorBase();
            m_binaryFormatter = new BinaryFormatter();
            m_level.OnLoadingDone += OnReadyBattleFromLevel;
            m_level.OnGameFail += OnGameFail;
            m_level.OnGameVictory += OnGameVictory;
            m_players = new List<string>();
            _readyDic = new Dictionary<string, int>();
            t = DateTime.Now.Ticks;
        }

        private void OnGameVictory()
        {

            Log.Trace("OnGameVictory");
            m_netHandler?.OnReleaseBattle(Id);

        }

        private void OnGameFail()
        {

            Log.Trace("OnGameFail");
            m_netHandler?.OnReleaseBattle(Id);
        }

        /// <summary>
        /// 向客户端发送可以开启战斗了
        /// </summary>
        private void OnReadyBattleFromLevel()
        {
            Log.Debug("服务器加载关卡配置完成");
            levelReady = true;
            m_readyWaitTime =
                DateTime.Now.AddMilliseconds(GameServerConstDefine.BattleSystemWaitPlayerReadySuperiorTime);
        }

        /// <summary>
        /// 初始化战斗副本实体
        /// </summary>
        /// <param name="players">玩家集合</param>
        /// <param name="barrierId">关卡Id</param>
        /// <param name="handler">通讯句柄</param>
        public void Init(List<string> players, int barrierId, IBattleSystemHandler handler)
        {
            
            foreach (var plyaerId in players)
            {
                _readyDic.Add(plyaerId,0);
            }

            int i = 0;
            foreach (var pId in players)
            {
                m_players.Add(pId);
                Log.Info("playerId = "+pId);
                i++;
            }
            Log.Info("Battle 玩家个数为："+players.Count);
            
            m_netHandler = handler;
            
            //初始化关卡配置
            GameBarrierConfig gameBarrierConfig = null;
            foreach (var item in m_netHandler.GetGameBarrierConfigs())
            {
                if (item.Id == barrierId)
                {
                    gameBarrierConfig = item;
                }
            }
            //Log.Info("barrier Id="+gameBarrierConfig.Id);
            m_level.InitConfig(gameBarrierConfig, m_netHandler.GetGameShipConfigs(),m_netHandler.GetGameSkillConfig());
            //关卡开启战斗

            m_startTime = DateTime.Now;
        }
        
        /// <summary>
        /// 由时间管理器进行驱动
        /// </summary>
        public override void Update()
        {
            
            t = DateTime.Now.Ticks;

            m_battleInterval = DateTime.Now.Ticks - m_oldIntervalTime;
            m_oldIntervalTime = DateTime.Now.Ticks;

            base.Update();
            if (IsRelease) return;
            
            CheckBattleState();
            //Log.Info("CheckReadyState" + (DateTime.Now.Ticks - t) / 10000);
            //获取是否可以开启战斗
            CheckReadyState();

            if (!canBattle) return ;
            //0 获取同步状态
            OnSyncState();

            //1 接收逻辑事件(生成、销毁、状态)
            OnEventMessage();
            
            //2 驱动物理引擎
            m_level?.Update();
            


        }
        /// <summary>
        /// 检查战斗状态
        /// </summary>
        private void CheckBattleState()
        {
            if (m_players == null || m_players.Count == 0)
            {

                m_netHandler?.OnReleaseBattle(Id);
            }
        }

        private Runner runner;
        /// <summary>
        /// 检查是否可以开始游戏
        /// </summary>
        private void CheckReadyState()
        {
            if (_readyDic != null&&!canBattle)
            {
                if (levelReady)
                {
                    Log.Trace("levelReady");

                    bool flag = _readyDic.ContainsValue(0);
                    if (!flag)
                    {
                        //Log.Trace("levelReady flag");
                        List<Tuple<string, int, int, int, int>> playerShips = new List<Tuple<string, int, int, int, int>>();

                      
                        foreach (var plyaerId in m_players)
                        {
                            

                            var plx = GameServer.Instance.PlayerCtxManager.FindPlayerContextByString(plyaerId) as GameServerPlayerContext;

                            var shipInfo = plx.m_gameServerDBPlayer.playerShip;
                            playerShips.Add(new Tuple<string, int, int, int, int>(plyaerId, shipInfo.shipId, shipInfo.shipType, shipInfo.weapon_a, shipInfo.weapon_b));
                            Log.Info("player shipType = "+shipInfo.shipType +"skillType_a = "+shipInfo.weapon_a+ "skillType_b = " + shipInfo.weapon_b);
                            //playerShips.Add(new Tuple<string, int, int, int, int>(plyaerId, shipInfo.shipId, 1004, 1012, 1013));

                        }
                        m_level.Start(playerShips);
                        

                        Log.Debug("服务器确认所有客户端关卡加载完毕完成第二次握手，可以开启战斗 ，发起第三次握手");
                        BroadcastMessage(new S2CM_ReadyBattleBarrierAck { BattleId = Id });
                        _readyDic.Clear();
                        _readyDic = null;
                        canBattle = true;
                    }
                    else
                    {
                        //等待玩家确认准备超时
                        if (m_readyWaitTime < DateTime.Now)
                        {
                            //目前直接释放资源
                            Log.Trace("等待玩家确认准备超时,直接释放资源");
                            Log.Trace("超时玩家为：");
                            foreach (var ready in _readyDic)
                            {
                                if (ready.Value == 0)
                                {
                                    Log.Trace("playerId = "+ready.Key);
                                }
                            }
                            m_netHandler.OnReleaseBattle(Id);
                        }
                    }
                        


                }
            }

        }

        private void OnSyncState()
        {
            OnSyncLevelState();

            OnSyncTaskState();

            OnSyncActor();
           
          
            //分组件封装所有的值

        }
        /// <summary>
        /// 同步关卡状态
        /// </summary>
        private void OnSyncLevelState()
        {
            S2C_SyncLevelStateBattleMessage message = new S2C_SyncLevelStateBattleMessage();
            message.BattleId = Id;
            message.IntervalTime =(long) m_level.GetDelta();
            message.Time = DateTime.Now.Ticks;
            message.Frame = m_level.GetCurrentFrame();
            BroadcastMessage(message);
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
                
                if (actor.IsShip())
                {
                    switch (actor.GetActorType())
                    {
                        case ActorTypeBaseDefine.ShipActorNone:
                        case ActorTypeBaseDefine.EliteShipActorA:
                        case ActorTypeBaseDefine.EliteShipActorB:
                        case ActorTypeBaseDefine.FighterShipActorA:
                        case ActorTypeBaseDefine.FighterShipActorB:
                        case ActorTypeBaseDefine.WaspShipActorA:
                        case ActorTypeBaseDefine.AnnihilationShipActor:
                        case ActorTypeBaseDefine.DroneShipActor:
                        case ActorTypeBaseDefine.PlayerShipActor:
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
                                //Log.Info(syncPhysics.ToJson());
                                BroadcastMessage(syncPhysics);
                            }

                            {
                                S2C_SyncSkillStateBattleMessage syncSkillMsg = new S2C_SyncSkillStateBattleMessage
                                {
                                    BattleId = Id,
                                    ActorId = shipActorBase.GetActorID()
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
                                break;
                            }
                        default: break;
                    }
                }
                else if (actor.IsWeapon())
                {
                    switch (actor.GetActorType())
                    {
                        case ActorTypeBaseDefine.AntiAircraftGunActor:
                        case ActorTypeBaseDefine.ContinuousLaserActor:
                        case ActorTypeBaseDefine.MachineGunActor:
                        case ActorTypeBaseDefine.PowerLaserActor:
                        case ActorTypeBaseDefine.TimeBombActor:
                        case ActorTypeBaseDefine.TorpedoActor:
                        case ActorTypeBaseDefine.TrackingMissileActor:
                        case ActorTypeBaseDefine.TriggerBombActor:
                            WeaponActorBase weaponActorBase = actor as WeaponActorBase;
                            S2C_SyncPhysicsStateBattleMessage syncPhysics = new S2C_SyncPhysicsStateBattleMessage
                            {
                                BattleId = Id,
                                ActorId = weaponActorBase.GetActorID(),
                                ActorType = weaponActorBase.GetActorType(),
                                AngleVelocity = weaponActorBase.GetAngleVelocity(),
                                ForceX = weaponActorBase.GetForce().X,
                                ForceY = weaponActorBase.GetForce().Y,
                                ForwardAngle = weaponActorBase.GetForwardAngle(),
                                PositionX = weaponActorBase.GetPosition().X,
                                PositionY = weaponActorBase.GetPosition().Y,

                                VelocityY = weaponActorBase.GetVelocity().Y,
                                Torque = weaponActorBase.GetTorque()
                            };
                            //Log.Info(syncPhysics.ToJson());
                            BroadcastMessage(syncPhysics);
                            break;
                            
                        default: break;
                    }
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
                syncLevelTask.Tasks.Add(taskItem);

            }
           
            //Log.Trace(syncLevelTask.ToJson());
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
                //Log.Info(e.MessageId.ToString());
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
            if (_readyDic == null) return;
            Log.Trace("OnReadyBattle"+player);
            _readyDic[player] = 1;


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
        /// 广播战斗消息,向所有玩家广播
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
            //Log.Trace(message.ToJson());
            //Log.Trace("message size = "+(message as Google.Protobuf.IMessage).CalculateSize());
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

        ~Battle()
        {
            Log.Trace("从内存中释放Battle Id = "+Id);
        }
        public override void Dispose()
        {
            if (m_isDispose)
            {
                return;
            }
            try
            {
                Log.Trace("Dispose Battle Id = " + Id);
                //if (m_netHandler.StopLogicalTimer(m_levelTimerId))
                //{
                //    Log.Trace("释放Level Timer 成功 battleId = "+Id);
                //}
                canBattle = false;;
                levelReady = false;
                m_isDispose = true;

                if (_readyDic != null)
                {
                    _readyDic.Clear();
                    _readyDic = null;
                }
                m_players?.Clear();
                m_players = null;
                m_netHandler = null;
                m_binaryFormatter = null;
                Log.Trace("战斗总时长为：" + (DateTime.Now.Ticks - m_startTime.Ticks) / 10000000 + "s");
                //todo:存入数据库

                m_level.OnLoadingDone -= OnReadyBattleFromLevel;
                m_level.OnGameFail -= OnGameFail;
                m_level.OnGameVictory -= OnGameVictory;
                
                m_level.Dispose();
                m_level = null;
                base.Dispose();

                m_readyWaitTime = DateTime.MaxValue;
                m_startTime = DateTime.MaxValue;
                m_timerId = default;
                //BEntityFactory.Recycle(this);

            }
            catch(Exception e)
            {
                Log.Error(e);
            }
          
        }




        #region BattleSystem
        public void SendCommandToLevel(ICommand commandMsg)
        {
            //Log.Fatal("Battle收到一条指令:" + commandMsg.CommandType);
            //Log.Fatal(commandMsg.tostring());
            //Log.Trace("指令序列化完成时间："+(commandMsg as Command).currenttime);
            m_level.PostCommand(commandMsg);
        }

        public void OnExitBattle(string playerId,S2C_ExitBattleMessage response)
        {
            lock (m_players)
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
        }
        public void OnReleasePlayer(string playerId)
        {
            lock (m_players)
            {
                //1 检查
                if (!m_players.Contains(playerId))
                    return;
                S2C_ExitBattleMessage response = new S2C_ExitBattleMessage
                {
                    PlayerId = playerId, BattleId = Id, State = S2C_ExitBattleMessage.Types.State.Ok
                };
                BroadcastMessage(response);
                //4 从集合删除
                m_players.Remove(playerId);
            }
            
        }
       
        #endregion

        #region 属性

        public List<string> Players => m_players;

        public bool IsRelease => m_isDispose;
        #endregion

        #region 字段

        /// <summary>
        /// 战斗开始时间
        /// </summary>
        private DateTime m_startTime;
        /// <summary>
        /// 最长等待时间
        /// </summary>
        private DateTime m_readyWaitTime;
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
        /// <summary>
        /// 战斗帧屏时间
        /// </summary>
        private long m_battleInterval;

        private long m_oldIntervalTime;

        private long m_levelTimerId;


        private BinaryFormatter m_binaryFormatter;


        private Dictionary<string, int> _readyDic;

        private bool levelReady = false;

        private bool canBattle = false;

        private bool m_isDispose = false;

        

        #endregion



    }

    public interface IBroadcastHandler
    {
        void BroadcastMessage(IBattleMessage message);
        ulong GetBattleId();
    }
}
