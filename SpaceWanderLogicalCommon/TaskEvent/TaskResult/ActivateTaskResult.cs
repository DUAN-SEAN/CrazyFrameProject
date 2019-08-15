using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class ActivateTaskResult : ITaskResult
    {
        protected ILevelActorComponentBaseContainer level;
        //将要激活的激活ID
        protected ulong activateid;
        public ActivateTaskResult(ILevelActorComponentBaseContainer level,ulong activateid)
        {
            this.level = level;
            this.activateid = activateid;
        }
        public void Execute()
        {
            level.GeTaskEventComponentInternalBase().ActivateTask(activateid);
        }
    }
}
