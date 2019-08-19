using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    [Serializable]
    public class FailEventMessage : BattleEventMessageBase
    {
        public long levelid;
        public FailEventMessage(long levelid)
        {
            _eventMessageId = EventMessageConstDefine.FailEvent;
            this.levelid = levelid;
        }
    }
}
