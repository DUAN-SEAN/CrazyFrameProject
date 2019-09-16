using System;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Vector2 = System.Numerics.Vector2;

namespace Box2DSharp.External
{
    /// <summary>
    /// 工厂
    /// </summary>
    public class Factory
    {
        /// <summary>
        /// 所属 World
        /// </summary>
        public World world;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="_world"></param>
        public Factory(World _world)
        {
            world = _world;
        }

        #region BodyDef

        /// <summary>
        /// 创建一个BodyDef,指定坐标。
        /// </summary>
        /// <param name="x">坐标 X</param>
        /// <param name="y">坐标 Y</param>
        /// <param name="bodyType">Body 类型，默认是DynamicBody</param>
        /// <param name="isSensor">是否不参加物理模拟，默认否</param>
        /// <returns></returns>
        public static BodyDef CreateBodyDef(float x, float y, BodyType bodyType = BodyType.DynamicBody, bool isSensor = false)
        {
            var bodyDef = new BodyDef { BodyType = bodyType };
            bodyDef.Position.Set(x, y);

            return bodyDef;
        }

        /// <summary>
        /// 创建一个BodyDef,指定坐标和角度。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="angle"></param>
        /// <param name="bodyType"></param>
        /// <param name="isSensor"></param>
        /// <returns></returns>
        public static BodyDef CreateBodyDef(float x, float y, float angle, BodyType bodyType = BodyType.DynamicBody, bool isSensor = false)
        {
            var bodyDef = new BodyDef { BodyType = bodyType };
            bodyDef.Position.Set(x, y);
            bodyDef.Angle = angle;

            return bodyDef;
        }

        #endregion

        #region BodyShape

        /// <summary>
        /// 三角形外形
        /// </summary>
        /// <param name="bottom"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        internal PolygonShape CreateTriangleShape(float bottom, float height, float offsetX = 0, float offsetY = 0)
        {
            var bodyShape = new PolygonShape();
            var vertices = new Vector2[3];
            vertices[0].Set(-bottom / 2 + offsetX, offsetY);
            vertices[1].Set(bottom / 2 + offsetX, offsetY);
            vertices[2].Set(offsetX, height + offsetY);
            bodyShape.Set(vertices);
            return bodyShape;
        }

        /// <summary>
        /// 矩形外形
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        internal PolygonShape CreateRectangleShape(float width, float height, float offsetX = 0, float offsetY = 0)
        {
            var bodyShape = new PolygonShape();
            var vertices = new Vector2[4];
            vertices[0].Set(-width / 2 + offsetX, -height / 2 + offsetY);
            vertices[1].Set(-width / 2 + offsetX, height / 2 + offsetY);
            vertices[2].Set(width / 2 + offsetX, height / 2 + offsetY);
            vertices[3].Set(width / 2 + offsetX, -height / 2 + offsetY);
            bodyShape.Set(vertices);
            return bodyShape;
        }

        /// <summary>
        /// 矩形外形(原点在下底)
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        internal PolygonShape CreateRectangleShape2(float width, float height, float offsetX = 0, float offsetY = 0)
        {
            var bodyShape = new PolygonShape();
            var vertices = new Vector2[4];
            vertices[0].Set(-width / 2 + offsetX, offsetY);
            vertices[1].Set(-width / 2 + offsetX, height + offsetY);
            vertices[2].Set(width / 2 + offsetX, height + offsetY);
            vertices[3].Set(width / 2 + offsetX, offsetY);
            bodyShape.Set(vertices);
            return bodyShape;
        }

        /// <summary>
        /// 圆形外形
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        internal CircleShape CreateCircleShape(float radius, float offsetX = 0, float offsetY = 0)
        {
            var bodyShape = new CircleShape();
            bodyShape.Radius = radius;
            bodyShape.Position = new Vector2(offsetX, offsetY);
            return bodyShape;
        }

        /// <summary>
        /// 梯形外形
        /// </summary>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        internal PolygonShape CreateTrapezoidShape(float top, float bottom, float height, float offsetX = 0, float offsetY = 0)
        {
            var bodyShape = new PolygonShape();
            var vertices = new Vector2[4];
            vertices[0].Set(-top / 2 + offsetX, height / 2 + offsetY);
            vertices[1].Set(top / 2 + offsetX, height / 2 + offsetY);
            vertices[2].Set(bottom / 2 + offsetX, -height / 2 + offsetY);
            vertices[3].Set(-bottom / 2 + offsetX, -height / 2 + offsetY);
            bodyShape.Set(vertices);
            return bodyShape;
        }

        #endregion

        #region FixtureDef

        /// <summary>
        /// 创建飞船的FixtureDef
        /// </summary>
        /// <param name="bodyShape"></param>
        /// <returns></returns>
        internal FixtureDef CreateShipFixtureDef(Shape bodyShape)
        {
            var fixtureDef = new FixtureDef
            {
                Shape = bodyShape,
                Density = 1.0f,
                Friction = 0.3f,
            };
            return fixtureDef;
        }

        /// <summary>
        /// 创建子弹的FixtureDef
        /// </summary>
        /// <param name="bodyShape"></param>
        /// <returns></returns>
        internal FixtureDef CreateBulletFixtureDef(Shape bodyShape)
        {
            var fixtureDef = new FixtureDef
            {
                Shape = bodyShape,
                Density = 1.0f,
                Friction = 0.1f,
                IsSensor = true
            };
            return fixtureDef;
        }

        /// <summary>
        /// 创建导弹的FixtureDef
        /// </summary>
        /// <param name="bodyShape"></param>
        /// <returns></returns>
        internal FixtureDef CreateMissileFixtureDef(Shape bodyShape)
        {
            var fixtureDef = new FixtureDef
            {
                Shape = bodyShape,
                Density = 1.0f,
                Friction = 0.1f,
                IsSensor = true
            };
            return fixtureDef;
        }

        #endregion

        #region SpaceWander

        public Body CreateSpaceWonderBody(Vector2 position, float angle, GameModel model, object userData)
        {
            switch (model)
            {
                case GameModel.WaspShip:
                    {
                        return CreateWaspShipBody(position, angle, userData);
                    }
                case GameModel.FighterShipA:
                    {
                        return CreateFighterShipABody(position, angle, userData);
                    }
                case GameModel.FighterShipB:
                    {
                        return CreateFighterShipBBody(position, angle, userData);
                    }
                case GameModel.DroneShip:
                    {
                        return CreateDroneShipBody(position, angle, userData);
                    }
                case GameModel.AnnihilationShip:
                    {
                        return CreateAnnihilationShipBody(position, angle, userData);
                    }
                case GameModel.EliteShipA:
                    {
                        return CreateEliteShipABody(position, angle, userData);
                    }
                case GameModel.EliteShipB:
                    {
                        return CreateEliteShipBBody(position, angle, userData);
                    }
                //case GameModel.BaseStation:
                //    {
                //        return CreateBaseStationBody(position, angle, userData);
                //    }
                case GameModel.S_Meteorolite:
                    {
                        return CreateMeteoroliteBody(position, angle, userData, 10);
                    }
                case GameModel.M_Meteorolite:
                    {
                        return CreateMeteoroliteBody(position, angle, userData, 20);
                    }
                case GameModel.L_Meteorolite:
                    {
                        return CreateMeteoroliteBody(position, angle, userData, 40);
                    }

                case GameModel.BlackHole:
                    {
                        return CreateBlackHoleBody(position, angle, userData);
                    }

                case GameModel.MachineGun:
                    {
                        return CreateMachineGunBody(position, angle, userData);
                    }
                case GameModel.AntiAircraftGun:
                    {
                        return CreateAntiAircraftGunBody(position, angle, userData);
                    }
                case GameModel.Torpedo:
                    {
                        return CreateTorpedoBody(position, angle, userData);
                    }
                case GameModel.TrackingMissile:
                    {
                        return CreateTrackingMissileBody(position, angle, userData);
                    }
                //case GameModel.ContinuousLaser:
                //    {
                //        return CreateContinuousLaserBody(position, angle, userData);
                //    }
                //case GameModel.PowerLaser:
                //    {
                //        return CreatePowerLaserBody(position, angle, userData);
                //    }
                case GameModel.TimeBomb:
                    {
                        return CreateTimeBombBody(position, angle, userData);
                    }
                case GameModel.TriggerBomb:
                    {
                        return CreateTriggerBomb(position, angle, userData);
                    }
                default:
                    {
                        return CreateRectangleBody(position.X, position.Y, 1, 1, BodyType.DynamicBody);
                    }
            }
        }

        /// <summary>
        /// 生成激光
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Body CreateSpaceWonderLaser(Vector2 position, float angle, object userData, float width, float height)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateRectangleShape2(width, height);

            var fd1 = CreateMissileFixtureDef(bodyShape);

            body.CreateFixture(fd1);

            body.UserData = userData;
            return body;
        }
        /// <summary>
        /// 黑洞
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateBlackHoleBody(Vector2 position, float angle, object userData)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle, BodyType.StaticBody);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateCircleShape(30);
            var fd1 = CreateBulletFixtureDef(bodyShape);

            body.CreateFixture(fd1);
            body.UserData = userData;
            return body;
        }


        /// <summary>
        /// 陨石
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateMeteoroliteBody(Vector2 position, float angle, object userData, float radius)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle, BodyType.StaticBody);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateCircleShape(radius);
            var fd1 = CreateShipFixtureDef(bodyShape);

            body.CreateFixture(fd1);
            body.UserData = userData;
            return body;
        }

        /// <summary>
        /// 触发炸弹
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateTriggerBomb(Vector2 position, float angle, object userData)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateRectangleShape(1, 3);
            var fd1 = CreateBulletFixtureDef(bodyShape);

            body.CreateFixture(fd1);
            body.UserData = userData;
            body.IsBullet = true;
            return body;
        }

        /// <summary>
        /// 定时炸弹
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateTimeBombBody(Vector2 position, float angle, object userData)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateRectangleShape(2, 4);
            var fd1 = CreateBulletFixtureDef(bodyShape);

            body.CreateFixture(fd1);
            body.UserData = userData;
            body.IsBullet = true;
            return body;
        }

        //private Body CreatePowerLaserBody(Vector2 position, float angle, object userData)
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// 持续激光
        ///// </summary>
        ///// <param name="position"></param>
        ///// <param name="angle"></param>
        ///// <param name="userData"></param>
        ///// <returns></returns>
        //private Body CreateContinuousLaserBody(Vector2 position, float angle, object userData)
        //{

        //}

        /// <summary>
        /// 跟踪导弹
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateTrackingMissileBody(Vector2 position, float angle, object userData)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateRectangleShape(0.5f, 1);
            var fd1 = CreateBulletFixtureDef(bodyShape);

            body.CreateFixture(fd1);
            body.UserData = userData;
            body.IsBullet = true;
            return body;
        }

        /// <summary>
        /// 鱼雷
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateTorpedoBody(Vector2 position, float angle, object userData)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateCircleShape(0.1f);
            var fd1 = CreateBulletFixtureDef(bodyShape);

            body.CreateFixture(fd1);
            body.UserData = userData;
            body.IsBullet = true;
            return body;
        }

        /// <summary>
        /// 高射炮
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateAntiAircraftGunBody(Vector2 position, float angle, object userData)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateCircleShape(0.1f);
            var fd1 = CreateBulletFixtureDef(bodyShape);

            body.CreateFixture(fd1);
            body.UserData = userData;
            body.IsBullet = true;
            return body;
        }

        /// <summary>
        /// 机枪
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateMachineGunBody(Vector2 position, float angle, object userData)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateRectangleShape(0.3f, 1);
            var fd1 = CreateBulletFixtureDef(bodyShape);

            body.CreateFixture(fd1);
            body.UserData = userData;
            body.IsBullet = true;
            return body;
        }

        /// <summary>
        /// 基站
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateBaseStationBody(Vector2 position, float angle, object userData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 精英船B
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateEliteShipBBody(Vector2 position, float angle, object userData)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateRectangleShape(6, 52);

            var bodyShape2 = CreateRectangleShape(10, 18, -8, -1);
            var bodyShape3 = CreateRectangleShape(10, 18, 8, -1);

            var bodyShape4 = new PolygonShape();
            var vertices = new Vector2[3];
            vertices[0].Set(-3, 10f);
            vertices[1].Set(-3f, 23f);
            vertices[2].Set(-14, 23f);
            bodyShape4.Set(vertices);

            var bodyShape5 = new PolygonShape();
            vertices[0].Set(3, 10f);
            vertices[1].Set(3f, 23f);
            vertices[2].Set(14, 23f);
            bodyShape5.Set(vertices);

            var bodyShape6 = new PolygonShape();
            vertices[0].Set(-3, -11f);
            vertices[1].Set(-3f, -19f);
            vertices[2].Set(-10, -19f);
            bodyShape6.Set(vertices);

            var bodyShape7 = new PolygonShape();
            vertices[0].Set(3, -11f);
            vertices[1].Set(3f, -19f);
            vertices[2].Set(10, -19f);
            bodyShape7.Set(vertices);


            var fd1 = CreateShipFixtureDef(bodyShape);
            var fd2 = CreateShipFixtureDef(bodyShape2);
            var fd3 = CreateShipFixtureDef(bodyShape3);
            var fd4 = CreateShipFixtureDef(bodyShape4);
            var fd5 = CreateShipFixtureDef(bodyShape5);
            var fd6 = CreateShipFixtureDef(bodyShape6);
            var fd7 = CreateShipFixtureDef(bodyShape7);

            body.CreateFixture(fd1);
            body.CreateFixture(fd2);
            body.CreateFixture(fd3);
            body.CreateFixture(fd4);
            body.CreateFixture(fd5);
            body.CreateFixture(fd6);
            body.CreateFixture(fd7);

            body.UserData = userData;
            body.LinearDamping = 0.01f;
            return body;
        }

        /// <summary>
        /// 精英船A
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateEliteShipABody(Vector2 position, float angle, object userData)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateRectangleShape(60, 160);
            var bodyShape2 = CreateTrapezoidShape(40, 60, 130, 0, 145);

            var fd1 = CreateShipFixtureDef(bodyShape);
            var fd2 = CreateShipFixtureDef(bodyShape2);

            body.CreateFixture(fd1);
            body.CreateFixture(fd2);

            body.UserData = userData;
            body.LinearDamping = 0.01f;
            return body;
        }

        /// <summary>
        /// 歼灭船
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateAnnihilationShipBody(Vector2 position, float angle, object userData)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateRectangleShape(2, 2);

            var bodyShape2 = new PolygonShape();
            var vertices = new Vector2[3];
            vertices[0].Set(-1, 0f);
            vertices[1].Set(-2f, -0.5f);
            vertices[2].Set(-1, -0.5f);
            bodyShape2.Set(vertices);

            var bodyShape3 = new PolygonShape();
            vertices[0].Set(1, 0f);
            vertices[1].Set(2f, -0.5f);
            vertices[2].Set(1, -0.5f);
            bodyShape3.Set(vertices);

            var fd1 = CreateShipFixtureDef(bodyShape);
            var fd2 = CreateShipFixtureDef(bodyShape2);
            var fd3 = CreateShipFixtureDef(bodyShape3);

            body.CreateFixture(fd1);
            body.CreateFixture(fd2);
            body.CreateFixture(fd3);

            body.UserData = userData;
            body.LinearDamping = 0.01f;
            return body;
        }

        /// <summary>
        /// 无人机
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateDroneShipBody(Vector2 position, float angle, object userData)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateCircleShape(1);
            var bodyShape2 = CreateRectangleShape(0.3f, 4, -1.15f, 1f);
            var bodyShape3 = CreateRectangleShape(0.3f, 4, 1.15f, 1f);

            var fd1 = CreateShipFixtureDef(bodyShape);
            var fd2 = CreateShipFixtureDef(bodyShape2);
            var fd3 = CreateShipFixtureDef(bodyShape3);

            body.CreateFixture(fd1);
            body.CreateFixture(fd2);
            body.CreateFixture(fd3);

            body.UserData = userData;
            body.LinearDamping = 0.01f;
            return body;
        }

        /// <summary>
        /// 战斗机B
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateFighterShipBBody(Vector2 position, float angle, object userData)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateTrapezoidShape(4, 14, 5);
            var bodyShape2 = new PolygonShape();
            var vertices = new Vector2[4];
            vertices[0].Set(-1, -2.5f);
            vertices[1].Set(-2f, -6.5f);
            vertices[2].Set(2, -6.5f);
            vertices[3].Set(1, -2.5f);
            bodyShape2.Set(vertices);

            var bodyShape3 = new PolygonShape();
            vertices[0].Set(-1, 2.5f);
            vertices[1].Set(-1f, 5.5f);
            vertices[2].Set(1f, 5.5f);
            vertices[3].Set(1f, 2.5f);
            bodyShape3.Set(vertices);

            var fd1 = CreateShipFixtureDef(bodyShape);
            var fd2 = CreateShipFixtureDef(bodyShape2);
            var fd3 = CreateShipFixtureDef(bodyShape3);

            body.CreateFixture(fd1);
            body.CreateFixture(fd2);
            body.CreateFixture(fd3);

            body.UserData = userData;
            body.LinearDamping = 0.1f;
            return body;
        }

        /// <summary>
        /// 战斗机A
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        private Body CreateFighterShipABody(Vector2 position, float angle, object userData)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle);
            var body = world.CreateBody(bodyDef);

            var bodyShape = CreateTrapezoidShape(14, 3, 6.5f);
            var bodyShape2 = new PolygonShape();
            var vertices = new Vector2[4];
            vertices[0].Set(-1.5f, -3.25f);
            vertices[1].Set(1.5f, -3.25f);
            vertices[2].Set(3, -7.25f);
            vertices[3].Set(-3, -7.25f);
            bodyShape2.Set(vertices);

            var bodyShape3 = new PolygonShape();
            vertices[0].Set(-1.5f, 3.25f);
            vertices[1].Set(-0.5f, 7.25f);
            vertices[2].Set(0.5f, 7.25f);
            vertices[3].Set(1.5f, 3.25f);
            bodyShape3.Set(vertices);

            var fd1 = CreateShipFixtureDef(bodyShape);
            var fd2 = CreateShipFixtureDef(bodyShape2);
            var fd3 = CreateShipFixtureDef(bodyShape3);

            body.CreateFixture(fd1);
            body.CreateFixture(fd2);
            body.CreateFixture(fd3);

            body.UserData = userData;
            body.LinearDamping = 0.1f;
            return body;

        }

        /// <summary>
        /// 黄蜂
        /// </summary>
        /// <returns></returns>
        private Body CreateWaspShipBody(Vector2 position, float angle, object userData)
        {
            var bodyDef = CreateBodyDef(position.X, position.Y, angle);
            var body = world.CreateBody(bodyDef);
            var bodyShape = CreateRectangleShape(4.2f, 8.5f);

            var bodyShape2 = new PolygonShape();
            var vertices = new Vector2[3];
            vertices[0].Set(1.2f, 4.25f);
            vertices[1].Set(-1.2f, 4.25f);
            vertices[2].Set(0, 8.25f);
            bodyShape2.Set(vertices);

            var bodyShape3 = new PolygonShape();
            vertices[0].Set(-2.2f, -4.25f);
            vertices[1].Set(-2.2f, 1.75f);
            vertices[2].Set(-8, 4.25f);
            bodyShape3.Set(vertices);

            var bodyShape4 = new PolygonShape();
            vertices[0].Set(2.2f, -4.25f);
            vertices[1].Set(2.2f, 1.75f);
            vertices[2].Set(8, 4.25f);
            bodyShape4.Set(vertices);

            var fd1 = CreateShipFixtureDef(bodyShape);
            var fd2 = CreateShipFixtureDef(bodyShape2);
            var fd3 = CreateShipFixtureDef(bodyShape3);
            var fd4 = CreateShipFixtureDef(bodyShape4);
            body.CreateFixture(fd1);
            body.CreateFixture(fd2);
            body.CreateFixture(fd3);
            body.CreateFixture(fd4);

            body.UserData = userData;
            body.LinearDamping = 0.1f;
            return body;
        }

        #endregion

        #region Body(直接创建一个Body，不建议这么做)

        /// <summary>
        /// 创建一个矩形Body
        /// </summary>
        /// <param name="x">坐标 X</param>
        /// <param name="y">坐标 Y</param>
        /// <param name="width">矩形的宽</param>
        /// <param name="height">矩形的高</param>
        /// <param name="bodyType">物体类型，默认是DynamicBody</param>
        /// <param name="isSensor">是否不参加物理模拟，默认否</param>
        /// <returns></returns>
        public Body CreateRectangleBody(float x, float y, float width, float height, BodyType bodyType = BodyType.DynamicBody, bool isSensor = false)
        {
            var bodyDef = CreateBodyDef(x, y, bodyType);

            var body = world.CreateBody(bodyDef);
            var bodyShape = new PolygonShape();
            bodyShape.SetAsBox(width / 2, height / 2);

            var fixtureDef = new FixtureDef
            {
                Shape = bodyShape,
                Density = 1.0f,
                Friction = 0.3f,
                IsSensor = isSensor
            };

            body.CreateFixture(fixtureDef);

            return body;
        }

        /// <summary>
        /// 创建一个圆形Body
        /// </summary>
        /// <param name="x">坐标 X</param>
        /// <param name="y">坐标 Y</param>
        /// <param name="radius">圆形 半径</param>
        /// <param name="bodyType">物体类型，默认是DynamicBody</param>
        /// <param name="isSensor">是否不参加物理模拟，默认否</param>
        /// <returns></returns>
        public Body CreateCircleBody(float x, float y, float radius, BodyType bodyType = BodyType.DynamicBody, bool isSensor = false)
        {
            var bodyDef = CreateBodyDef(x, y, bodyType);

            var body = world.CreateBody(bodyDef);
            var bodyShape = new CircleShape();
            bodyShape.Radius = radius;

            var fixtureDef = new FixtureDef
            {
                Shape = bodyShape,
                Density = 1.0f,
                Friction = 0.3f,
                IsSensor = isSensor
            };

            body.CreateFixture(fixtureDef);

            return body;
        }

        /// <summary>
        /// 创建一个三角形Body
        /// </summary>
        /// <param name="x">坐标 X</param>
        /// <param name="y">坐标 Y</param>
        /// <param name="bottom">物体的底</param>
        /// <param name="height">物体的高</param>
        /// <param name="bodyType">物体类型，默认是DynamicBody</param>
        /// <param name="isSensor">是否不参加物理模拟，默认否</param>
        /// <returns></returns>
        public Body CreateTriggleBody(float x, float y, float bottom, float height, BodyType bodyType = BodyType.DynamicBody, bool isSensor = false)
        {
            var bodyDef = CreateBodyDef(x, y, bodyType);

            var body = world.CreateBody(bodyDef);
            var bodyShape = new PolygonShape();

            var vertices = new Vector2[3];
            vertices[0].Set(-bottom / 2, 0);
            vertices[1].Set(bottom / 2, 0);
            vertices[2].Set(0, height);
            bodyShape.Set(vertices);

            var fixtureDef = new FixtureDef
            {
                Shape = bodyShape,
                Density = 1.0f,
                Friction = 0.3f,
                IsSensor = isSensor
            };

            body.CreateFixture(fixtureDef);

            return body;
        }


        /// <summary>
        /// 创建一个梯形物体
        /// </summary>
        /// <param name="x">坐标 X</param>
        /// <param name="y">坐标 Y</param>
        /// <param name="top">上底</param>
        /// <param name="bottom">下底</param>
        /// <param name="height">高</param>
        /// <param name="bodyType">物体类型，默认是DynamicBody</param>
        /// <returns></returns>
        public Body CreateTrapezoidBody(float x, float y, float top, float bottom, float height, BodyType bodyType = BodyType.DynamicBody, bool isSensor = false)
        {
            var bodyDef = CreateBodyDef(x, y, bodyType);
            var body = world.CreateBody(bodyDef);
            var bodyShape = new PolygonShape();

            var vertices = new Vector2[4];
            vertices[0].Set(-top / 2, height / 2);
            vertices[1].Set(top / 2, height / 2);
            vertices[2].Set(bottom / 2, -height / 2);
            vertices[3].Set(-bottom / 2, -height / 2);
            bodyShape.Set(vertices);

            var fixtureDef = new FixtureDef
            {
                Shape = bodyShape,
                Density = 1.0f,
                Friction = 0.3f,
                IsSensor = isSensor
            };

            body.CreateFixture(fixtureDef);

            return body;
        }

        #endregion

    }

}
