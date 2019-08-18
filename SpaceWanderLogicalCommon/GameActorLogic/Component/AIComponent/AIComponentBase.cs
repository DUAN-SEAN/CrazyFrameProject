using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public abstract class AIComponentBase : 
        IAIBase,
        IAIInternalBase
    {
        /// <summary>
        /// 是否暂停
        /// </summary>
        protected bool isPause = false;

        protected IBaseComponentContainer container;
        protected AIComponentBase(IBaseComponentContainer container)
        {
            this.container = container;
        }
        public bool StartAILogic()
        {
            isPause = false;
            return isPause;
        }

        public bool PauseAILogic()
        {
            isPause = true;
            return isPause;
        }

        /// <summary>
        /// -1表示该AI组件没有被特化 没有AI逻辑
        /// </summary>
        public int GetAIStatus()
        {
            return -1;
        }

        /// <summary>
        /// 运行AI逻辑
        /// </summary>
        public void Update()
        {
            if(isPause == true) return;
            TickLogical();

        }

        /// <summary>
        /// 被底层所重写
        /// 用来执行特定逻辑
        /// </summary>
        public virtual void TickLogical()
        {

        }

    }
}
