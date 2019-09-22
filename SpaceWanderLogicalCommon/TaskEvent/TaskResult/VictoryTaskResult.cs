using Crazy.Common;
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
            Log.Trace("任务完成：胜利");
            level.GetEventComponentBase().AddEventMessagesToHandlerForward(new VictoryEventMessage(level.GetLevelID()));
        }

        public void StartResult()
        {
        }
    }
}
