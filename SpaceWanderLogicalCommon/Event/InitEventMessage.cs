using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace SpaceWanderLogicalCommon.Event
{
    [Serializable]
    public class InitEventMessage : BattleEventMessageBase
    {
        protected ulong actorid;
        public InitEventMessage(ulong actorid)
        {
            _eventMessageId = EventMessageConstDefine.InitEvent;
            this.actorid = actorid;
        }
    }
}
