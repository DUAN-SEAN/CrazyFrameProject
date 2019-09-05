using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Vector2 = System.Numerics.Vector2;

namespace Box2DSharp.External
{
    public class Factory
    {
        /// <summary>
        /// 所属 World
        /// </summary>
        public World world;

        public Factory(World world)
        {
            this.world = world;
        }


        /// <summary>
        /// 创建一个BodyDef
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
        public Body CreateCircleBody(float x,float y, float radius, BodyType bodyType = BodyType.DynamicBody, bool isSensor = false)
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





    }
}
