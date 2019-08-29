using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 发送胜利事件
    /// </summary>
    public class VictoryTaskResult:ITaskResult
    {
        protected ILevelActorComponentBaseContainer level;
        public VictoryTaskResult(ILevelActorComponentBaseContainer level)
        {
            this.level = level;
        }

        public void Dispose()
        {
            level = null;
        }
        public void Execute()
        {
            level.GetEventComponentBase().AddEventMessagesToHandlerForward(new VictoryEventMessage(level.GetLevelID()));
        }
    }
}
