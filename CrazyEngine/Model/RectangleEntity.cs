using System;
using CrazyEngine;

namespace CrazyEngine
{
    [Serializable]
    public class RectangleEntity : Body
    {
        public RectangleEntity()
        {
            Position = Vector2.Zero;
            Collider = new Collider(Vector2.Zero, Vector2.Zero);
        }
        public RectangleEntity(Vector2 min, Vector2 max)
        {
            Max_posi = max;
            Min_posi = min;
            Position = new Vector2((max.x + min.x) / 2, (max.y + min.y) / 2);
            Collider = new Collider(min, max);
        }

        public bool DoSomething()
        {
            return true;
        }

        


        public Vector2 Max_posi;
        public Vector2 Min_posi;
    }
}