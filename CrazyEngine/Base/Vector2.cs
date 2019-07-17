using System;
using System.Xml.Serialization;

namespace CrazyEngine
{
    [Serializable]
    public struct Vector2
    {

        public static Vector2 Zero = new Vector2(0, 0);
        public static Vector2 Up = new Vector2(1, 0);
        public static Vector2 Down = new Vector2(-1, 0);

        private float x1;
        private float y1;

        public Vector2(float _x, float _y)
        {
            x1 = (float)Math.Round(_x, 6);
            y1 = (float)Math.Round(_y, 6);
        }
        public Vector2(Vector2 vector)
        {
            x1 = vector.x1;
            y1 = vector.y1;
        }
        public float x { get => x1; private set => x1 = value; }
        public float y { get => y1; private set => y1 = value; }

        public Vector2 normalized  //方向
        {
            get
            {
                if (Math.Abs(magnitude) < float.Epsilon) return new Vector2();
                return new Vector2(x / magnitude, y / magnitude);
            }
        }


        public float magnitude  //大小
        {
            get
            {
                return (float)Math.Sqrt(x * x + y * y);
            }
        }
        public float magnitudeNoSqrt  //大小
        {
            get
            {
                return (float)(x * x + y * y);
            }
        }
        public void Set(float newX, float newY)
        {
            x = (float)Math.Round(newX, 6);
            y = (float)Math.Round(newY, 6);
        }

        public float Sin
        {
            get
            {
                return y / magnitude;
            
            }
        }
        public float Cos
        {
            get
            {
            
                return x / magnitude;
            }
        }

        public float SinNoSqrt
        {
            get
            {
                return y * y / magnitudeNoSqrt;

            }
        }
        public float CosNoSqrt
        {
            get
            {

                return x * x / magnitudeNoSqrt;
            }
        }
        /// <summary>
        /// 旋转（绕本体）
        /// </summary>
        public Vector2 Rotate(int degree)
        {
            float angle = (float)(degree / 360f * Math.PI * 2);
            float newX = (float)(x * Math.Cos(angle) - y * Math.Sin(angle));
            float newY = (float)(x * Math.Sin(angle) + y * Math.Cos(angle));
            return new Vector2(newX, newY);
        }
        /// <summary>
        /// 旋转（绕点）
        /// </summary>
        /// <param name="v"></param>
        /// <param name="degree"></param>
        /// <returns></returns>
        public Vector2 Rotate(Vector2 v, int degree)
        {
            float newX = (float)((x - v.x) * Math.Cos(degree * Math.PI / 180) - (y - v.y) * Math.Sin(degree * Math.PI / 180) + v.x);
            float newY = (float)((y - v.y) * Math.Cos(degree * Math.PI / 180) + (x - v.x) * Math.Sin(degree * Math.PI / 180) + v.y);
            return new Vector2(newX, newY);
        }

        public static float Distance(Vector2 v1, Vector2 v2)
        {
            return (float)Math.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y));
        }

        public static float DistanceNoSqrt(Vector2 v1, Vector2 v2)
        {
            return (float)(v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y);
        }

        /// <summary>
        /// float 插值运算
        /// </summary>
        /// <returns>The lerp.</returns>
        /// <param name="a">The alpha component.</param>
        /// <param name="b">The blue component.</param>
        /// <param name="t">T.</param>
        public static float Lerp(float a, float b, float t)
        {
            return Math.Abs(a - b) > 0.0001 ? a > b ? a -= t : a += t : b;
        }

        /// <summary>
        /// Vector 差值运算
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector2 Lerp(Vector2 v1, Vector2 v2, float t)
        {
            //return new Vector2(Lerp(v1.x, v2.x, t), Lerp(v1.y, v2.y, Math.Abs(v2.y - v1.y) * t / Math.Abs(v2.x - v1.x)));
            return new Vector2(Lerp(v1.x, v2.x, t), Lerp(v1.y, v2.y, t));
        }

        /// <summary>
        /// 向量叉乘
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        public static float Mul(Vector2 v1, Vector2 v2, Vector2 v3)
        {
            return (v2.x - v1.x) * (v3.y - v1.y) - (v3.x - v1.x) * (v2.y - v1.y);
        }

        /// <summary>
        /// 方法重写和操作符重载
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Vector2"/>.</returns>
        public override string ToString()
        {
            return "(" + x + "," + y + ")";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2)) return false;
            return new Vector2(x, y) == (Vector2)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Vector2 vector1, Vector2 Vector2)
        {
            return Math.Abs(vector1.x - Vector2.x) < float.Epsilon && Math.Abs(vector1.y - Vector2.y) < float.Epsilon;
        }

        public static bool operator !=(Vector2 vector1, Vector2 Vector2)
        {
            return !(vector1 == Vector2);
        }

        public static Vector2 operator +(Vector2 vector1, Vector2 Vector2)
        {
            return new Vector2(vector1.x + Vector2.x, vector1.y + Vector2.y);
        }

        public static Vector2 operator -(Vector2 vector1, Vector2 Vector2)
        {
            return new Vector2(vector1.x - Vector2.x, vector1.y - Vector2.y);
        }

        public static Vector2 operator *(Vector2 vector, float num)
        {
            return new Vector2(vector.x * num, vector.y * num);
        }

        public static Vector2 operator *(float num, Vector2 vector)
        {
            return vector * num;
        }

        public static float operator *(Vector2 vector1, Vector2 vector2)
        {
            return vector1.x * vector2.y + vector2.x * vector1.y;
        }

        public static Vector2 operator /(Vector2 vector, float num)
        {
            if (Math.Abs(num) < float.Epsilon) return new Vector2();
            return new Vector2(vector.x / num, vector.y / num);
        }
        public static Vector2 operator -(Vector2 vector)
        {
            return new Vector2(-vector.x, -vector.y);
        }

    }
}