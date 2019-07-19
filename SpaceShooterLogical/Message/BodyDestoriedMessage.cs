using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine;
namespace SpaceShip.Base
{
    public class BodyDestoriedMessage : BodyMessage
    {
        public BodyDestoriedMessage(Body body) : base(body)
        {

        }

        public override int GetMessageType()
        {
            return MessageType.BodyDestoried;
        }
    }
}
