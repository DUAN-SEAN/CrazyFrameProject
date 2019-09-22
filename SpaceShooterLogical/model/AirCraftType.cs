using System;
using System.Collections.Generic;

namespace SpaceShip.Model
{
    public class AirCraftType
    {

        public const int Bomber = 1;
        public const int Colossal = 2;
        public const int Cruiser = 3;
        public const int Fighter = 4;
        public const int FightrtHeavy = 5;
        public const int GalacticCarrier = 6;
        public const int Transport = 7;
        public const int Stealth = 8;
        public const int DroneAttach = 9;
        public const int DroneRepair = 10;

    }
    public class AirCraftInfo
    {

#pragma warning disable CS0169 // 从不使用字段“AirCraftInfo.Hp”
        private float Hp;
#pragma warning restore CS0169 // 从不使用字段“AirCraftInfo.Hp”
#pragma warning disable CS0169 // 从不使用字段“AirCraftInfo.Mp”
        private float Mp;
#pragma warning restore CS0169 // 从不使用字段“AirCraftInfo.Mp”
#pragma warning disable CS0169 // 从不使用字段“AirCraftInfo.Name”
        private string Name;
#pragma warning restore CS0169 // 从不使用字段“AirCraftInfo.Name”
#pragma warning disable CS0169 // 从不使用字段“AirCraftInfo.Weapon”
        private List<Weapon> Weapon;
#pragma warning restore CS0169 // 从不使用字段“AirCraftInfo.Weapon”
#pragma warning disable CS0169 // 从不使用字段“AirCraftInfo.AircraftType”
        private int AircraftType;
#pragma warning restore CS0169 // 从不使用字段“AirCraftInfo.AircraftType”
#pragma warning disable CS0169 // 从不使用字段“AirCraftInfo.speed”
        private float speed;
#pragma warning restore CS0169 // 从不使用字段“AirCraftInfo.speed”
#pragma warning disable CS0169 // 从不使用字段“AirCraftInfo.acceleratedSpeed”
        private float acceleratedSpeed;
#pragma warning restore CS0169 // 从不使用字段“AirCraftInfo.acceleratedSpeed”
#pragma warning disable CS0169 // 从不使用字段“AirCraftInfo.Maxspeed”
        private float Maxspeed;
#pragma warning restore CS0169 // 从不使用字段“AirCraftInfo.Maxspeed”
#pragma warning disable CS0169 // 从不使用字段“AirCraftInfo.MaxacceleratedSpeed”
        private float MaxacceleratedSpeed;
#pragma warning restore CS0169 // 从不使用字段“AirCraftInfo.MaxacceleratedSpeed”

    }

}


