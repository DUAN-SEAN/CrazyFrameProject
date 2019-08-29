using CrazyEngine.Base;
using CrazyEngine.Common;
using CrazyEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CrazyEngine.External
{
    public static class BodyExtensions
    {
        /// <summary>
        /// 环形检测
        /// </summary>
        /// <param name="body"></param>
        /// <param name="bodies">被检测的物体组</param>
        /// <param name="radius">检测的半径</param>
        /// <param name="layer">检测的层</param>
        /// <returns>检测到的物体,按照距离进行排序</returns>
        public static List<Body> RingDetection(this Body body ,IEnumerable<Body> bodies , double radius , DectectionLayer layer = DectectionLayer.All)
        {
            List<tempBody> CollisionalBodies = new List<tempBody>();

            foreach(var b in bodies)
            {
                var distanceNoSqrt = Helper.DistanceNoSqrt(body.Position, b.Position);
                if (distanceNoSqrt < radius * radius)
                {
                    CollisionalBodies.Add(new tempBody(b, Math.Sqrt(distanceNoSqrt)));
                }
            }

            return (from o in CollisionalBodies.OrderBy(o => o.Distance) select o.Body).ToList();
        }

        /// <summary>
        /// 向目标地址移动
        /// </summary>
        /// <param name="body"></param>
        /// <param name="targetPoint"></param>
        public static void FollowTarget(this Body body, Point targetPoint)
        {
            if (Helper.DistanceNoSqrt(body.Position, targetPoint) < double.Epsilon) return;

            Point tmp = targetPoint - body.Position;
            bool isClockwise = Helper.Cross(tmp, body.Forward) > 0;
            double cos = Helper.IncludedAngle(tmp, body.Forward);
            if(cos > 0.8) body.Force += body.Mass * body.Forward * 0.000005;
            if (isClockwise)
            {
                body.AngularVelocity = -0.01;
            }
            else
            {
                body.AngularVelocity = 0.01;
            }
        }

        /// <summary>
        /// 触发检测
        /// </summary>
        /// <param name="body"></param>
        /// <param name="trigger"></param>
        /// <param name="distance"></param>
        public static void TriggerDetection(this Body body, Body trigger, double distance, DectectionLayer dectectionLayer = DectectionLayer.All)
        {
            if (!trigger.Trigger) return;
            trigger.Position = body.Position + body.Forward * distance;
            trigger.InitAngle(body.Angle);
        }

        /// <summary>
        /// 将trigger位置摆到body位置前面
        /// </summary>
        /// <param name="body"></param>
        /// <param name="trigger"></param>
        /// <param name="distance"></param>
        /// <param name="dectectionLayer"></param>
        public static void Detection(this Body body, Body trigger, double distance, DectectionLayer dectectionLayer = DectectionLayer.All)
        {
            trigger.Position = body.Position + body.Forward * distance;
            trigger.Angle = body.Angle;
            trigger.InitAngle(body.Angle);
        }

        /// <summary>
        /// 距离检测
        /// </summary>
        /// <param name="body"></param>
        /// <param name="point"></param>
        /// <param name="distance"></param>
        /// <returns>在距离内返回true，否则返回false</returns>
        public static bool DistanceDetection(this Body body, Point point, double distance)
        {
            return Helper.DistanceNoSqrt(body.Position, point) <= distance * distance;
        }

        /// <summary>
        /// 朝向目标
        /// </summary>
        /// <param name="body"></param>
        /// <param name="targetPoint"></param>
        /// <returns></returns>
        public static bool ForwardToTarget(this Body body, Point targetPoint)
        {
            Point tmp = targetPoint - body.Position;
            bool isClockwise = Helper.Cross(tmp, body.Forward) > 0;
            double cos = Helper.IncludedAngle(tmp, body.Forward);

            if (isClockwise)
            {
                body.AngularVelocity = -0.01;
            }
            else
            {
                body.AngularVelocity = 0.01;
            }

            return cos > 0.9;
        }


    }

    public struct tempBody
    {
        public Body Body;
        public double Distance;
        public tempBody(Body body, double distance)
        {
            Body = body;
            Distance = distance;
        }
    }

    public enum DectectionLayer
    {
        Player,
        Enemy,
        All
    }
}
