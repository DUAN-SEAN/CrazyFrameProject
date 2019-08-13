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

    public abstract class EnvirinfoComponentBase:
        IEnvirinfoBase,
        IEnvirinfointernalBase
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

        protected EnvirinfoComponentBase()
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

        }

        public void Tick()
        {
            m_runner.Update(DateTime.Now.Ticks);
            m_collision.Update();
        }


        #region IEnvirinfoBase

       

        public void AddToEngine(ObjBase obj)
        {
            m_engine.World.Add(obj);
        }

        public void RemoveFromEngine(ObjBase obj)
        {
            m_engine.World.Remove(obj);
        }
        #endregion

        #region IEnvirinfointernalBase
        public Engine GetEngine()
        {
            return m_engine;
        }
        public CollisionEvent GetCollisionEvent()
        {
            return m_collision;
        }
        #endregion

    }
}
