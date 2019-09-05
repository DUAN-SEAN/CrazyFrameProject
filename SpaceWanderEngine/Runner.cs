using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;
using Crazy.Common;

namespace SpaceWanderEngine
{
    public class Runner
    {
        public Runner(World world)
        {
            this._world = world;


            _fiexdFixedUpdate = new FixedUpdate(TimeSpan.FromSeconds(_timeStep), Tick);
            _fiexdFixedUpdate.Start();
           //Test();
        }

        /// <summary>
        /// 1/60s 执行一次
        /// </summary>
        private void Tick()
        {
           
            _world?.Step(_timeStep, _velocityIterations, _positionIterations);
            if (_body != null)
            {
                Log.Trace(_body.Transform.ToString());
            }
        }
        
        public void Update()
        {
            
            _fiexdFixedUpdate?.Update();
            

        }

        private void Test()
        {

            _world = new World(new Vector2(0, -10f));
            _world.SetContactListener(new SpaceWanderContactListener());
            //_world.DestructionListener = this;
            var GroundBody = _world.CreateBody(new BodyDef());

            var groundBodyDef = new BodyDef { BodyType = BodyType.StaticBody };
            groundBodyDef.Position.Set(0.0f, -10.0f);

            var groundBody = _world.CreateBody(groundBodyDef);

            var groundBox = new PolygonShape();
            groundBox.SetAsBox(1000.0f, 10.0f);

            groundBody.CreateFixture(groundBox, 0.0f);

            // Define the dynamic body. We set its position and call the body factory.
            var bodyDef = new BodyDef { BodyType = BodyType.DynamicBody };

            bodyDef.Position.Set(0, 4f);

            var dynamicBox = new PolygonShape();
            dynamicBox.SetAsBox(1f, 1f, Vector2.Zero, 45f);

            // Define the dynamic body fixture.
            var fixtureDef = new FixtureDef
            {
                Shape = dynamicBox,
                Density = 1.0f,
                Friction = 0.3f
            };

            // Set the box density to be non-zero, so it will be dynamic.

            // Override the default friction.

            // Add the shape to the body.
            var body = _world.CreateBody(bodyDef);
            body.CreateFixture(fixtureDef);
            Random r = new Random();
            for (int i = 1; i <= 100; i++)
            {
                bodyDef.Position = new Vector2(r.Next(-50,50), r.Next(0, 500));
                bodyDef.Angle = r.Next(0, 360);
                _body = _world.CreateBody(bodyDef);
                _body.CreateFixture(fixtureDef).Body.UserData = i.ToString();
                Log.Trace("生成物体成功");
                
            }

            _world.ContactManager.ContactListener = new SpaceWanderContactListener();
        }

        private Body _body;

        private int _velocityIterations = 6;

        private int _positionIterations = 2;

        private float _timeStep = 1/60f;

        private World _world;

        private FixedUpdate _fiexdFixedUpdate;


    }

    public class SpaceWanderContactListener : IContactListener
    {
        public void BeginContact(Contact contact)
        {
           
            Log.Trace("开始碰撞");
            Log.Trace("开始碰撞" + contact.FixtureA.Body.UserData?.ToString() + ":" + contact.FixtureB.Body?.UserData?.ToString());


        }
        public void EndContact(Contact contact)
        {
            Log.Trace("结束碰撞");
            Log.Trace("结束碰撞" + contact.FixtureA.Body?.UserData?.ToString() + ":" + contact.FixtureB.Body?.UserData?.ToString());
        }

        public void PreSolve(Contact contact, in Manifold oldManifold)
        {
            //Debug.Log("PreSolve" + contact.FixtureA.Body?.UserData.ToString() + ":" + contact.FixtureB.Body?.UserData.ToString());
        }

        public void PostSolve(Contact contact, in ContactImpulse impulse)
        {
            //Debug.Log("PostSolve" + contact.FixtureA.Body?.UserData.ToString() + ":" + contact.FixtureB.Body?.UserData.ToString());
        }
    }
}
