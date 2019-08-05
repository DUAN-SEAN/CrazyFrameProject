using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 关卡Actor 对外接口
    /// </summary>
    public interface ILevelActorBaseContainer:
        IEventComponentBase
    {
        /// <summary>
        /// 获得ID
        /// </summary>
        long GetLevelID();

        

        /// <summary>
        /// 开启关卡
        /// </summary>
        void Start();

        
        /// <summary>
        /// 清理关卡现场
        /// </summary>
        void Dispose();
    }

    /// <summary>
    /// 关卡Actor 对内接口
    /// </summary>
    public interface ILevelActorComponentBaseContainer
    {
        /// <summary>
        /// 获取事件组件
        /// </summary>
        IEventComponentBase GetEventComponentBase();
    }
}
