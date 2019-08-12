using System;
using System.Collections.Generic;
using CrazyEngine.Base;
using CrazyEngine.Common;
using CrazyEngine.Core;


namespace CrazyEngine.External
{
    public abstract class Collider
    {

        public abstract void OnCollisionStay(Collision collision);

    }
}
