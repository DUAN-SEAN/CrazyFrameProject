﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace SpaceWanderLogicalCommon.Event
{
    [Serializable]
    public class DestroyEventMessage : BattleEventMessageBase
    {
        protected ulong actorid;
        public DestroyEventMessage(ulong actorid)
        {
            _eventMessageId = EventMessageConstDefine.DestroyEvent;
            this.actorid = actorid;
        }
    }
}
