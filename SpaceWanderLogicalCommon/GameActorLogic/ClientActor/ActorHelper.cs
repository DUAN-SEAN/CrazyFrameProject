using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public static class ActorHelper
    {
        public static bool IsShip(this ActorBase actor)
        {
            switch (actor.GetActorType())
            {
                case ActorTypeBaseDefine.ShipActorNone:
                case ActorTypeBaseDefine.EliteShipActorA:
                case ActorTypeBaseDefine.EliteShipActorB:
                case ActorTypeBaseDefine.FighterShipActorA:
                case ActorTypeBaseDefine.FighterShipActorB:
                case ActorTypeBaseDefine.WaspShipActorA:
                case ActorTypeBaseDefine.AnnihilationShipActor:
                case ActorTypeBaseDefine.DroneShipActor:
                case ActorTypeBaseDefine.PlayerShipActor:
                    return true;
                default: return false;
            }
        }
    }
}
