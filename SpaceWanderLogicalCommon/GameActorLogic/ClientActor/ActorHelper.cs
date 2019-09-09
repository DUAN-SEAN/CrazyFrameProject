using Box2DSharp.External;
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

        public static float GetBodyMassByActor(this ActorBase actor)
        {
            return BodyExtensions.GetSpaceWanderMass(actor.GetGameModelByActorType());
        }


        /// <summary>
        /// 将Actor类型转换成游戏模型类型
        /// </summary>
        public static GameModel GetGameModelByActorType(this IBaseContainer actor)
        {

            switch (actor.GetActorType())
            {
                case ActorTypeBaseDefine.ActorNone:
                    return GameModel.ModelNone;
                case ActorTypeBaseDefine.AnnihilationShipActor:
                    return GameModel.AnnihilationShip;
                case ActorTypeBaseDefine.AntiAircraftGunActor:
                    return GameModel.AntiAircraftGun;
                case ActorTypeBaseDefine.BaseStation:
                    return GameModel.BaseStation;
                case ActorTypeBaseDefine.ContinuousLaserActor:
                    return GameModel.ContinuousLaser;
                case ActorTypeBaseDefine.DroneShipActor:
                    return GameModel.DroneShip;
                case ActorTypeBaseDefine.EliteShipActorA:
                    return GameModel.EliteShipA;
                case ActorTypeBaseDefine.EliteShipActorB:
                    return GameModel.EliteShipB;
                case ActorTypeBaseDefine.FighterShipActorA:
                    return GameModel.FighterShipA;
                case ActorTypeBaseDefine.FighterShipActorB:
                    return GameModel.FighterShipB;
                case ActorTypeBaseDefine.GainNone:
                    return GameModel.ModelNone;
                case ActorTypeBaseDefine.MachineGunActor:
                    return GameModel.MachineGun;
                case ActorTypeBaseDefine.PlayerShipActor:
                    return GameModel.ModelNone;
                case ActorTypeBaseDefine.PowerLaserActor:
                    return GameModel.PowerLaser;
                case ActorTypeBaseDefine.PropNone:
                    return GameModel.ModelNone;
                case ActorTypeBaseDefine.RecoveryNone:
                    return GameModel.ModelNone;
                case ActorTypeBaseDefine.ShipActorNone:
                    return GameModel.ModelNone;
                case ActorTypeBaseDefine.SummonNone:
                    return GameModel.ModelNone;
                case ActorTypeBaseDefine.TimeBombActor:
                    return GameModel.TimeBomb;
                case ActorTypeBaseDefine.TorpedoActor:
                    return GameModel.Torpedo;
                case ActorTypeBaseDefine.TrackingMissileActor:
                    return GameModel.TrackingMissile;
                case ActorTypeBaseDefine.TriggerBombActor:
                    return GameModel.TriggerBomb;
                case ActorTypeBaseDefine.WaspShipActorA:
                    return GameModel.WaspShip;
                case ActorTypeBaseDefine.WeaponNone:
                    return GameModel.ModelNone;
            }


            return GameModel.ModelNone;
        }
    }
}
