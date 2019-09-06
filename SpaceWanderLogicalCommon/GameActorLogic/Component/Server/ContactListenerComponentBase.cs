using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Dynamics.Contacts;
using Crazy.Common;

namespace GameActorLogic
{
    public class ContactListenerComponentBase: IContactListenerComponentBase
    {
        IEnvirinfoInternalBase envir;
        public ContactListenerComponentBase(IEnvirinfoInternalBase envir)
        {
            this.envir = envir;
            envir.SetContactListener(this);
        }

        public void BeginContact(Contact contact)
        {
            Log.Trace("BeginContact contact" + contact);
            UserData userdateA = contact.FixtureA.Body.UserData as UserData;
            UserData userdateB = contact.FixtureB.Body.UserData as UserData;
            Log.Trace("BeginContact UserDataA:" + userdateA + "    UserDataB" + userdateB);
            if(userdateA != null)
            {
                var actor = envir.GetActor(userdateA.ActorID);
                actor.OnContactEnter(userdateB);
            }
            if (userdateB != null)
            {
                var actor = envir.GetActor(userdateB.ActorID);
                actor.OnContactEnter(userdateA);
            }

        }

        public void EndContact(Contact contact)
        {
            Log.Trace("EndContact contact" + contact);

            UserData userdateA = contact.FixtureA.Body.UserData as UserData;
            UserData userdateB = contact.FixtureB.Body.UserData as UserData;
            Log.Trace("EndContact UserDataA:" + userdateA + "    UserDataB" + userdateB);

            if (userdateA != null)
            {
                var actor = envir.GetActor(userdateA.ActorID);
                actor.OnContactExit(userdateB);
            }
            if (userdateB != null)
            {
                var actor = envir.GetActor(userdateB.ActorID);
                actor.OnContactExit(userdateA);
            }
        }

        public void PostSolve(Contact contact, in ContactImpulse impulse)
        {

        }

        public void PreSolve(Contact contact, in Manifold oldManifold)
        {

        }
    }
}
