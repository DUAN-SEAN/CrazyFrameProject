using System;

namespace CrazyEngine
{
    public struct Circle : IBaseStruct
    {

        private Vector2 _center;

        /// <summary>
        /// 圆的中心
        /// </summary>
        /// <value>The center.</value>
        public Vector2 Center
        {
            get { return _center; }
            set { _center = value; }
        }

        /// <summary>
        /// 圆的半径
        /// </summary>
        /// <value>The radius.</value>
        public float Radius { get; set; }

        public Circle(Vector2 center, float radius)
        {
            _center = center;
            Radius = radius < 0 ? 0 : radius;
        }

        public Circle(float centerX, float centerY, float radius)
        {
            _center = new Vector2(centerX, centerY);
            Radius = radius < 0 ? 0 : radius;
        }

    }
}