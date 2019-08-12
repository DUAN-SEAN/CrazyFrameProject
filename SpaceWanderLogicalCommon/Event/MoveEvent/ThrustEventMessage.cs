using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class ThrustEventMessage : BattleEventMessageBase
    {
        public double Thrustproc = 0.0001f;
        public ThrustEventMessage()
        {
            _eventMessageId = EventMessageConstDefine.ThrustEvent;
        }

        public ThrustEventMessage(double thrustproc)
        {
            _eventMessageId = EventMessageConstDefine.ThrustEvent;
            Thrustproc = thrustproc;
        }
    }
}
