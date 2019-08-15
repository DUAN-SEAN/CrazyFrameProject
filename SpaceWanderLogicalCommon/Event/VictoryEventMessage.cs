using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class VictoryEventMessage : BattleEventMessageBase
    {
        public long levelid;
        public VictoryEventMessage(long levelid)
        {
            _eventMessageId = EventMessageConstDefine.VictoryEvent;
            this.levelid = levelid;
        }
    }
}
