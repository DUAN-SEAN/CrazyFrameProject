﻿using Crazy.Common;
using CrazyEngine.Base;
using CrazyEngine.Common;
using CrazyEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;


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
            if(cos > 0.8) body.Force += body.Mass * body.Forward * 0.00005;
            if (isClockwise)
            {
                body.AngularVelocity = -0.1;
            }
            else
            {
                body.AngularVelocity = 0.1;
            }
        }

        /// <summary>
        /// 设置方向
        /// </summary>
        /// <param name="body"></param>
        /// <param name="angle"></param>
        public static void SetForward(this Body body, double angle)
        {
            //body.InitAngle(angle);
            //body.InitAngle(angle);
            body.Angle = angle;
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
            if(trigger == null) return;
            
            trigger.Position.Set(body.Position);
            trigger.InitAngle(body.Angle);
            trigger.Velocity = body.Velocity + body.Forward * 10;
            trigger.AngularVelocity = 0;


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

                //Log.Trace("BodyExtensions ForwardToTarget: isClockwise:" + isClockwise);
            if (isClockwise)
            {
                body.AngularVelocity = -0.1;
                //Log.Trace("BodyExtensions ForwardToTarget: body.AugularVelocity:" + body.AngularVelocity);
            }
            else
            {
                body.AngularVelocity = 0.1;
                //Log.Trace("BodyExtensions ForwardToTarget: body.AugularVelocity:" + body.AngularVelocity);
            }
            //Log.Trace("BodyExtensions ForwardToTarget: cos:" + cos);
            return cos > 0.9;
        }

        /// <summary>
        /// 段瑞要的1
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="frame"></param>
        /// <param name="frictionAir"></param>
        /// <returns></returns>
        public static Point PredictPosition(Point position, Point velocity, Point force, double mass, double time, double frictionAir = 0.005)
        {
            Point velocityPrev = new Point(velocity.X, velocity.Y);

            velocity.X = velocityPrev.X * (1 - frictionAir) + force.X / mass  * time;
            velocity.Y = velocityPrev.Y * (1 - frictionAir) + force.Y / mass  * time;

            position.X += velocity.X * time / 2;
            position.Y += velocity.Y * time / 2;

            return position;
        }

        /// <summary>
        /// 段瑞要的2
        /// </summary>
        /// <param name="anlge"></param>
        /// <param name="angleVelocity"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static Point PredictAngle(double angle, double angleVelocity, double time, double delta)
        {
            angle += angleVelocity * time;

            return new Point(-Math.Sin(angle), Math.Cos(angle));
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
