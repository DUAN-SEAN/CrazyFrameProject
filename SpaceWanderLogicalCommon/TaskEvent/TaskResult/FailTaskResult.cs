using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 发送失败事件
    /// </summary>
    public class FailTaskResult : ITaskResult
    {
        protected ILevelActorComponentBaseContainer level;
        public FailTaskResult(ILevelActorComponentBaseContainer level)
        {
            this.level = level;
        }

        public void Dispose()
        {
            level = null;
        }
        public void Execute()
        {
            level.GetEventComponentBase().AddEventMessagesToHandlerForward(new FailEventMessage(level.GetLevelID()));
        }

        public void StartResult()
        {
        }
    }
}
