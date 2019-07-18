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

            //1 接收网络指令




            //2 接收世界指令(生成、销毁)




            //3 同步状态（各个战斗实体根据自身特性同步状态）
            foreach (var item in m_bodyEntityDic.Values)
            {
                item.SyncState();
            }


            //4 驱动物理引擎




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
        private BodyEntity GetBodyEntity(ulong id)
        {
            BodyEntity bodyEntity = null;
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
        private Dictionary<ulong, BodyEntity> m_bodyEntityDic = new Dictionary<ulong, BodyEntity>();
        
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
