using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    /// <summary>
    /// //TODO 这个类应该是朱颖的一个可受碰撞的实体
    /// </summary>
    public abstract class ColliderComponentBase : 
        IColliderBase,
        IColliderinternal
    {
        #region IColliderBase

        #endregion

        #region IColliderinternal

        public event Action OnColliderEnter;
        public event Action OnColliderStay;
        public event Action OnColliderExit;

        #endregion

    }
}
