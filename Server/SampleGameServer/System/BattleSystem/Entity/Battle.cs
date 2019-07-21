using Crazy.Common;
using MongoDB.Bson;
using SpaceShip.Base;
using SpaceShip.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
namespace GameServer.Battle
{
    /// <summary>
    /// 表示一场战斗的实体
    /// </summary>
    public class Battle : BEntity,IBroadcastHandler
    {
        public override void Start(ulong id)
        {
            base.Start(id);
            m_startTime = DateTime.Now;
            m_level = new Level();

        }
        /// <summary>
        /// 初始化战斗副本实体
        /// </summary>
        /// <param name="players">玩家集合</param>
        /// <param name="barrierId">关卡Id</param>
        /// <param name="handler">通讯句柄</param>
        public void Init(List<string> players,int barrierId,INetCumnication handler)
        {
            
            m_level.Init(barrierId,players);
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
            foreach (var item in m_bodyEntityDic.Values)
            {
                item.SyncState();//驱动每一个实体进行状态同步
            }


            //4 驱动物理引擎
            if (m_level == null)
                return;
            m_level.Tick();


        }
        
        /// <summary>
        /// 处理事件队列，广播游戏事件
        /// 1 玩家生成飞机绑定
        /// 2 攻击事件（击中）
        /// 3 AI事件（武器、道具）
        /// 
        /// n 玩家飞机解除绑定
        /// </summary>
        private void OnEventMessage()
        {
            
            var queue = m_level.GetBodyMessages();
            
            if(queue == null)
            {
                Log.Error("queue null");
                return;
            }

            while (true)
            {
                if (queue.Count <= 0)
                {
                    return;
                }
                var bodyMessage = queue.Dequeue();
                Log.Info("OnEventMessage::" + bodyMessage.GetType().ToString());

                switch (bodyMessage.GetMessageType())
                {
                     case MessageType.BodyAttack:break;
                     case MessageType.BodyAttacked:break;
                     case MessageType.BodyDestoried:
                        OnDestoryBodyEntity(bodyMessage);

                        break;
                     case MessageType.BodyInit:

                        OnInitBodyEvent(bodyMessage);

                        break;
                    default:
                        break;
                }
            }
           

            
        }

        #region OnEvent
        /// <summary>
        /// 接收到战斗逻辑层发来的生成物体事件
        /// </summary>
        /// <param name="bodyMessage"></param>
        private void OnInitBodyEvent(IBodyMessage bodyMessage)
        {
            BodyInitMessage bodyInitMessage = bodyMessage as BodyInitMessage;

            int bodyId = bodyInitMessage.GetBodyID();

            ShipBase shipBody = null;
            ShipBodyEntity shipBodyEntity = null;
            WeaponBodyEntity weaponBodyEntity = null;
            EnvirmentEntity envirmentEntity = null;
            var type = bodyInitMessage.GetBody().GetType().ToString();
            Log.Info("OnInitBodyEvent::Type = " + type);
            switch (type)
            {
                case SpaceBodyType.PlayerInBody://最特殊的
                    var playerBody = bodyInitMessage.GetBody() as PlayerInBody;
                    var playerBodyEntity = BEntityFactory.CreateEntity<PlayerBodyEntity>();
                    playerBodyEntity.Init(playerBody,this);
                    //添加到字典中
                    m_playerToBody.Add(playerBody.UserID, playerBodyEntity);
                    m_bodyEntityDic.Add(playerBody.Id.Value, playerBodyEntity);
                    break;
                case SpaceBodyType.AICarrierShipInBody://BOSS飞船
                    shipBody = bodyInitMessage.GetBody() as ShipBase;
                    shipBodyEntity = BEntityFactory.CreateEntity<ShipBodyEntity>();
                    shipBodyEntity.Init(shipBody, this);
                    //添加到字典中
                    m_bodyEntityDic.Add(shipBody.Id.Value, shipBodyEntity);
                    break;
                case SpaceBodyType.AISmallShipInBody://小AI
                    shipBody = bodyInitMessage.GetBody() as ShipBase;
                    shipBodyEntity = BEntityFactory.CreateEntity<ShipBodyEntity>();
                    shipBodyEntity.Init(shipBody, this);
                    //添加到字典中
                    m_bodyEntityDic.Add(shipBody.Id.Value, shipBodyEntity);
                    break;          
                case SpaceBodyType.MeteoriteInBody://陨石
                    var Body = bodyInitMessage.GetBody() as MeteoriteInBody;
                    envirmentEntity = BEntityFactory.CreateEntity<EnvirmentEntity>();
                    envirmentEntity.Init(Body, this);
                    //添加到字典中
                    m_bodyEntityDic.Add(Body.Id.Value, envirmentEntity);
                    break;
                case SpaceBodyType.LightInBody://激光 直线
                    var weanpon1 = bodyInitMessage.GetBody() as LightInBody;
                    weaponBodyEntity = BEntityFactory.CreateEntity<WeaponBodyEntity>();
                    weaponBodyEntity.Init(weanpon1, this);
                    //添加到字典中
                    m_bodyEntityDic.Add(weanpon1.Id.Value, weaponBodyEntity);
                    break;
                case SpaceBodyType.MissileInBody://导弹 喷火
                    var weanpon2 = bodyInitMessage.GetBody() as MissileInBody;
                    weaponBodyEntity = BEntityFactory.CreateEntity<WeaponBodyEntity>();
                    weaponBodyEntity.Init(weanpon2, this);
                    //添加到字典中
                    m_bodyEntityDic.Add(weanpon2.Id.Value, weaponBodyEntity);
                    break;
                case SpaceBodyType.MineInBody://地雷
                    var weanpon3 = bodyInitMessage.GetBody() as MineInBody;
                    weaponBodyEntity = BEntityFactory.CreateEntity<WeaponBodyEntity>();
                    weaponBodyEntity.Init(weanpon3, this);
                    //添加到字典中
                    m_bodyEntityDic.Add(weanpon3.Id.Value, weaponBodyEntity);
                    break;
                case SpaceBodyType.BoltInBody:
                    var weanpon4 = bodyInitMessage.GetBody() as BoltInBody;
                    weaponBodyEntity = BEntityFactory.CreateEntity<WeaponBodyEntity>();
                    weaponBodyEntity.Init(weanpon4, this);
                    //添加到字典中
                    m_bodyEntityDic.Add(weanpon4.Id.Value, weaponBodyEntity);
                    break;
                default: break;
            }
        }


        /// <summary>
        /// 接受到销毁事件
        /// </summary>
        private void OnDestoryBodyEntity(IBodyMessage bodyMessage)
        {
            BodyDestoriedMessage message = bodyMessage as BodyDestoriedMessage;
            ABodyEntity bodyEntity = null;
            if (!m_bodyEntityDic.TryGetValue(message.GetBodyID(),out bodyEntity))
            {
                Log.Debug("OnDestoryBodyEntity::没有销毁的body ID = "+ message.GetBodyID());
                return;
            }


            bodyEntity.Dispose();
            //从body字典中移除
            m_bodyEntityDic.Remove(message.GetBodyID());
            //从玩家body映射字典中删除
            if (message.GetBody().UserID != null)
            {
                m_playerToBody.Remove(message.GetBody().UserID);
            }
        }

        #endregion
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
        /// <summary>
        /// 通过BodyEntity的Id 获取bodyEntity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ABodyEntity GetBodyEntity(int id)
        {
            ABodyEntity bodyEntity = null;
            if(!m_bodyEntityDic.TryGetValue(id,out bodyEntity))
            {
                return null;
            }
            return bodyEntity;
        }
       

        #region IBroadcastHandler
        /// <summary>
        /// 广播战斗消息
        /// </summary>
        /// <param name="message"></param>
        public void BroadcastMessage(IBattleMessage message)
        {
            if(message.BattleId == default)
            {
                Log.Error("BroadcastMessage::BattleId is default ");
                return;
            }
            if(m_players == null)
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
            m_playerToBody.Clear();
            m_bodyEntityDic.Clear();

            m_level.Dispose();

            m_players = null;
            m_playerToBody = null;
            m_bodyEntityDic = null;
            m_level = null;
            m_netHandler = null;

            base.Dispose();
        }
        /// <summary>
        /// 玩家与Body的映射关系
        /// </summary>
        private Dictionary<string, PlayerBodyEntity> m_playerToBody = new Dictionary<string, PlayerBodyEntity>();
        /// <summary>
        /// BodyEntity字典
        /// </summary>
        private Dictionary<int, ABodyEntity> m_bodyEntityDic = new Dictionary<int, ABodyEntity>();
        
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
        private Level m_level;

        /// <summary>
        /// 玩家集合
        /// </summary>
        private List<string> m_players;

        /// <summary>
        /// 网路通信句柄
        /// </summary>
        private INetCumnication m_netHandler;

    }

    public interface IBroadcastHandler
    {
        void BroadcastMessage(IBattleMessage message);
        ulong GetBattleId();
    }
}
