using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// AI对外接口
    /// AI停止 与 启动
    /// </summary>
    public interface IAIBase
    {
        /// <summary>
        /// 开启AI逻辑
        /// </summary>
        bool StartAILogic();

        /// <summary>
        /// 暂停AI逻辑
        /// </summary>
        bool PauseAILogic();

        AIComponentBase Clone(IBaseComponentContainer container);

    }

    /// <summary>
    /// AI组件对内接口
    /// 
    /// </summary>
    public interface IAIInternalBase : IAIBase
    {
        /// <summary>
        /// 获得AI状态
        /// </summary>
        int GetAIStatus();
    }
}
