using Crazy.Common;
using Crazy.NetSharp;
using Crazy.ServerBase;
using GameServer.Configure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    /// <summary>
    /// 1 游戏战斗系统，由于服务器处理单服，所以战斗系统将作为模块嵌入服务器中
    /// 2 所有逻辑通过消息和玩家现场进行通信
    /// 3 游戏战斗系统的消息由GameServer负责生成驱动
    /// </summary>
    public class BattleSystem:BaseSystem, INetCumnication
    {

        public override void Update(int data1 = 0, long data2 = 0, object data3 = null)
        {
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
                    CommandBattleLocalMessage commandBattleLocalMessage = msg as CommandBattleLocalMessage;
                    OnBattleCommand(commandBattleLocalMessage);
                    break;
                case GameServerConstDefine.BattleSystemExitBattleBarrier:
                    ExitBattleLocalMessage exitBattleLocal = msg as ExitBattleLocalMessage;
                    OnExitBattle(exitBattleLocal);
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
            m_gameShipInfoConfigs =  GameServer.Instance.m_gameServerGlobalConfig.GameShipConfig;
            m_gameSkillConfig = GameServer.Instance.m_gameServerGlobalConfig.GameSkillConfig;
            if (m_gameBarrierConfigs == null) return false;
            if (m_gameShipInfoConfigs == null) return false;
            if (m_gameSkillConfig == null) return false;

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
            Battle battleEntity = BEntityFactory.CreateEntity<Battle>();



            battleEntity.Init(msg.Players, msg.BarrierId, this);
            var timerId = TimerManager.SetLoopTimer(50, battleEntity.Update);//设置Tick步长
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
            S2C_ExitBattleMessage response = new S2C_ExitBattleMessage();
            response.PlayerId = msg.PlayerId;
            response.BattleId = msg.BattleId;
            if (battle == null)
            {
                PostLocalMessageToCtx(new SystemSendNetMessage{Message = response,PlayerId = msg.PlayerId}, msg.PlayerId);
                return;
            }
            battle.OnExitBattle(msg.PlayerId,response);

        }


        /// <summary>
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
        private void OnReleaseBattle(ulong battleId)
        {
            //Battle battleEntity = null;
            //if(!m_battleDic.TryGetValue(battleId,out battleEntity))
            //{
            //    Log.Error("OnReleaseBattle Find Null");
            //}
            //TimerManager.UnsetLoopTimer(battleEntity.GetTimerId());//解除绑定

            //battleEntity.Dispose();//最终Dispose战斗实体 释放资源
            
        }
        public void SendMessageToClient(IBattleMessage message,string playerId)
        {
            PostLocalMessageToCtx(new SystemSendNetMessage { Message = message, PlayerId = playerId }, playerId);
            

        }
        public void SendMessageToClient(IBattleMessage message, List<string> players)
        {
            PostLocalMessageToCtx(new SystemSendNetMessage { Message = message }, players);

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
        /// 战斗系统的Timer管理
        /// </summary>
        public BattleTimerManager TimerManager { get; protected set; }

    }

    public interface INetCumnication
    {
        void SendMessageToClient(IBattleMessage message, string playerId);
        void SendMessageToClient(IBattleMessage message, List<string> players);

    }
}
