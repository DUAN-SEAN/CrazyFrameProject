using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    /// <summary>
    /// 基础飞船事件组件
    /// </summary>
    public abstract class ShipEventComponentBase:
        IShipEventBase,
        IShipEventinternalBase
    {

        #region IShipEventBase

        public void Init()
        {
            OnInit?.Invoke();
        }

        public void Destroy()
        {
            OnDestroy?.Invoke();
        }
        #endregion

        #region IShipEventinternalBase
        public event Action OnInit;
        public event Action OnDestroy;
        #endregion

    }
}
