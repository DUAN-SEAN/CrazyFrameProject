using System.Collections.Generic;
using System.Linq;
using CrazyEngine.Common;

namespace CrazyEngine.Base
{
    public static class Factory
    {
        /// <summary>
        /// 矩形
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="isStatic"></param>
        /// <returns></returns>
        public static Body CreateRectangleBody(double x, double y, double width, double height, bool isStatic = false,bool isTrigger = false)
        {
            var path = new List<Point> { new Point(0, 0), new Point(width, 0), new Point(width, height), new Point(0,height) };
            var body = new Body {Static = isStatic, Trigger = isTrigger};
            body.Init(path);
            body.Position = new Point(x, y);
            return body;
        }

        /// <summary>
        /// 三角形
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="d">底</param>
        /// <param name="h">高</param>
        /// <param name="isStatic"></param>
        /// <returns></returns>
        public static Body CreateTriggleBody(double x, double y, double d, double h, bool isStatic = false)
        {
            var path = new List<Point> { new Point(0, 0), new Point(d, 0), new Point(d / 2, h) };
            var body = new Body { Static = isStatic };
            body.Init(path);
            body.Position = new Point(x, y);
            return body;
        }

        /// <summary>
        /// 菱形
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width">低</param>
        /// <param name="height">高</param>
        /// <param name="isStatic"></param>
        /// <returns></returns>
        public static Body CreateRhombusBody(double x, double y,double width, double height, bool isStatic = false)
        {
            var path = new List<Point> { new Point(0, 0), new Point(width/2, height/2), new Point(width, 0), new Point(width/2, -height/2) };
            var body = new Body { Static = isStatic };
            body.Init(path);
            body.Position = new Point(x, y);
            return body;
        }

        /// <summary>
        /// 圆（其实是八边形）
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius">半径</param>
        /// <param name="isStatic"></param>
        /// <returns></returns>
        public static Body CreateCircleBody(double x, double y, double radius, bool isStatic = false)
        {
            var gr = 0.707107 * radius;//二分之根号2
            var g3r = 0.8660254 * radius; // 二分之根号三

            bool accuracy = radius < 15;
            List<Point> path;

            if (accuracy)
            {
                path = new List<Point>
                {
                    new Point(-radius, 0), new Point(-gr, gr), new Point(0, radius), new Point(gr, gr),
                    new Point(radius, 0), new Point(gr, -gr), new Point(0, -radius), new Point(-gr, -gr)
                };
            }
            else
            {
                path = new List<Point>
                {
                    new Point(-radius,0), new Point(-g3r, radius/2), new Point(-radius/2, g3r), new Point(0,radius),
                    new Point(radius/2, g3r), new Point(g3r, radius/2), new Point(radius, 0), new Point(g3r, -radius/2),
                    new Point(radius/2, -g3r), new Point(0,-radius), new Point(-radius/2, -g3r), new Point(-g3r, -radius/2)
                };
            }

            var body = new Body { Static = isStatic };
            body.Init(path);
            body.Position = new Point(x, y);
            return body;
        }

        /// <summary>
        /// 梯形
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="height">高</param>
        /// <param name="top">上底</param>
        /// <param name="bottom">下底</param>
        /// <param name="isStatic"></param>
        /// <returns></returns>
        public static Body CreateTrapezoidBody(double x, double y, double height, double top, double bottom, bool isStatic = false)
        {
            var path = new List<Point> {
                new Point(-top/2, height/2), new Point(top/2, height/2), new Point(bottom/2, -height/2), new Point(-bottom/2, -height/2)
            };
            var body = new Body { Static = isStatic };
            body.Init(path);
            body.Position = new Point(x, y);
            return body;
        }

        /// <summary>
        /// 克隆物体
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static Body CreateCloneBody(Body body)
        {
            var cloneBody = new Body()
            {
                Type = body.Type,
                Angle = body.Angle,
                Velocity = body.Velocity,
                Force = body.Force,
                AngularVelocity = body.AngularVelocity,
                Static = body.Static,
                Trigger = body.Trigger
            };

            cloneBody.Init(body.Vertices.ToPoints().ToList());
            cloneBody.InitAngle(body.Angle);
            cloneBody.Position = body.Position;

            return cloneBody;
        }

    }
}
