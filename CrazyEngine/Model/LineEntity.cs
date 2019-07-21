using System;
using CrazyEngine;

namespace CrazyEngine
{
    [Serializable]

    public class LineEntity : Body
    {
        public float Length;
        public Vector2 StartPoint;
        public LineEntity()
        {
            Collider = new Collider(Vector2.Zero);
            Position = Vector2.Zero;
        }
        public LineEntity(Vector2 start, Vector2 end)
        {
            Collider = new Collider(new Line(start, end));
            Position = (start + end) / 2;

        }
        public bool DoSomething()
        {
            return true;
        }

        

    }
}