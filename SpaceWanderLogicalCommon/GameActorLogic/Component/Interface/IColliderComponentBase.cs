using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DSharp.Dynamics;


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
    public interface IColliderInternal: IColliderBase
    {
        /// <summary>
        /// 碰撞机被进入调用一次
        /// </summary>
        event Action<UserData> OnColliderEnter;
        /// <summary>
        /// 碰撞机中有碰撞持续调用
        /// </summary>
        event Action<UserData> OnColliderStay;
        /// <summary>
        /// 碰撞机退出后调用一次
        /// </summary>
        event Action<UserData> OnColliderExit;
    }
}
