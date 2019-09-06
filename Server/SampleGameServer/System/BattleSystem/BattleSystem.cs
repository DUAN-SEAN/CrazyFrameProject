using Crazy.Common;
using Crazy.NetSharp;
using Crazy.ServerBase;
using GameServer.Configure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace GameServer.Battle
{
    /// <summary>
    /// 1 游戏战斗系统，由于服务器处理单服，所以战斗系统将作为模块嵌入服务器中
    /// 2 所有逻辑通过消息和玩家现场进行通信
    /// 3 游戏战斗系统的消息由GameServer负责生成驱动
    /// </summary>
    public class BattleSystem:BaseSystem, IBattleSystemHandler
    {
        public override void Update()
        {
            //Log.Trace("BattleSystem::LocalMessageCount = " + p_localMessages.Count);
            base.Update();
        }

        public override void Update(int data1 = 0, long data2 = 0, object data3 = null)
        {
            base.Update(data1, data2, data3);
            base.Update(data1, data2, data3);
           
        }

        public override Task OnMessage(ILocalMessage msg)
        {
            
            switch (msg.MessageId)
            {
                case GameServerConstDefine.BattleSystemCreateBattleBarrier:
                    OnCreateBattleBarrier((CreateBattleBarrierMessage)msg);
                    break;
                case GameServerConstDefine.BattleSystemCommandUpLoad:
                    var commandBattleLocalMessage = msg as CommandBattleLocalMessage;
                    OnBattleCommand(commandBattleLocalMessage);
                    break;
                case GameServerConstDefine.BattleSystemExitBattleBarrier:
                    var exitBattleLocal = msg as ExitBattleLocalMessage;
                    OnExitBattle(exitBattleLocal);
                    break;
                case GameServerConstDefine.BattleSystemClientReadyBattle:
                    var clientReadyBattleLocalMessage = msg as ClientReadyBattleLocalMessage;
                    OnReadyBattle(clientReadyBattleLocalMessage);
                    break;
                case GameServerConstDefine.BattleSystemPlayerShutdown:
                    var toBattlePlayerShutdownMessage = msg as ToBattlePlayerShutdownMessage;
                    OnPlayerShutdown(toBattlePlayerShutdownMessage.playerId);
                    break;
                case GameServerConstDefine.BattleSystemNeedReleaseBattleTimer:
                    var releaseBattleTimerLocalMessage = msg as ReleaseBattleTimerLocalMessage;
                    OnReleaseBattleTimer(releaseBattleTimerLocalMessage.TimerId);
                    break;
                default: return base.OnMessage(msg);
            }
            return Task.CompletedTask;

        }

        

        /// <summary>
        /// GameServer初始化战斗系统
        /// 1 获取关卡配置文件
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            //获取关卡配置文件,获取所有的游戏匹配信息
            m_gameBarrierConfigs = GameServer.Instance.m_gameServerGlobalConfig.BarrierConfigs;
            Log.Info("m_gameBarrierConfigs 2 count = "+m_gameBarrierConfigs[1].TaskConfigs.Length.ToString());
            m_gameShipInfoConfigs =  GameServer.Instance.m_gameServerGlobalConfig.GameShipConfig;
            m_gameSkillConfig = GameServer.Instance.m_gameServerGlobalConfig.GameSkillConfig;
            m_gameBattleConfig = GameServer.Instance.m_gameServerGlobalConfig.GameBattleConfig;
            if (m_gameBarrierConfigs == null) return false;
            if (m_gameShipInfoConfigs == null) return false;
            if (m_gameSkillConfig == null) return false;
            if (m_gameBattleConfig == null) return false;

            TimerManager = new BattleTimerManager();
            TimerManager.Start();
            return true;
        }
        /// <summary>
        /// 创建战斗关卡
        /// 由匹配系统分配的玩家队伍创建新的战斗场景
        /// 1 读取关卡id验证id合法性
        /// 2 读取玩家信息验证玩家合法性
        /// 3 生成战斗场景
        /// 4 与玩家进行实时通信
        /// </summary>
        private void OnCreateBattleBarrier(CreateBattleBarrierMessage msg)
        {
            ////TODO:战斗场景在此生成
            ////每场关卡运行在独立的线程中
            //Battle battleEntity = BEntityFactory.CreateEntity<Battle>();
            Battle battleEntity = new Battle();
            battleEntity.Start(BattleGenerateId++);

            battleEntity.Init(msg.Players, msg.BarrierId, this);
            //Log.Debug(m_gameBattleConfig.BattleTickTime+"   "+"Battle Update");
            var timerId = TimerManager.SetLoopTimer(m_gameBattleConfig.BattleTickTime, battleEntity.Update);//设置Tick步长
            battleEntity.SetTimer(timerId);
            //字典添加
            m_battleDic.Add(battleEntity.Id, battleEntity);
            //向玩家现场客户端发送战斗创建成功的消息，Ps 所有战斗消息目前都这样写
            var info = new S2CM_CreateBattleBarrier.Types.CreateBattleBarrierInfo();
            info.PlayerIds.Add(msg.Players);
            info.BattleId = battleEntity.Id;
            foreach (var item in msg.Players)
            {
                PostLocalMessageToCtx(new SystemSendNetMessage { Message = new S2CM_CreateBattleBarrier { BattleId = battleEntity.Id, BattleInfo = info }, PlayerId = item }, item);
            }

        }
        /// <summary>
        /// 收到从服务器或客户端发来的玩家退出战斗的请求
        /// </summary>
        /// <param name="exitBattleLocal"></param>
        private void OnExitBattle(ExitBattleLocalMessage msg)
        {
            m_battleDic.TryGetValue(msg.BattleId, out Battle battle);
            S2C_ExitBattleMessage response = new S2C_ExitBattleMessage
            {
                PlayerId = msg.PlayerId, BattleId = msg.BattleId
            };
            if (battle == null)
            {
                PostLocalMessageToCtx(new SystemSendNetMessage{Message = response,PlayerId = msg.PlayerId}, msg.PlayerId);
                return;
            }
            battle.OnExitBattle(msg.PlayerId,response);

        }


        /// <summary>
        /// 弃用了原因是太慢
        /// 玩家发来的战斗指令
        /// 根据battleId进行转发 
        /// </summary>
        /// <param name="commandBattleLocalMessage"></param>
        private void OnBattleCommand(CommandBattleLocalMessage msg)
        {
            var battleId = msg.battleId;
            var commandMsg = msg.ICommand;
            if (m_battleDic.TryGetValue(battleId,out Battle battle))
            {
                battle.SendCommandToLevel(commandMsg);
            }

        }
        /// <summary>
        /// 玩家确认战斗
        /// </summary>
        /// <param name="clientReadyBattleLocalMessage"></param>
        private void OnReadyBattle(ClientReadyBattleLocalMessage clientReadyBattleLocalMessage)
        {
            var battleId = clientReadyBattleLocalMessage.battleId;
            if (m_battleDic.TryGetValue(battleId, out Battle battle))
            {
                Log.Trace("收到玩家发来的战斗准备完成请求 PlayerId = "+ clientReadyBattleLocalMessage.playerId);
                battle.OnReadyBattle(clientReadyBattleLocalMessage.playerId);
            }
        }
        /// <summary>
        /// 玩家现场发来的关闭
        /// </summary>
        /// <param name="playerId"></param>
        private void OnPlayerShutdown(string playerId)
        {
            Log.Trace("battlesystem::关闭玩家战斗");
            foreach (var battle in m_battleDic.Values)
            {
                if(battle.IsRelease) continue;
                if (battle.Players.Contains(playerId))
                {
                    battle.OnReleasePlayer(playerId);
                }
            }
        }

        /// <summary>
        /// 释放关卡战斗资源：
        /// 修改关卡战斗实体状态为正在关闭
        /// 暂停所有物理层逻辑
        /// 释放所有物理单位
        /// 释放应用层玩家
        /// 发送匹配队伍系统玩家更新状态
        /// 修改关卡战斗实体状态为已关闭
        /// 解除战斗系统关于关卡战斗实体的注册
        /// PS 该方法可以由各层调用：实体自身、战斗系统、GameSever
        /// </summary>
        public void OnReleaseBattle(ulong battleId)
        {
            if (!m_battleDic.TryGetValue(battleId, out var battleEntity))
            {
                Log.Error("OnReleaseBattle Find Null");
                return;
            }
            //玩家大于0 说明战斗是正常结束，则计算反给客户端玩家退出战斗
            if (battleEntity.Players.Count > 0)
            {

            }
            var timerId = battleEntity.GetTimerId();
            lock (battleEntity)
            {
                battleEntity.Dispose();//最终Dispose战斗实体 释放资源
            }

            m_battleDic.Remove(battleId);

            //向当前系统发送关闭Timer
            PostLocalMessage(new ReleaseBattleTimerLocalMessage {TimerId = timerId});


        }
        public void OnReleaseBattleTimer(long timerId)
        {
            TimerManager.UnsetLoopTimer(timerId);//解除绑定
            Log.Trace("释放一场战斗Timer Id = "+timerId );
        }
        /// <summary>
        /// 向玩家发送战斗消息
        /// </summary>
        /// <param name="message">battleMessage</param>
        /// <param name="playerId">玩家Id</param>
        public void SendMessageToClient(IBattleMessage message,string playerId)
        {
            PostLocalMessageToCtx(new SystemSendNetMessage { Message = message, PlayerId = playerId }, playerId);
            

        }
        /// <summary>
        /// 向玩家发送战斗消息
        /// </summary>
        /// <param name="message">battleMessage</param>
        /// <param name="players">玩家Id集合</param>
        public void SendMessageToClient(IBattleMessage message, List<string> players)
        {
            PostLocalMessageToCtx(new SystemSendNetMessage { Message = message }, players);

        }

        public long StartLogicalTimer(BattleTimerManager.OnBattleTimerCallBack job)
        {
            return TimerManager.SetLoopTimer(m_gameBattleConfig.LevelTickTime, job);
        }

        public bool StopLogicalTimer(long id)
        {
            return TimerManager.UnsetLoopTimer(id);
        } 
        public Battle GetBattleEntity(ulong battleId)
        {
            if(m_battleDic.TryGetValue(battleId,out Battle battle))
            {
                return battle;
            }
            return null;
        }
        /// <summary>
        /// 获取战斗关卡配置
        /// </summary>
        /// <returns></returns>
        public List<GameBarrierConfig> GetGameBarrierConfigs()
        {
            
            return m_gameBarrierConfigs?.ToList();
        }
        /// <summary>
        /// 获取飞船配置
        /// </summary>
        /// <returns></returns>
        public List<GameShipConfig> GetGameShipConfigs()
        {
            return m_gameShipInfoConfigs?.ToList();
        }
        /// <summary>
        /// 获取技能配置
        /// </summary>
        /// <returns></returns>
        public GameSkillConfig GetGameSkillConfig()
        {
            return m_gameSkillConfig;
        }

        /// <summary>
        /// 战斗实体字典
        /// </summary>
        private readonly Dictionary<ulong, Battle> m_battleDic = new Dictionary<ulong, Battle>();

        /// <summary>
        /// 战斗关卡的匹配文件
        /// </summary>
        private GameBarrierConfig[] m_gameBarrierConfigs;
        /// <summary>
        /// 飞船配置信息
        /// </summary>
        private GameShipConfig[] m_gameShipInfoConfigs;
        /// <summary>
        /// 飞船配置信息
        /// </summary>
        private GameSkillConfig m_gameSkillConfig;
        /// <summary>
        /// 战斗配置
        /// </summary>
        public GameBattleConfig m_gameBattleConfig;
        /// <summary>
        /// 战斗系统的Timer管理
        /// </summary>
        public BattleTimerManager TimerManager { get; protected set; }
        /// <summary>
        /// 生成battleId
        /// </summary>
        private static UInt64 BattleGenerateId = 1;
    }

    public interface IBattleSystemHandler
    {
        void SendMessageToClient(IBattleMessage message, string playerId);
        void SendMessageToClient(IBattleMessage message, List<string> players);

        long StartLogicalTimer(BattleTimerManager.OnBattleTimerCallBack job);

        bool StopLogicalTimer(long id);


        List<GameBarrierConfig> GetGameBarrierConfigs();

        List<GameShipConfig> GetGameShipConfigs();

        GameSkillConfig GetGameSkillConfig();

        void OnReleaseBattle(ulong battleId);
    }
}
