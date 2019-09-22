using Box2DSharp.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        public static bool IsEnvir(this ActorBase actor)
        {
            switch (actor.GetActorType())
            {
                case ActorTypeBaseDefine.BaseStation:
                case ActorTypeBaseDefine.Meteorite_L:
                case ActorTypeBaseDefine.Meteorite_M:
                case ActorTypeBaseDefine.Meteorite_S:
                case ActorTypeBaseDefine.BlackHole:
                    return true;
                default: return false;
            }
        }
        public static bool IsEnvir(this Int32 actor)
        {
            switch (actor)
            {
                case ActorTypeBaseDefine.BaseStation:
                case ActorTypeBaseDefine.Meteorite_L:
                case ActorTypeBaseDefine.Meteorite_M:
                case ActorTypeBaseDefine.Meteorite_S:
                case ActorTypeBaseDefine.BlackHole:
                    return true;
                default: return false;
            }
        }
        public static bool IsPlayer(this ActorBase actor)
        {
            return actor.GetCamp() == LevelActorBase.PlayerCamp;
        }

       

        public static bool IsBoomWeapon(this Int32 actortype)
        {
            switch (actortype)
            {
                case ActorTypeBaseDefine.TorpedoActor:
                case ActorTypeBaseDefine.TimeBombActor:
                case ActorTypeBaseDefine.TriggerBombActor:
                    return true;
            }
            return false;
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
                //None
                case ActorTypeBaseDefine.ActorNone:
                    return GameModel.ModelNone;
                case ActorTypeBaseDefine.PropNone:
                    return GameModel.ModelNone;
                case ActorTypeBaseDefine.RecoveryNone:
                    return GameModel.ModelNone;
                case ActorTypeBaseDefine.ShipActorNone:
                    return GameModel.ModelNone;
                case ActorTypeBaseDefine.SummonNone:
                    return GameModel.ModelNone;
                case ActorTypeBaseDefine.PlayerShipActor:
                    return GameModel.ModelNone;
                case ActorTypeBaseDefine.GainNone:
                    return GameModel.ModelNone;
                case ActorTypeBaseDefine.WeaponNone:
                    return GameModel.ModelNone;

                //基站
                case ActorTypeBaseDefine.BaseStation:
                    return GameModel.BaseStation;

                //环境
                //陨石
                case ActorTypeBaseDefine.Meteorite_S:
                    return GameModel.S_Meteorolite;
                case ActorTypeBaseDefine.Meteorite_M:
                    return GameModel.M_Meteorolite;
                case ActorTypeBaseDefine.Meteorite_L:
                    return GameModel.L_Meteorolite;
                case ActorTypeBaseDefine.BlackHole:
                    return GameModel.BlackHole;
                //船
                case ActorTypeBaseDefine.AnnihilationShipActor:
                    return GameModel.AnnihilationShip;
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
                case ActorTypeBaseDefine.WaspShipActorA:
                    return GameModel.WaspShip;

                //武器
                case ActorTypeBaseDefine.AntiAircraftGunActor:
                    return GameModel.AntiAircraftGun;
                case ActorTypeBaseDefine.ContinuousLaserActor:
                    return GameModel.ContinuousLaser;
                case ActorTypeBaseDefine.MachineGunActor:
                    return GameModel.MachineGun;
                case ActorTypeBaseDefine.PowerLaserActor:
                    return GameModel.PowerLaser;
                case ActorTypeBaseDefine.TimeBombActor:
                    return GameModel.TimeBomb;
                case ActorTypeBaseDefine.TorpedoActor:
                    return GameModel.Torpedo;
                case ActorTypeBaseDefine.TrackingMissileActor:
                    return GameModel.TrackingMissile;
                case ActorTypeBaseDefine.TriggerBombActor:
                    return GameModel.TriggerBomb;
                
                
            }


            return GameModel.ModelNone;
        }

        /// <summary>
        /// 用船的类型来找到指定的大小
        /// 长宽比例默认为1
        /// </summary>
        public static Vector2 GetLaserShapeByShip(Int32 shiptype,float heightpro = 1,float widthpro = 1)
        {
            switch (shiptype)
            {
                default:
                    return new Vector2(1 * widthpro, 30 * heightpro);
            }
        }
    }
}
