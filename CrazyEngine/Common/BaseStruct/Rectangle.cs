using System;

namespace CrazyEngine
{
    public struct Rectangle : IBaseStruct
    {
        /// <summary>
        /// 矩形小值点
        /// </summary>
        /// <value>The minimum.</value>
        public Vector2 Min { set; get; }
        /// <summary>
        /// 矩形大值点
        /// </summary>
        /// <value>The max.</value>
        public Vector2 Max { set; get; }
        /// <summary>
        /// 矩形的宽
        /// </summary>
        /// <value>The length.</value>
        public float Width { set; get; }
        /// <summary>
        /// 矩形的高
        /// </summary>
        /// <value>The width.</value>
        public float Height { set; get; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public Rectangle(Vector2 min, Vector2 max)
        {
            Min = new Vector2(Math.Min(min.x, max.x), Math.Min(min.y, max.y));
            Max = new Vector2(Math.Max(min.x, max.x), Math.Max(min.y, max.y));

            Width = Max.x - Min.x;
            Height = Max.y - Min.y;
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        public Rectangle(float minX, float minY, float maxX, float maxY)
        {
            Vector2 min = new Vector2(minX, minY);
            Vector2 max = new Vector2(maxX, maxY);

            Min = new Vector2(Math.Min(min.x, max.x), Math.Min(min.y, max.y));
            Max = new Vector2(Math.Max(min.x, max.x), Math.Max(min.y, max.y));

            Width = Max.x - Min.x;
            Height = Max.y - Min.y;
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2((Max.x + Min.x) / 2, (Max.y + Min.y) / 2);
            }
            set
            {
                Max = value + new Vector2(Width / 2, Height / 2);
                Min = value - new Vector2(Width / 2, Height / 2);
            }
        }
    }
}