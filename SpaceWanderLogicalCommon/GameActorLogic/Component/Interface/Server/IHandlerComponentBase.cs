using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public interface IHandlerComponentBase
    {
        #region HandlerEvent

        /// <summary>
        /// 当HandMessageList处理生成事件时回调
        /// </summary>
        event Action<ulong> OnInitMessageHandler;

        /// <summary>
        /// 当HandMessageList处理销毁事件时回调
        /// </summary>
        event Action<ulong> OnDestroyMessageHandler;

        /// <summary>
        /// 当HandMessageList处理任务更新事件时回调
        /// </summary>
        event Action<int> OnTaskUpdateMessageHandler;

        /// <summary>
        /// 当胜利调用
        /// </summary>
        event Action OnGameVictory;

        /// <summary>
        /// 当失败调用未实现
        /// </summary>
        event Action OnGameFail;
        #endregion
    }

    public interface IHandlerComponentInternalBase : IHandlerComponentBase
    {

    }
}
