using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    [Serializable]
    public abstract class BattleEventMessageBase:IEventMessage
    {
        protected int _eventMessageId = EventMessageConstDefine.BattleEventNone;
        public int MessageId
        {
            get => _eventMessageId;
        }
    }





}
