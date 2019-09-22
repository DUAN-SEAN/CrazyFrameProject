using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using Vector2 = System.Numerics.Vector2;

namespace Box2DSharp.External
{
    public static class CrazyUtils
    {
        public static float IncludedAngleCos(Vector2 v1, Vector2 v2)
        {
            v1.Normalize(); v2.Normalize();
            float dot = Vector2.Dot(v1, v2);
            return 2 * dot / (v1.Length() + v2.Length());
        }
    }
}
