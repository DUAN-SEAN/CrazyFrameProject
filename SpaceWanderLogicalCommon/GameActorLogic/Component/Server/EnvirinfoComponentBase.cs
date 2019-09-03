using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Base;
using CrazyEngine.Core;
using CrazyEngine.External;

namespace GameActorLogic
{

    public class EnvirinfoComponentBase:
        IEnvirinfoBase,
        IEnvirinfoInternalBase
    {
        /// <summary>
        /// 物理引擎
        /// </summary>
        protected Engine m_engine;
        /// <summary>
        /// 物理引擎驱动器
        /// </summary>
        protected Runner m_runner;

        protected CollisionEvent m_collision;
        protected List<ActorBase> _actorList;

        public EnvirinfoComponentBase()
        {
            m_engine = new Engine();
            m_runner = new Runner(m_engine);
            m_collision = new CollisionEvent(m_engine);
            _actorList = new List<ActorBase>();
        }

        /// <summary>
        /// engine 应该已经被添加进 runner
        /// </summary>
        protected EnvirinfoComponentBase(Engine engine,Runner runner)
        {
            m_engine = engine;
            m_runner = runner;
            m_collision = new CollisionEvent(m_engine);
            _actorList = new List<ActorBase>();

        }

        /// <summary>
        /// 对物理引擎和Actor对象的逻辑进行Tick
        /// </summary>
        public void Tick()
        {
           

            for (int i = 0; i < _actorList.Count; i++)
            {
                _actorList[i].Update();
            }
            m_runner.Update(DateTime.Now.Ticks/10000);
            //m_runner.Update();
            m_collision.Update();

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

        public ActorBase GetActorByBodyId(int bodyid)
        {
            return _actorList.Find(actor => actor.GetBodyId() == bodyid);
        }

        #endregion

        #region IEnvirinfoInternalBase
        public Engine GetEngine()
        {
            return m_engine;
        }

        public void SetDelta(float delta)
        {
            m_runner.Delta = delta;
        }

        public float GetDelta()
        {
            return (float) m_runner.Delta;
        }

        public CollisionEvent GetCollisionEvent()
        {
            return m_collision;
        }

        public void AddActor(ActorBase actor)
        {

            IBaseComponentContainer container = actor as IBaseComponentContainer;
            var body = container.GetPhysicalinternalBase().GetBody();
            var collider = container.GetPhysicalinternalBase().GetCollider();
            m_collision.colliders.Add(body.Id.Value, collider);
            m_engine.World.Add(body);
            _actorList.Add(actor);
        }

        public void RemoveActor(ActorBase actor)
        {
            IBaseComponentContainer container = actor as IBaseComponentContainer;
            var body = container.GetPhysicalinternalBase().GetBody();
            m_collision.colliders.Remove(body.Id.Value);
            m_engine.World.Remove(body);
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

            m_collision.Dispose();
            m_collision = null;

            m_engine = null;
            m_runner = null;
           
            
        }
    }
}
