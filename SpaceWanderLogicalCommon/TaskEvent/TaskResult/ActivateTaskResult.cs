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
        protected int activateid;
        public ActivateTaskResult(ILevelActorComponentBaseContainer level,int activateid)
        {
            this.level = level;
            this.activateid = activateid;
        }

        public void Dispose()
        {
            level = null;
        }
        public void Execute()
        {
            level.GeTaskEventComponentInternalBase().ActivateTask(activateid);
        }
    }
}
