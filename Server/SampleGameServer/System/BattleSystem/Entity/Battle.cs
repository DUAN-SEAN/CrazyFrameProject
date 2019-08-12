using Crazy.Common;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using GameActorLogic;
using Google.Protobuf;

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

        }
        /// <summary>
        /// 初始化战斗副本实体
        /// </summary>
        /// <param name="players">玩家集合</param>
        /// <param name="barrierId">关卡Id</param>
        /// <param name="handler">通讯句柄</param>
        public void Init(List<string> players, int barrierId, INetCumnication handler)
        {

            m_level.Start(players,barrierId);
            m_players = players;
            m_netHandler = handler;
        }

        /// <summary>
        /// 由时间管理器进行驱动
        /// </summary>
        public override void Update()
        {
            base.Update();

            //1 接收网络指令，将收集的指令下发给系统



            //2 接收逻辑事件(生成、销毁)
            OnEventMessage();



            //3 同步状态（各个战斗实体根据自身特性同步状态）
            


            //4 驱动物理引擎
            if (m_level == null)
                return;
            m_level.Update();


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
            
        }


        #endregion

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
        private INetCumnication m_netHandler;

        private BinaryFormatter m_binaryFormatter;

       
    }

    public interface IBroadcastHandler
    {
        void BroadcastMessage(IBattleMessage message);
        ulong GetBattleId();
    }
}
