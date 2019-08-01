using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWanderLogicalCommon
{
    public class ShipBaseActor:IShipBaseContainer
    {






        protected HealthShieldComponent _healthShieldComponent;
        protected MoveComponent _moveComponent;

        public void Move()
        {
            _moveComponent.Move();
        }

        public void GetHP()
        {
            _healthShieldComponent.GetHP();
        }
    }
}
