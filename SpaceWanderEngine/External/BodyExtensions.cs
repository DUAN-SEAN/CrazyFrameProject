using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using Crazy.Common;
using Vector2 = System.Numerics.Vector2;

namespace Box2DSharp.External
{
    public static class BodyExtensions
    {

        /// <summary>
        /// 圆形检测
        /// </summary>
        /// <param name="body"></param>
        /// <param name="bodies"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static List<Body> CircleDetection(this Body body, IEnumerable<Body> bodies, double radius)
        {
            List<tempBody> CollisionalBodies = new List<tempBody>();

            foreach (var b in bodies)
            {
                var distance = Vector2.DistanceSquared(body.GetPosition(), b.GetPosition());
                if (distance < radius * radius)
                {
                    CollisionalBodies.Add(new tempBody(b, Math.Sqrt(distance)));
                }
            }

            return (from o in CollisionalBodies.OrderBy(o => o.Distance) select o.Body).ToList();
        }

        /// <summary>
        /// 追向目标
        /// </summary>
        /// <param name="body"></param>
        /// <param name="targetPoint"></param>
        /// <returns></returns>
        public static float FollowTarget(this Body body, Vector2 targetPoint)
        {
            var cos = FowardToTarget(body, targetPoint);

            //if(cos > 0.8)
            //{
            //    //TODO 加速
            //}

            return 0;
        }

        /// <summary>
        /// 朝向目标
        /// </summary>
        /// <param name="body"></param>
        /// <param name="targetPoint"></param>
        /// <returns></returns>
        public static bool FowardToTarget(this Body body, Vector2 targetPoint)
        {
            if (Vector2.DistanceSquared(body.GetPosition(), targetPoint) < double.Epsilon) return false;

            Vector2 tmp = targetPoint - body.GetPosition();
            bool isClockwise = MathUtils.Cross(tmp, body.GetForward()) > 0;
            float cos = CrazyUtils.IncludedAngleCos(tmp, body.GetForward());

            //body.ApplyTorque

            return cos > 0;
        }

        /// <summary>
        /// 距离检测
        /// </summary>
        /// <param name="body"></param>
        /// <param name="point"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static bool DistanceDetection(this Body body, Vector2 point, double distance)
        {
            return Vector2.DistanceSquared(body.GetPosition(), point) <= distance * distance;
        }

        /// <summary>
        /// 物体的朝向
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static Vector2 GetForward(this Body body)
        {
            return body.GetTransform().Rotation.GetYAxis();
        }

        /// <summary>
        /// 获取力
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static Vector2 GetForce(this Body body)
        {
            return body.Force;
        }

        /// <summary>
        /// 获取转矩
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static float GetTorque(this Body body)
        {
            return body.Torque;
        }

        /// <summary>
        /// 给物体施加力
        /// </summary>
        /// <param name="body"></param>
        /// <param name="force"></param>
        public static void AddForce(this Body body, Vector2 force)
        {
            //Log.Trace("Add" + force);
            body.ApplyForceToCenter(force, true);
        }

        /// <summary>
        /// 给物体施加扭矩
        /// </summary>
        /// <param name="body"></param>
        /// <param name="torque"></param>
        public static void AddTorque(this Body body, float torque)
        {
            body.ApplyTorque(torque, true);
        }

        /// <summary>
        /// 设置速度
        /// </summary>
        /// <param name="body"></param>
        /// <param name="velocity"></param>
        public static void GetVelocity(this Body body, Vector2 velocity)
        {
            body.LinearVelocity = velocity;
        }

        /// <summary>
        /// 设置角速度
        /// </summary>
        /// <param name="body"></param>
        /// <param name="angularVelocity"></param>
        public static void SetAngularVelocity(this Body body, float angularVelocity)
        {
            body.AngularVelocity = angularVelocity;
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
}