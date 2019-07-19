using CrazyEngine;

namespace SpaceShip.Base
{
    public struct Command
    {
        public int ID;
        public Vector2 Force;
        public Vector2 Forward;
        public AttackCommand attackCommand;
        public UnAttackCommand unattackCommand;
    }


    public struct AttackCommand
    {
        public int type;
        public bool attackState;

    }

    public struct UnAttackCommand
    {
        public int type;
        public bool unattackState;
    }
}
