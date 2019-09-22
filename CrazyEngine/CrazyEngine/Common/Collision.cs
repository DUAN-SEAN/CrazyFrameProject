using CrazyEngine.Base;

namespace CrazyEngine.Common
{
    /// <summary>
    /// 记录碰撞信息
    /// </summary>
    public class Collision
    {
        public Body BodyA { get; set; }
        public Body BodyB { get; set; }
        public Body ParentA { get; set; }
        public Body ParentB { get; set; }
        public bool Collided { get; set; }
        public double Depth { get; set; }
        public Body AxisBody { get; set; }
        public int AxisNumber { get; set; }
        public bool Reused { get; set; }
        /// <summary>
        /// 法线
        /// </summary>
        public Point Normal { get; set; }
        /// <summary>
        /// 切线
        /// </summary>
        public Point Tangent { get; set; }
        public Point Penetration { get; set; }
        public Vertices Supports { get; set; }
    }
}