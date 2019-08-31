using System;
using System.Collections.Generic;
using System.Linq;
using CrazyEngine.Base;
using CrazyEngine.Common;
using CrazyEngine.Core;

namespace CrazyEngine.External
{
    public class CollisionEvent
    {
        public Engine engine;
        public List<Pair> pairList;
        public Dictionary<int, Collider> colliders = new Dictionary<int, Collider>();

        public CollisionEvent(Engine engine)
        {
            this.engine = engine;
        }

        public void Update()
        {
            pairList = engine.Pairs.PairList.Where(x => x.Active).ToList();
            
            foreach(var pair in pairList)
            {
                Body bodyA = pair.Collision.BodyA;
                Body bodyB = pair.Collision.BodyB;
                if (colliders.TryGetValue(bodyA.Id.Value, out Collider colliderA))
                    colliderA.OnCollisionStay?.Invoke(pair.Collision);
                if (colliders.TryGetValue(bodyB.Id.Value, out Collider colliderB))
                    colliderB.OnCollisionStay?.Invoke(pair.Collision);
            }
        }

        public void Dispose()
        {
            engine = null;
            pairList?.Clear();
            pairList = null;
            colliders.Clear();
            colliders = null;
        }

    }
}
