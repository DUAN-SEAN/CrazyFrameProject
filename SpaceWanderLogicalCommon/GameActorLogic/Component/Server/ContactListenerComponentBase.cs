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
            //Log.Trace("BeginContact contact" + contact);
            //Log.Trace("BeginContact userdataA" + contact.FixtureA.Body.UserData);
            //Log.Trace("BeginContact userdataB" + contact.FixtureB.Body.UserData);
            UserData userdateA = contact.FixtureA.Body.UserData as UserData;
            UserData userdateB = contact.FixtureB.Body.UserData as UserData;
            //Log.Trace("BeginContact UserDataA:" + userdateA + "    UserDataB" + userdateB);

            ActorBase actorA = null;
            ActorBase actorB = null;


            if (userdateA != null)
            {
                actorA = envir.GetActor(userdateA.ActorID);
                if (actorA.GetContactEnterFlag()) actorA = null;
            }
            if (userdateB != null)
            {
               actorB = envir.GetActor(userdateB.ActorID);
                if (actorB.GetContactEnterFlag()) actorB = null;
            }

            if(actorA != null)
            {
                actorA.OnContactEnter(userdateB);
            }
            if (actorB != null)
            {
                actorB.OnContactEnter(userdateA);
            }

        }

        public void EndContact(Contact contact)
        {
            //Log.Trace("EndContact contact" + contact);

            UserData userdateA = contact.FixtureA.Body.UserData as UserData;
            UserData userdateB = contact.FixtureB.Body.UserData as UserData;
            //Log.Trace("EndContact UserDataA:" + userdateA + "    UserDataB" + userdateB);

            ActorBase actorA = null;
            ActorBase actorB = null;


            if (userdateA != null)
            {
                actorA = envir.GetActor(userdateA.ActorID);
                if (actorA.GetContactExitFlag()) actorA = null;
            }
            if (userdateB != null)
            {
                actorB = envir.GetActor(userdateB.ActorID);
                if (actorB.GetContactExitFlag()) actorB = null;
            }

            if (actorA != null)
            {
                actorA.OnContactExit(userdateB);
            }
            if (actorB != null)
            {
                actorB.OnContactExit(userdateA);
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
