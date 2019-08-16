using System;
using CrazyEngine.Common;


namespace CrazyEngine.Base
{
    /// <summary>
    /// 约束
    /// </summary>
    public class Constraint : ObjBase
    {
        public Body BodyA { get; set; }
        public Body BodyB { get; set; }
        public Point PointA { get; set; }
        public Point PointB { get; set; }
        public double AngleA { get; set; }
        public double AngleB { get; set; }
        public double Length { get; set; }
        public double Stiffness { get; set; } = 0.1d;
        public double AngularStiffness { get; set; } = 0d;

        public Constraint()
        {
            Type = ObjType.Constraint;
        }

        public void Init()
        {
            if (BodyA != null && PointA == null)
                PointA = new Point(0,0);
            if (BodyB != null && PointB == null)
                PointB = new Point(0,0);

            //MonoBehaviour.print(BodyA == null);

            Point initialPointA = BodyA != null
                ? (BodyA.Position + PointA)
                : PointA;
            Point initialPointB = BodyB != null 
                ? (BodyB.Position + PointB)
                : PointB;

            Length = (initialPointA - initialPointB).Magnitude();
            AngleA = BodyA?.Angle ?? AngleA;
            AngleB = BodyB?.Angle ?? AngleB;
        }
    }
}