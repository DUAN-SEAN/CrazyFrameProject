using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    /// <summary>
    /// 武器事件组件基础实现
    /// </summary>
    public abstract class WeanponEventComponentBase : 
        IWeaponEventBase,
        IWeaponEventinternalBase
    {
        #region IWeaponEventBaes
        public void Start()
        {
            OnStart?.Invoke();
        }

        public void End()
        {
            OnEnd?.Invoke();
        }

        public void Destroy()
        {
            OnDestroy?.Invoke();
        }
        #endregion

        #region IWeaponEventinternalBase

        public event Action OnStart;
        public event Action OnEnd;
        public event Action OnDestroy;

        #endregion



    }
}
