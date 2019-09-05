using Box2DSharp.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 所有碰撞信息监听器
    /// </summary>
    public interface IContactListenerComponentBase : IContactListener
    {

    }


    public interface IContactListenerInternalComponent : IConfigComponentBase
    {

    }
}
