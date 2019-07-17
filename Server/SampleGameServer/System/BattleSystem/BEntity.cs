using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    /// <summary>
    /// 战斗实体，战斗的载体
    /// 飞船 导弹 子弹 陨石，一切需要通信的实体
    /// </summary>
    public abstract class BEntity
    {
        
        /// <summary>
        /// 初始化实体
        /// </summary>
        /// <param name="id"></param>
        public virtual void Start(ulong id)
        {
            m_Id = id;
            IsDispose = false;
        }
        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDispose) return;
            m_Id = 0;
            IsDispose = true;
            BEntityFactory.Recycle(this);

        }

        /// <summary>
        /// 1 由系统驱动tick，用于更新实体的状态
        /// 2 如果是玩家控制  用于模拟控制器如PlayerController AIController
        /// 3 获取本地消息队列，处理本地消息（其他系统边界），例如玩家的输入，现场带来的消息
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// 外部只能获取不能修改该字段
        /// </summary>
        public ulong Id { get; }
        /// <summary>
        /// 实体的唯一表示
        /// </summary>
        private ulong m_Id;
        /// <summary>
        /// 是否被dispose
        /// </summary>
        private bool IsDispose;
        
            
    }
}
