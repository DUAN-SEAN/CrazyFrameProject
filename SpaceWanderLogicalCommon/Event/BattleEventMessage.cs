﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    [Serializable]
    public abstract class BattleEventMessageBase:IEventMessage
    {
        public int MessageId
        {
            get => EventMessageConstDefine.BattleEventNone;
        }
    }





}
