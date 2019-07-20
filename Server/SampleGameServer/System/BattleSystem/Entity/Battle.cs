using SpaceShip.Base;
using System;
using System.Collections.Generic;
using System.Linq;
namespace GameServer.Battle
{
    /// <summary>
    /// 表示一场战斗的实体
    /// </summary>
    public class Battle : BEntity
    {
        public override void Start(ulong id)
        {
            base.Start(id);
            m_startTime = DateTime.Now;
            m_level = new Level();

        }

        public void Init(List<string> players,int barrierId)
        {
            
            //m_level.Init(players);
        }
         
        /// <summary>
        /// 由时间管理器进行驱动
        /// </summary>
        public override void Update()
        {
            base.Update();

            //1 接收网络指令
            


            //2 接收逻辑事件(生成、销毁)
            OnEventMessage();



            //3 同步状态（各个战斗实体根据自身特性同步状态）
            foreach (var item in m_bodyEntityDic.Values)
            {
                item.SyncState();
            }


            //4 驱动物理引擎
            if (m_level == null)
            {
                return;
            }
            m_level.Tick();


        }
        /// <summary>
        /// 创建一个body通信实体
        /// </summary>
        /// <param name="bodyId"></param>
        private void CreateBodyEntity()
        {
            
        }
        /// <summary>
        /// 绑定body通信实体
        /// </summary>
        private void BindBodyEntity()
        {

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
            while (true)
            {
                var bodyMessage = queue.Dequeue();
                if (bodyMessage == null) break;
               
                switch (bodyMessage.GetMessageType())
                {
                     case MessageType.BodyAttack:break;
                     case MessageType.BodyAttacked:break;
                     case MessageType.BodyDestoried:break;
                     case MessageType.BodyInit:

                        BodyInitMessage bodyInitMessage = bodyMessage as BodyInitMessage;
                        
                        int bodyId = bodyInitMessage.GetBodyID();

                        switch (bodyInitMessage.GetBody().GetType())
                        {
                            
                        }
                        
                        break;
                     default:
                        break;
                }
            }
           

            
        }
      
     
        /// <summary>
        /// 接受到销毁事件
        /// </summary>
        private void OnDestoryBodyEntity()
        {

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
        /// <summary>
        /// 通过BodyEntity的Id 获取bodyEntity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ABodyEntity GetBodyEntity(ulong id)
        {
            ABodyEntity bodyEntity = null;
            if(!m_bodyEntityDic.TryGetValue(id,out bodyEntity))
            {
                return null;
            }
            return bodyEntity;
        }
        public override void Dispose()
        {
            base.Dispose();
            m_playerToBody.Clear();
            m_bodyEntityDic.Clear();
        }
        /// <summary>
        /// 玩家与Body的映射关系
        /// </summary>
        private Dictionary<string, ulong> m_playerToBody = new Dictionary<string, ulong>();
        /// <summary>
        /// BodyEntity字典
        /// </summary>
        private Dictionary<ulong, ABodyEntity> m_bodyEntityDic = new Dictionary<ulong, ABodyEntity>();
        
        /// <summary>
        /// 战斗开始时间
        /// </summary>
        private DateTime m_startTime;
        /// <summary>
        /// timeId
        /// </summary>
        private long m_timerId;

        private Level m_level;

    }
}
