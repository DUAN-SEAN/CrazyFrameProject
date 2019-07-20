using CrazyEngine;
using SpaceShip.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ABodyEntity(Body body)
        {
            m_body = body;
        }

        
        /// <summary>
        /// 绑定的body实体
        /// </summary>
        protected Body m_body;

        


        
        
    }

    public sealed class PlayerBodyEntity : BEntity
    {
        
    }

}
