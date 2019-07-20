using System;

namespace SpaceShip.Model
{

    public class Weapon
    {
        private int weaponType;
        private float speed;
        private float range;
        private float attack;

        public int WeaponType { get => weaponType; set => weaponType = value; }
        public float Speed { get => speed; set => speed = value; }
        public float Range { get => range; set => range = value; }
        public float Attack { get => attack; set => attack = value; }
    }

    public class WeaponType
    {
        public const int Bolt = 1;
        public const int Missile = 2;
        public const int Light = 3;
        public const int Mine = 4;

    }


}

