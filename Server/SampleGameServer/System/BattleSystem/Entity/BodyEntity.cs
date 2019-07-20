using CrazyEngine;
using SpaceShip.Base;
using SpaceShip.Factory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using Google.Protobuf;

namespace GameServer.Battle
{
    /// <summary>
    /// 表示一个世界中的引擎控制的实体
    /// </summary>
    public abstract class ABodyEntity:BEntity
    {


        /// <summary>
        /// 同步状态
        /// 同步内容：位置、血量
        /// </summary>
        public virtual void SyncState()
        {
            var position = m_body.Position;
            var force = m_body.Force;
            var forward = m_body.Forward;
            var accleration = m_body.Acceleration;





            //Command
            
        }

        public ABodyEntity()
        {
            
        }
        /// <summary>
        /// 需要重写并向客户端发送生成消息
        /// </summary>
        /// <param name="body"></param>
        public virtual void Init(Body body,IBroadcastHandler handler)
        {
            m_body = body;
            broadcastHandler = handler;
            using(MemoryStream memory = new MemoryStream())
            {
                formatter.Serialize(memory, body);
                ByteString bs = ByteString.FromStream(memory);
                S2C_BodyInitBattleMessage msg = new S2C_BodyInitBattleMessage { BattleId = handler.GetBattleId(), BodyType = body.GetType().ToString(), PlayerId = body.UserID, Body = bs };
                handler.BroadcastMessage(msg);


            }
            



        }
        /// <summary>
        /// 将实体回收
        /// </summary>
        public override void Dispose()
        {
            m_body.Dispose();
            m_body = null;
            broadcastHandler = null;
            formatter = null;



            //最终要调用基类，放入池子中
            base.Dispose();

            
        }
       
        /// <summary>
        /// 绑定的body实体
        /// </summary>
        protected Body m_body;

        /// <summary>
        /// 广播句柄
        /// </summary>
        protected IBroadcastHandler broadcastHandler;


        protected BinaryFormatter formatter = new BinaryFormatter();

    }


    public class ShipBodyEntity : ABodyEntity
    {
      

        public override void Init(Body body, IBroadcastHandler handler)
        {
            base.Init(body, handler);
            m_shipBase = body as ShipBase;
            //向客户端发送

        }

        public override void SyncState()
        {
            base.SyncState();
        }
        public override void Dispose()
        {
            m_shipBase = null;
            base.Dispose();
        }
        private ShipBase m_shipBase;



    }


    /// <summary>
    /// 玩家通信实体，继承自ABodyEntity
    /// </summary>
    public sealed class PlayerBodyEntity : ShipBodyEntity
    {
        public PlayerBodyEntity() { }

        /// <summary>
        /// 玩家实体的同步逻辑
        /// </summary>
        public override void SyncState()
        {
            var hp = m_playerInBody.HP;
            var force = m_playerInBody.Force;
            var forward = m_playerInBody.Forward;
            var accleration =  m_playerInBody.Acceleration;
            var id =  m_playerInBody.Id;
            var position = m_playerInBody.Position;
            var velocity = m_playerInBody.Velocity;
            



        }

        public override void Init(Body body, IBroadcastHandler handler)
        {
            base.Init(body,handler);
            m_playerInBody = body as PlayerInBody;
        }

        public override void Dispose()
        {
            
            m_playerInBody = null;
            base.Dispose();
        }

        private PlayerInBody m_playerInBody;
    }

  


}
