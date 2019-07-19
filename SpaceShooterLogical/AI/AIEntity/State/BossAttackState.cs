using System;
using FSMSystemSpace;
using FSMTransition = FSMSystemSpace.Transition;
using FSMStateID = FSMSystemSpace.StateID;
using CrazyEngine;
using SpaceShip.Base;
using SpaceShip.Factory;

namespace SpaceShip.AI
{


    public class BossAttackState : FSMState
    {
        protected AICarrierShipInBody m_body;
        protected ShipBase attack_body;
        public BossAttackState(AIShipBase body)
        {
            stateID = (FSMStateID)AIShipStateID.BOSSATTACKSTATE;
            if (body is AICarrierShipInBody)
                m_body = body as AICarrierShipInBody;
        }


        public override void DoBeforeEntering()
        {
            //LogUI.Log(m_body.Id + "enter attack");

        }

        public override void DoBeforeEntering<T>(T t)
        {
            if (!(t is ShipBase body) || m_body == null)
            {
                m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.ALERT);

                return;
            }

            attack_body = body;

        }

        public override void DoingSomthing()
        {
            if (attack_body == null || m_body == null)
            {
                m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.ALERT);
                return;
            }
            //if (attack_body.shipinworld == null)
            //{
            //    m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.ALERT);
            //    return;
            //}
            Vector2 boltposition1 = m_body.GetShotPositionOne();
            Vector2 boltposition2 = m_body.GetShotPositionTwo();
            Vector2 missleposition1 = m_body.GetMisslePositionOne();
            Vector2 missleposition2 = m_body.GetMisslePositionTwo();


            Vector2 vector2 = attack_body.Position - m_body.Position;
            m_body.Forward = Vector2.Lerp(m_body.Forward, vector2, m_body.rotatespeed);

            // LogUI.Log(vector2.normalized.magnitudeNoSqrt);
            if (!m_body.isAttack)
            {
                //LogUI.Log("boltposition1:" + boltposition1);
                //LogUI.Log("boltposition2:" + boltposition2);

                //LogUI.Log("missleposition1:" + missleposition1);
                //LogUI.Log("missleposition2:" + missleposition2);
                //LogUI.Log("attack position: "+attack_body.Position);

                var weanpon1 = BodyFactory.Instance.LoadBoltWeaponByType<BoltInBody>((SeanD)m_body.iSBSean, m_body, boltposition1, attack_body.Position - boltposition1);
                m_body.iSBSean.GetBodyMessages().Add(new BodyInitMessage(BodyMessageID.Bolt, weanpon1));
                weanpon1 = BodyFactory.Instance.LoadBoltWeaponByType<BoltInBody>((SeanD)m_body.iSBSean, m_body, boltposition2, attack_body.Position - boltposition2);
                m_body.iSBSean.GetBodyMessages().Add(new BodyInitMessage(BodyMessageID.Bolt, weanpon1));
                var weanpon2 = BodyFactory.Instance.LoadMissileWeaponByType<MissileInBody>((SeanD)m_body.iSBSean, m_body, missleposition1, attack_body.Position - missleposition1);
                m_body.iSBSean.GetBodyMessages().Add(new BodyInitMessage(BodyMessageID.Missile, weanpon2));
                weanpon2 = BodyFactory.Instance.LoadMissileWeaponByType<MissileInBody>((SeanD)m_body.iSBSean, m_body, missleposition2, attack_body.Position - missleposition2);
                m_body.iSBSean.GetBodyMessages().Add(new BodyInitMessage(BodyMessageID.Missile, weanpon2));


                m_body.IsAttack = true;
            }

            // 目标太远 回到警戒状态

            if (Vector2.DistanceNoSqrt(m_body.Position, attack_body.Position) > m_body.followrange)
            {
                if (m_body.IsLeader) m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.FOLLOW);
                //else m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.TEAM);
            }

            //end
        }

        public override void DoBeforeLeaving()
        {

            //LogUI.Log(m_body.Id + "leaving attack");

        }
    }
}