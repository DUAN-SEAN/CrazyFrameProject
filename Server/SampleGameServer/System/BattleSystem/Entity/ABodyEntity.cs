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

        }
        
    }
}
