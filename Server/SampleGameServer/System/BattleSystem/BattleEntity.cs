using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    /// <summary>
    /// 表示一场战斗的实体
    /// </summary>
    public class BattleEntity : BEntity
    {


        public override void Dispose()
        {
            base.Dispose();
        }

        public override void Init(ulong id)
        {
            base.Init(id);
        }
        /// <summary>
        /// 由时间管理器进行驱动
        /// </summary>
        public override void Update()
        {
            base.Update();

        }
        /// <summary>
        /// 战斗开始时间
        /// </summary>
        private DateTime m_startTime;
        
    }
}
