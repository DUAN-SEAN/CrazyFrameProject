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
            pairList = engine.Pairs.PairList.Where(x => x.Active).ToList();
        }

        public void Update()
        {
            foreach(var pair in pairList)
            {
                Body bodyA = pair.Collision.BodyA;
                Body bodyB = pair.Collision.BodyB;
                if (colliders.TryGetValue(bodyA.Id.Value, out Collider colliderA))
                    colliderA.OnCollisionStay(pair.Collision);
                if (colliders.TryGetValue(bodyB.Id.Value, out Collider colliderB))
                    colliderB.OnCollisionStay(pair.Collision);
            }
        }

        public void Dispose()
        {
            engine = null;
            pairList.Clear();
            pairList = null;
            colliders.Clear();
            colliders = null;
        }

    }
}
