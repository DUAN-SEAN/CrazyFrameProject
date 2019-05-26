using Crazy.NetSharp;
using Crazy.ServerBase;
using GameServer.Configure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    /// <summary>
    /// 1 游戏战斗系统，由于服务器处理单服，所以战斗系统将作为模块嵌入服务器中
    /// 2 所有逻辑通过消息和玩家现场进行通信
    /// 3 游戏战斗系统的消息由GameServer负责生成驱动
    /// </summary>
    public class BattleSystem:BaseSystem
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
            if (m_gameBarrierConfigs == null) return false;


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
            //TODO:战斗场景在此生成




            //向玩家现场客户端发送战斗创建成功的消息，Ps 所有战斗消息目前都这样写
            foreach (var item in msg.Players)
            {
                PostLocalMessageToCtx(new SystemSendNetMessage { Message = null, PlayerId = item },item);
            }
        }
      
        private GameBarrierConfig[] m_gameBarrierConfigs;

    }
}
