using CrazyEngine;
namespace SpaceShip.Base
{
    public interface IBodyMessage
    {
        int GetBodyID();

        int GetMessageType();

        Body GetBody();
    }

    public abstract class BodyMessage : IBodyMessage
    {
        protected Body m_body;

        public BodyMessage(Body body)
        {
            m_body = body;
        }

        public Body GetBody()
        {
            return m_body;
        }

        public int GetBodyID()
        {
            return m_body.Id.Value;
        }

        public abstract int GetMessageType();
    }

    public class BodyInitMessage :BodyMessage, IBodyMessage
    {
       
        

        public BodyInitMessage(Body body) : base(body)
        {
           
           
        }

        public override int GetMessageType()
        {
            return MessageType.BodyInit;
        }
    }


    


   



    public class MessageType
    {
        public const int BodyInit = 1;
        public const int BodyAttack = 2;
        public const int BodyAttacked = 3;
        public const int BodyDestoried = 4;

    }
}
