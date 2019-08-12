using System;
using System.Collections.Generic;
using CrazyEngine.Common;
using CrazyEngine.Base;
using System.Linq;

namespace CrazyEngine.Core
{
    static class Helper
    {
        /// <summary>
        /// 获取一个边框Bound的ID
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        public static string GetId(this Bound bound)
        {
            return bound.Min.ToString() + bound.Max.ToString();
        }

        /// <summary>
        /// 合并两个边框(返回一个更大的边框)
        /// </summary>
        /// <param name="bound"></param>
        /// <param name="bound2"></param>
        /// <returns></returns>
        public static Bound Union(this Bound bound, Bound bound2)
        {
            return new Bound()
            {
                Min = new Point(Math.Min(bound.Min.X, bound2.Min.X),
                    Math.Min(bound.Min.Y, bound2.Min.Y)),
                Max = new Point(Math.Max(bound.Max.X, bound2.Max.X),
                    Math.Max(bound.Max.Y, bound2.Max.Y))
            };
        }

        /// <summary>
        /// 判断坐标(x,y)是否在边框内
        /// </summary>
        /// <param name="bound"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool Contains(this Bound bound, double x, double y)
        {
            return (x >= bound.Min.X && x <= bound.Max.X
                    && y >= bound.Min.Y && y <= bound.Max.Y);
        }

        /// <summary>
        /// 判断两个边框Bound是否交叉
        /// </summary>
        /// <param name="bound"></param>
        /// <param name="bound2"></param>
        /// <returns></returns>
        public static bool Intersect(this Bound bound, Bound bound2)
        {
            return (bound.Min.X <= bound2.Max.X && bound.Max.X >= bound2.Min.X
                    && bound.Max.Y >= bound2.Min.Y && bound.Min.Y <= bound2.Max.Y);
        }

        /// <summary>
        /// 获得对Pair的ID
        /// </summary>
        /// <param name="bodyA"></param>
        /// <param name="bodyB"></param>
        /// <returns></returns>
        public static string GetPairId(Body bodyA, Body bodyB)
        {
            return bodyA.Id < bodyB.Id
                ? bodyA.Id + "_" + bodyB.Id
                : bodyB.Id + "_" + bodyA.Id;
        }

        /// <summary>
        /// (这个我不太懂怎么解释)
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static IEnumerable<Body> EnumParts(this Body body)
        {
            for (var i = body.Parts.Count > 1 ? 1 : 0; i < body.Parts.Count; i++)
            {
                yield return body.Parts[i];
            }
        }

        /// <summary>
        /// 返回两个Vertex的点乘值
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static double Dot(this Vertex vertex, Point point)
        {
            return (vertex.X * point.X) + (vertex.Y * point.Y);
        }

        /// <summary>
        /// 返回两个Point的点乘值
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static double Dot(this Point lhs, Point rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y;
        }

        /// <summary>
        /// 使传入的Point的x，y变成-x，-y
        /// </summary>
        /// <param name="lhs"></param>
        public static void Negate(this Point lhs)
        {
            lhs.X = -lhs.X;
            lhs.Y = -lhs.Y;
        }

        /// <summary>
        /// 返回一个Point(-x,-y)
        /// </summary>
        /// <param name="lhs"></param>
        /// <returns></returns>
        public static Point Negative(this Point lhs)
        {
            return new Point(-lhs.X, -lhs.Y);
        }

        /// <summary>
        /// 返回坐标Point(-y,x)
        /// </summary>
        /// <param name="lhs"></param>
        /// <returns></returns>
        public static Point Perpendicular(this Point lhs)
        {
            return new Point(-lhs.Y, lhs.X);
        }

        /// <summary>
        /// Vertices是否包含一个vertex
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public static bool Contains(this Vertices vertices, Vertex vertex)
        {
            for (var i = 0; i < vertices.Vertexes.Count; i++)
            {
                var j = (i == vertices.Vertexes.Count - 1) ? 0 : (i + 1);
                var vertice = vertices.Vertexes[i];
                var nextVertice = vertices.Vertexes[j];
                if ((vertex.X - vertice.X) * (nextVertice.Y - vertice.Y) + (vertex.Y - vertice.Y) * (vertice.X - nextVertice.X) > 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获得单个顶点Vertex的ID
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public static string GetId(this Vertex vertex)
        {
            return vertex.Body.Id + "_" + vertex.Index;
        }

        /// <summary>
        /// 返回两个Point叉乘后的值
        /// </summary>
        /// <param name="point"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double Cross(this Point point, Point point2)
        {
            return (point.X * point2.Y) - (point.Y * point2.X);
        }

        /// <summary>
        /// 顶点Vertex坐标加上Point
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point Add(this Vertex vertex, Point point)
        {
            return new Point(vertex.X + point.X, vertex.Y + point.Y);
        }

        /// <summary>
        /// 顶点Vertex坐标减去Point
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point Sub(this Vertex vertex, Point point)
        {
            return new Point(vertex.X - point.X, vertex.Y - point.Y);
        }

        /// <summary>
        /// 返回Number在min和max之间的一个值
        /// </summary>
        /// <param name="number"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double Clamp(double number, double min, double max)
        {
            return number < min ? min : (number > max ? max : number);
        }

        /// <summary>
        /// point旋转angle弧度后的坐标位置
        /// </summary>
        /// <param name="point"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Point Rotate(this Point point, double angle)
        {
            if (Math.Sign(angle) == 0)
                return new Point(point.X, point.Y);

            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            return new Point(point.X * cos - point.Y * sin,
                point.X * sin + point.Y * cos);
        }

        /// <summary>
        /// 判断point是否为(0,0)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsZero(this Point point)
        {
            return Math.Sign(point.X) == 0 && Math.Sign(point.Y) == 0;
        }

        /// <summary>
        /// 将Bound转化为Point数组
        /// </summary>
        /// <param name="bound"></param>
        /// <returns></returns>
        public static Point[] ToPoints(this Bound bound)
        {
            return new List<double>()
            {
                bound.Min.X,
                bound.Max.X,
                bound.Max.X,
                bound.Min.X
            }.Zip(new List<double>()
            {
                bound.Min.Y,
                bound.Min.Y,
                bound.Max.Y,
                bound.Max.Y
            }, (a, b) => new Point(a, b)).ToArray();
        }

    }
}