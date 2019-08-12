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
        public Dictionary<Body, Collider> colliders = new Dictionary<Body, Collider>();

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
                if (colliders.TryGetValue(bodyA, out Collider colliderA))
                    colliderA.OnCollisionStay(pair.Collision);
                if (colliders.TryGetValue(bodyB, out Collider colliderB))
                    colliderB.OnCollisionStay(pair.Collision);
            }
        }

    }
}
