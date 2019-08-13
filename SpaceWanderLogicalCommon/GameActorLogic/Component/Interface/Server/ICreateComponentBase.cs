﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Common;

namespace GameActorLogic
{
    public interface ICreateComponentBase
    {

    }

    public interface ICreateInternalComponentBase : ICreateComponentBase
    {
        ActorBase CreateActor(int actortype, double point_x, double point_y,double angle);
    }
}
