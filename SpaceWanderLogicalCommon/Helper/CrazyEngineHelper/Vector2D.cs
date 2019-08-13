


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Common;

namespace GameActorLogic
{
    public class Vector2D
    {
        public double X { get; set; }

        public double Y { get; set; }

        public Vector2D()
        {
        }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }
        private Vector2D(Vector2D pt)
        {
            X = pt.X;
            Y = pt.Y;
        }

        private Vector2D(Point pt)
        {
            X = pt.X;
            Y = pt.Y;
        }

        public override string ToString()
        {
            return "{" + X + "," + Y + "}";
        }

        public static Vector2D operator +(Vector2D lhs, Vector2D rhs)
        {
            var pt = new Vector2D(lhs);
            pt.X += rhs.X;
            pt.Y += rhs.Y;
            return pt;
        }

        public static Vector2D operator -(Vector2D lhs, Vector2D rhs)
        {
            var pt = new Vector2D(lhs);
            pt.X -= rhs.X;
            pt.Y -= rhs.Y;
            return pt;
        }

        public static Vector2D operator *(Vector2D lhs, double rhs)
        {
            var pt = new Vector2D(lhs);
            pt.X *= rhs;
            pt.Y *= rhs;
            return pt;
        }

        public static Vector2D operator *(double lhs, Vector2D rhs)
        {
            var pt = new Vector2D(rhs);
            pt.X *= lhs;
            pt.Y *= lhs;
            return pt;
        }

        public static Vector2D operator /(Vector2D lhs, double rhs)
        {
            var pt = new Vector2D(lhs);
            pt.X /= rhs;
            pt.Y /= rhs;
            return pt;
        }

        public static Vector2D operator /(Vector2D lhs, Vector2D rhs)
        {
            var pt = new Vector2D(lhs);
            pt.X /= rhs.X;
            pt.Y /= rhs.Y;
            return pt;
        }

        public void Offset(Vector2D pt)
        {
            X += pt.X;
            Y += pt.Y;
        }

        public void Clear()
        {
            X = Y = 0;
        }

        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public void Normalize()
        {
            var magnitude = Math.Sqrt(X * X + Y * Y);
            if (Math.Sign(magnitude) == 0)
            {
                X = Y = 0;
                return;
            }
            X /= magnitude;
            Y /= magnitude;
        }

        public static Vector2D Zero()
        {
            return new Vector2D(0, 0);
        }

        public double Angle(Vertex vertex)
        {
            return Math.Atan2(vertex.Y - Y, vertex.X - X);
        }

        public void RotateAbout(double angle, Vector2D point)
        {
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            var x = point.X + ((X - point.X) * cos - (Y - point.Y) * sin);
            Y = point.Y + ((X - point.X) * sin + (Y - point.Y) * cos);
            X = x;
        }
    }
}
