using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public abstract class AIComponentBase : 
        IAIBase,
        IAIinternalBase
    {



        public bool StartAILogic()
        {
            return true;
        }

        public bool PauseAILogic()
        {
            return true;
        }

        /// <summary>
        /// -1表示该AI组件没有被特化 没有AI逻辑
        /// </summary>
        public int GetAIStatus()
        {
            return -1;
        }


        //public 
    }
}
