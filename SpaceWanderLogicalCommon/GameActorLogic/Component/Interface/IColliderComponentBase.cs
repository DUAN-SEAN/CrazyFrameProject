using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 对外碰撞机
    /// </summary>
    public interface IColliderBase
    {
    }

    /// <summary>
    /// 对内碰撞机事件
    /// </summary>
    public interface IColliderinternal: IColliderBase
    {
        event Action OnColliderStay;
    }
}
