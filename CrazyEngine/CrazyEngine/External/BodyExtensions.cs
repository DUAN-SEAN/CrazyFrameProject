using CrazyEngine.Base;
using CrazyEngine.Common;
using CrazyEngine.Core;
using System;
using System.Collections.Generic;


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
        /// <returns>检测到的物体</returns>
        public static List<Body> RingDetection(this Body body ,IEnumerable<Body> bodies , double radius , DectectionLayer layer = DectectionLayer.All)
        {
            List<Body> CollisionalBodies = new List<Body>();

            foreach(var b in bodies)
            {
                if(Helper.DistanceNoSqrt(body.Position, b.Position) < radius * radius)
                {
                    CollisionalBodies.Add(b);
                }
            }

            return CollisionalBodies;
        }

        /// <summary>
        /// 向目标地址移动
        /// </summary>
        /// <param name="body"></param>
        /// <param name="targetPoint"></param>
        public static void FollowTarget(this Body body, Point targetPoint)
        {
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

        public static void OnCollisionStay(this Body body, Common.Collision collision)
        {

        }

        /// <summary>
        /// 触发检测
        /// </summary>
        /// <param name="body"></param>
        /// <param name="trigger"></param>
        /// <param name="distance"></param>
        public static void TriggerDetection(this Body body, Body trigger, double distance,
            DectectionLayer dectectionLayer = DectectionLayer.All)
        {
            if (!trigger.Trigger) return;
            trigger.Position = body.Position + body.Forward * distance;
            trigger.InitAngle(body.Angle);
        }


    }

    public enum DectectionLayer
    {
        Player,
        Enemy,
        All
    }
}
