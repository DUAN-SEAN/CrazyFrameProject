using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    CollisionalBodies.Add(new tempBody(b, (float)Math.Sqrt(distance)));
                }
            }

            return (from o in CollisionalBodies.OrderBy(o => o.Distance) select o.Body).ToList();
        }

        /// <summary>
        /// 爆炸冲量
        /// </summary>
        /// <param name="body"></param>
        /// <param name="boomPos"></param>
        /// <param name="forceProc"></param>
        public static void Boom(this Body body, Vector2 boomPos, float forceProc)
        {
            Vector2 tmp = body.GetPosition() - boomPos;
            body.ApplyLinearImpulseToCenter(tmp * forceProc, true);
        }

        /// <summary>
        /// 黑洞吸力
        /// </summary>
        /// <param name="body"></param>
        /// <param name="attractPos"></param>
        /// <param name="proc"></param>
        public static void Attract(this Body body, Vector2 attractPos, float forceProc,float radius)
        {
            Vector2 tmp = body.GetPosition() - attractPos;
            float distance = radius - tmp.Length();
            body.ApplyLinearImpulseToCenter(-tmp * distance * distance* forceProc, true);
        }

        /// <summary>
        /// 追向目标
        /// </summary>
        /// <param name="body"></param>
        /// <param name="targetPoint"></param>
        /// <returns></returns>
        public static bool FollowTarget(this Body body, Vector2 targetPoint, float force, float angularVelocityProc)
        {
            var cos = FowardToTarget(body, targetPoint, angularVelocityProc);

            if (cos > 0.9f)
            {
                body.AddForce(force * body.GetForward());
            }

            return cos > 0.9f;
        }

        /// <summary>
        /// 朝向目标
        /// </summary>
        /// <param name="body"></param>
        /// <param name="targetPoint"></param>
        /// <param name="angularVelocityProc">角速度系数(0.1 - 1)</param>
        /// <returns></returns>
        public static float FowardToTarget(this Body body, Vector2 targetPoint, float angularVelocityProc, float deadArea = 0.95f)
        {
            if (Vector2.DistanceSquared(body.GetPosition(), targetPoint) < double.Epsilon) return 0;

            Vector2 tmp = targetPoint - body.GetPosition();
            bool isClockwise = MathUtils.Cross(tmp, body.GetForward()) > 0;
            float cos = CrazyUtils.IncludedAngleCos(tmp, body.GetForward());

            body.ApplyAngularImpulse(-body.AngularVelocity * body.Inertia, true);

            if (cos > deadArea)
            {
                return cos;
            }

            if (isClockwise)
            {
                body.ApplyAngularImpulse(-angularVelocityProc * body.Inertia, true);
            }
            else
            {
                body.ApplyAngularImpulse(angularVelocityProc * body.Inertia, true);
            }

            return cos;
        }

        /// <summary>
        /// 朝向方向（入参为一个代表方向的向量）
        /// </summary>
        /// <param name="body"></param>
        /// <param name="targetPoint"></param>
        /// <param name="angularVelocityProc"></param>
        /// <returns></returns>
        public static float MoveForward(this Body body, Vector2 targetPoint, float angularVelocityProc)
        {
            targetPoint.Normalize();
            return FowardToTarget(body, targetPoint + body.GetPosition(), angularVelocityProc);
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
        /// 获取各个飞船的质量
        /// </summary>
        /// <param name="gameModel"></param>
        /// <returns></returns>
        public static float GetSpaceWanderMass(GameModel gameModel)
        {
            switch (gameModel)
            {
                case GameModel.WaspShip:
                    {
                        return 75.3f;
                    }
                case GameModel.FighterShipA:
                    {
                        return 81.25f;
                    }
                case GameModel.FighterShipB:
                    {
                        return 63f;
                    }
                case GameModel.DroneShip:
                    {
                        return 5.54f;
                    }
                case GameModel.AnnihilationShip:
                    {
                        return 4.5f;
                    }
                case GameModel.EliteShipA:
                    {
                        return 16100f;
                    }
                case GameModel.EliteShipB:
                    {
                        return 871f;
                    }
                case GameModel.BaseStation:
                    {
                        return 1f;
                    }
                case GameModel.S_Meteorolite:
                    {
                        return 314.15f;
                    }
                case GameModel.M_Meteorolite:
                    {
                        return 1256.636f;
                    }
                case GameModel.L_Meteorolite:
                    {
                        return 5026.548f;
                    }
                case GameModel.MachineGun:
                    {
                        return 0.3f;
                    }
                case GameModel.AntiAircraftGun:
                    {
                        return 0.03f;
                    }
                case GameModel.Torpedo:
                    {
                        return 0.03f;
                    }
                case GameModel.TrackingMissile:
                    {
                        return 0.5f;
                    }
                case GameModel.TimeBomb:
                    {
                        return 8f;
                    }
                case GameModel.TriggerBomb:
                    {
                        return 3f;
                    }
                default:
                    {
                        return 1;
                    }
            }
        }

        /// <summary>
        /// 获取船头的坐标
        /// </summary>
        /// <param name="body"></param>
        /// <param name="gameModel"></param>
        /// <returns></returns>
        public static Vector2 GetSpaceShipNosePosition(this Body body, GameModel gameModel)
        {
            switch (gameModel)
            {
                case GameModel.WaspShip:
                    {
                        return body.GetPosition() + body.GetForward() * 8.25f;
                    }
                case GameModel.FighterShipA:
                    {
                        return body.GetPosition() + body.GetForward() * 3.25f;
                    }
                case GameModel.FighterShipB:
                    {
                        return body.GetPosition() + body.GetForward() * 5.5f;
                    }
                case GameModel.DroneShip:
                    {
                        return body.GetPosition() + body.GetForward();
                    }
                case GameModel.AnnihilationShip:
                    {
                        return body.GetPosition() + body.GetForward();
                    }
                case GameModel.EliteShipA:
                    {
                        return body.GetPosition() + body.GetForward() * 210f;
                    }
                case GameModel.EliteShipB:
                    {
                        return body.GetPosition() + body.GetForward() * 26f;
                    }
                default:
                    {
                        return Vector2.Zero;
                    }
            }
        }

        #region  物体属性


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

        #endregion
    }

    public struct tempBody
    {
        public Body Body;
        public float Distance;
        public tempBody(Body body, float distance)
        {
            Body = body;
            Distance = distance;
        }
    }
}