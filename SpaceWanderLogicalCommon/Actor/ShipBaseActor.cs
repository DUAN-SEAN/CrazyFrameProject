using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWanderLogicalCommon
{


    public class ShipBaseActor:IShipBaseContainer,IShipBase
    {



        public void Move()
        {
            _moveComponent.Move();
        }

        public void GetHP()
        {
            _healthShieldComponent.GetHP();
        }


        IinternalColliderComponent IShipBaseContainer.GetColliderComponent()
        {
            return _colliderComponent;
        }
        IHealthShieldComponent IShipBaseContainer.GetHealthShieldComponent()
        {
            return _healthShieldComponent;
        }
        IInternalEventComponent IShipBaseContainer.GetEventComponent()
        {
            return _eventComponent;
        }
        IInternalMoveComponent IShipBaseContainer.GetMoveComponent()
        {
            return _moveComponent;
        }
        IInternalPhycisXComponent IShipBaseContainer.GetPhycisXComponent()
        {
            return _phycisXComponent;
        }

        public IInternalFireControlComponentBase GetInternalFireControlComponentBase()
        {
            return _fireControlComponent;
        }




        protected HealthShieldComponent _healthShieldComponent;
        protected MoveComponent _moveComponent;
        protected ColliderComponent _colliderComponent;
        protected PhycisXComponent _phycisXComponent;
        protected EventComponent _eventComponent;
        protected FireControlComponent _fireControlComponent;

      
    }
}
