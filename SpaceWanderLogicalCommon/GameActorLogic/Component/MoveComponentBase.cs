using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    /// <summary>
    /// 基础移动组件
    /// </summary>
    public abstract class MoveComponentBase : 
        IMoveBase,
        IMoveinternalBase
    {

        #region IMoveBase
        public void Left()
        {
            OnLeft?.Invoke();
        }

        public void Right()
        {
            OnRight?.Invoke();
        }

        public void Forward()
        {
            OnForward?.Invoke();
        }


        #endregion

        #region IMoveinternalBase
        public event Action OnLeft;
        public event Action OnRight;
        public event Action OnForward;
        #endregion



    }
}
