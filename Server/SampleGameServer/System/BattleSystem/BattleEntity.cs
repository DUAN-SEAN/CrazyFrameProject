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


       

        public override void Start(ulong id)
        {
            base.Start(id);
            m_startTime = DateTime.Now;
        }
        /// <summary>
        /// 由时间管理器进行驱动
        /// </summary>
        public override void Update()
        {
            base.Update();






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




        public override void Dispose()
        {
            base.Dispose();
        }

        /// <summary>
        /// 战斗开始时间
        /// </summary>
        private DateTime m_startTime;
        /// <summary>
        /// timeId
        /// </summary>
        private long m_timerId;
        
    }
}
