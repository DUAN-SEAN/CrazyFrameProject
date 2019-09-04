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
        public static bool IsShip(this Int32 actor)
        {
            switch (actor)
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

        public static bool IsWeapon(this ActorBase actor)
        {
            switch (actor.GetActorType())
            {
                case ActorTypeBaseDefine.AntiAircraftGunActor:
                case ActorTypeBaseDefine.ContinuousLaserActor:
                case ActorTypeBaseDefine.MachineGunActor:
                case ActorTypeBaseDefine.PowerLaserActor:
                case ActorTypeBaseDefine.TimeBombActor:
                case ActorTypeBaseDefine.TorpedoActor:
                case ActorTypeBaseDefine.TrackingMissileActor:
                case ActorTypeBaseDefine.TriggerBombActor:
                    return true;
                default: return false;
            }
        }

        public static bool IsWeapon(this Int32 actor)
        {
            switch (actor)
            {
                case ActorTypeBaseDefine.AntiAircraftGunActor:
                case ActorTypeBaseDefine.ContinuousLaserActor:
                case ActorTypeBaseDefine.MachineGunActor:
                case ActorTypeBaseDefine.PowerLaserActor:
                case ActorTypeBaseDefine.TimeBombActor:
                case ActorTypeBaseDefine.TorpedoActor:
                case ActorTypeBaseDefine.TrackingMissileActor:
                case ActorTypeBaseDefine.TriggerBombActor:
                    return true;
                default: return false;
            }
        }
        public static bool IsPlayer(this ActorBase actor)
        {
            return actor.GetCamp() == LevelActorBase.PlayerCamp;
        }

        public static bool isActorTypeNumber(this Int32 num)
        {
            switch (num)
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
                case ActorTypeBaseDefine.AntiAircraftGunActor:
                case ActorTypeBaseDefine.ContinuousLaserActor:
                case ActorTypeBaseDefine.MachineGunActor:
                case ActorTypeBaseDefine.PowerLaserActor:
                case ActorTypeBaseDefine.TimeBombActor:
                case ActorTypeBaseDefine.TorpedoActor:
                case ActorTypeBaseDefine.TrackingMissileActor:
                case ActorTypeBaseDefine.TriggerBombActor:
                    return true;
            }
            return false;

        }
    }
}
