using System;
using CrazyEngine;
namespace CrazyEngine
{
    public class PointEntity : Body
    {
        public PointEntity()
        {
            Collider = new Collider(Vector2.Zero);
            Position = Vector2.Zero;
        }
        public PointEntity(Vector2 vector)
        {
            Collider = new Collider(vector);
            Position = vector;
        }

        public bool DoSomething()
        {
            return true;
        }



    }
}