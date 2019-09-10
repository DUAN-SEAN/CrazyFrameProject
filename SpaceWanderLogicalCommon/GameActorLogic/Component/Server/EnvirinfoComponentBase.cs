
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Box2DSharp.Dynamics;
using Box2DSharp.External;
using Crazy.Common;
using SpaceWanderEngine;

namespace GameActorLogic
{

    public class EnvirinfoComponentBase :
        IEnvirinfoBase,
        IEnvirinfoInternalBase
    {
        /// <summary>
        /// 工厂
        /// </summary>
        protected Factory factory;

        /// <summary>
        /// 物理引擎
        /// </summary>
        protected World _world;

        /// <summary>
        /// 物理引擎驱动器
        /// </summary>
        protected Runner m_runner;

        protected List<ActorBase> _actorList;

        /// <summary>
        /// 关卡地图长宽
        /// </summary>
        protected float MapHeight;
        protected float MapWidth;
        public EnvirinfoComponentBase()
        {
            _world = new World(Vector2.Zero);
            factory = new Factory(_world);
            m_runner = new Runner(_world);
            _actorList = new List<ActorBase>();
            _world.IsAutoClearForces = false;
        }

        /// <summary>
        /// engine 应该已经被添加进 runner
        /// </summary>
        protected EnvirinfoComponentBase(World engine, Runner runner)
        {
            _world = engine;
            m_runner = runner;

            _actorList = new List<ActorBase>();

        }

        protected Stopwatch stopwatch = new Stopwatch();
        /// <summary>
        /// 对物理引擎和Actor对象的逻辑进行Tick
        /// </summary>
        public void Tick()
        {
            stopwatch?.Restart();
            for (int i = 0; i < _actorList.Count; i++)
            {
                _actorList[i].Update();
                //if (_actorList[i].IsWeapon())
                //Log.Trace("EnvirinfoComponentBase:ActorId" + _actorList[i].GetActorID() + " ActorType" + _actorList[i].GetActorType() + " 位置坐标:" + _actorList[i].GetPosition() + " 力" + _actorList[i].GetForce() + " 速度" + _actorList[i].GetVelocity() + " 转矩" + _actorList[i].GetAngleVelocity());
                //if(_actorList[i].GetActorType() == ActorTypeBaseDefine.ContinuousLaserActor)
                //Log.Trace("EnvirinfoComponentBase:ActorId" + _actorList[i].GetActorID() + " ActorType" + _actorList[i].GetActorType() + "Fixture Count" + ((IBaseComponentContainer)_actorList[i]).GetPhysicalinternalBase().GetBody().FixtureList.Count + " IsSenior" + ((IBaseComponentContainer)_actorList[i]).GetPhysicalinternalBase().GetBody().FixtureList[0].IsSensor);
            }
            stopwatch?.Stop();
            //if (stopwatch.ElapsedMilliseconds > 0)
            //    Log.Trace("Tick _actorList:" + stopwatch.ElapsedMilliseconds);
            stopwatch?.Restart();

            m_runner.Update();

            stopwatch?.Stop();
            //if (stopwatch.ElapsedMilliseconds > 0)
            //    Log.Trace("Tick m_runner:" + stopwatch.ElapsedMilliseconds);

        }


        #region IEnvirinfoBase

        public List<ActorBase> GetAllActors()
        {
            return _actorList;
        }

        public List<ActorBase> GetPlayerActors()
        {
            return _actorList.Where(t => t.IsPlayer()).ToList();
        }

        public List<ActorBase> GetShipActors()
        {
            return _actorList.Where(t => t.IsShip()).ToList();
        }

        public List<ActorBase> GetWeaponActors()
        {
            return _actorList.Where(t => t.IsWeapon()).ToList();
        }

        public ActorBase GetActor(ulong id)
        {
            return _actorList.Find(actor => actor.GetActorID() == id);
        }

        public ActorBase GetActorByBodyUserData(UserData bodyid)
        {
            return _actorList.Find(actor => actor.GetBodyUserData() == bodyid);
        }

        #endregion

        #region IEnvirinfoInternalBase

        public void SetDelta(float delta)
        {
        }

        public float GetDelta()
        {
            return 0;
        }


        public void AddActor(ActorBase actor)
        {

            IBaseComponentContainer container = actor as IBaseComponentContainer;
            var init = container.GetInitData();
            //actor.CreateBody(factory.CreateRectangleBody(init.point_x, init.point_y, 10, 10));

            //判断是否是激光
            if (actor.GetActorType() != ActorTypeBaseDefine.ContinuousLaserActor && actor.GetActorType() != ActorTypeBaseDefine.PowerLaserActor)
                actor.CreateBody(factory.CreateSpaceWonderBody(new Vector2(init.point_x, init.point_y), init.angle, actor.GetGameModelByActorType(), new UserData(actor.GetActorID(), actor.GetActorType())));
            //是持续激光
            else if (actor.GetActorType() == ActorTypeBaseDefine.ContinuousLaserActor)
            {
                //获取长宽属性
                var WH = ActorHelper.GetLaserShapeByShip(actor.GetActorType());
                actor.CreateBody(factory.CreateSpaceWonderLaser(new Vector2(init.point_x, init.point_y), init.angle, new UserData(actor.GetActorID(), actor.GetActorType()), WH.X, WH.Y));
            }
            //是蓄力激光
            else if (actor.GetActorType() == ActorTypeBaseDefine.PowerLaserActor)
            {
                var WH = ActorHelper.GetLaserShapeByShip(actor.GetActorType(), heightpro: actor.GetActorInitPro());
                actor.CreateBody(factory.CreateSpaceWonderLaser(new Vector2(init.point_x, init.point_y), init.angle, new UserData(actor.GetActorID(), actor.GetActorType()), WH.X, WH.Y));
            }
            //Log.Trace("AddActor: actorID" + actor.GetActorID() + " InitDate" + init.point_x + " " + init.point_y + " " + init.angle);
            //Log.Trace("actor id" + actor.GetActorID() + " 生成一个Actor Position:" + actor.GetPosition() + " Forward:" + actor.GetForward());

            _actorList.Add(actor);
        }

        public void RemoveActor(ActorBase actor)
        {
            IBaseComponentContainer container = actor as IBaseComponentContainer;
            var body = container.GetPhysicalinternalBase().GetBody();
            //TODO 从世界去除
            _world.DestroyBody(body);
            _actorList.Remove(actor);
        }
        #endregion

        public void Dispose()
        {
            foreach (var actorBase in _actorList)
            {
                actorBase.Dispose();
            }
            _actorList.Clear();
            _actorList = null;


            m_runner = null;
            _world = null;


        }

        /// <summary>
        /// 添加空气墙
        /// </summary>
        public void AddAirWall()
        {
            factory.CreateRectangleBody(0, -MapHeight / 2, MapWidth, 5, bodyType: BodyType.StaticBody);
            factory.CreateRectangleBody(0, MapHeight / 2, MapWidth, 5, bodyType: BodyType.StaticBody);
            factory.CreateRectangleBody(MapWidth / 2, 0, 5, MapHeight, bodyType: BodyType.StaticBody);
            factory.CreateRectangleBody(-MapWidth / 2, 0, 5, MapHeight, bodyType: BodyType.StaticBody);

        }

        public void SetMapSize(float height, float width)
        {
            this.MapHeight = height;
            this.MapWidth = width;
        }

        public Factory GetFactory()
        {
            return factory;
        }

        public void SetContactListener(IContactListener contactListener)
        {
            _world?.SetContactListener(contactListener);
        }
    }
}
