using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    public class LeftEventMessage:BattleEventMessageBase
    {
        public double Leftang = 0.0000001;
        public LeftEventMessage()
        {
            _eventMessageId = EventMessageConstDefine.LeftEvent;
        }

        public LeftEventMessage(double leftang)
        {
            _eventMessageId = EventMessageConstDefine.LeftEvent;
            Leftang = leftang;
        }
    }
}
