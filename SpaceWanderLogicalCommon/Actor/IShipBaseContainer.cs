using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWanderLogicalCommon
{
    public interface IShipBaseContainer
    {
        IinternalColliderComponent GetColliderComponent();
        IHealthShieldComponent GetHealthShieldComponent();
        IInternalEventComponent GetEventComponent();
        IInternalMoveComponent GetMoveComponent();
        IInternalPhycisXComponent GetPhycisXComponent();

        IInternalFireControlComponentBase GetInternalFireControlComponentBase();

    }
    public interface IShipBase:
        IColliderComponent,
        IEventComponent,
        IHealthShieldComponent,
        IMoveComponent,
        IPhycisXComponent,
        IFireControlComponentBase
    {
    

    }


}
