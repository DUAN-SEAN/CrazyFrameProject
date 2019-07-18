using System;
using FSMSystemSpace;
using FSMTransition = FSMSystemSpace.Transition;
using FSMStateID = FSMSystemSpace.StateID;
using SpaceShip.Factory;
using CrazyEngine;
using SpaceShip.Base;

namespace SpaceShip.AI
{


    public class AttackState : FSMState
    {
        protected AIShipBase m_body;
        public ShipBase attack_body;
        public AttackState(AIShipBase body)
        {
            stateID = (FSMStateID)AIShipStateID.ATTACKSTATE;
            m_body = body;
        }


        public override void DoBeforeEntering()
        {
            //LogUI.Log(m_body.Id + "enter attack");

        }

        public override void DoBeforeEntering<T>(T t)
        {
            if (!(t is ShipBase body))
            {
                m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.ALERT);

                return;
            }

            attack_body = body;

        }

        public override void DoingSomthing()
        {
            if (attack_body == null)
            {
                m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.ALERT);
                return;
            }
            //if (attack_body.shipinworld == null)
            //{
            //    m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.ALERT);
            //    return;
            //}
            Vector2 vector2 = attack_body.Position - m_body.Position;
            m_body.Forward = Vector2.Lerp(m_body.Forward, vector2, m_body.rotatespeed);

            // LogUI.Log(vector2.normalized.magnitudeNoSqrt);
            if (!m_body.isAttack && vector2.normalized.magnitudeNoSqrt < 1.001)
            {
                var weanpon = BodyFactory.Instance.LoadBoltWeaponByType<BoltInBody>((SeanD)m_body.iSBSean, m_body);
                m_body.iSBSean.GetBodyMessages().Add(new BodyMessage(BodyMessageID.Bolt, weanpon));
                m_body.IsAttack = true;
            }

            // 目标太远 回到警戒状态

            if (Vector2.DistanceNoSqrt(m_body.Position, attack_body.Position) > m_body.followrange)
            {
                if (m_body.IsLeader) m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.FOLLOW);
                else m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.TEAM);
            }

            //end
        }

        public override void DoBeforeLeaving()
        {

            //LogUI.Log(m_body.Id + "leaving attack");

        }


    }
}