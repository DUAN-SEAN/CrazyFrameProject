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

        private float Hp;
        private float Mp;
        private string Name;
        private List<Weapon> Weapon;
        private int AircraftType;
        private float speed;
        private float acceleratedSpeed;
        private float Maxspeed;
        private float MaxacceleratedSpeed;

    }

}


