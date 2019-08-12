using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    public class RightEventMessage : BattleEventMessageBase
    {
        public double Rightang = -0.0000001;
        public RightEventMessage()
        {
            _eventMessageId = EventMessageConstDefine.RightEvent;
        }

        public RightEventMessage(double rightang)
        {
            _eventMessageId = EventMessageConstDefine.RightEvent;
            Rightang = rightang;
        }
    }
}
