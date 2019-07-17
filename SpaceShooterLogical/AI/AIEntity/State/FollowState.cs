using System;
using FSMSystemSpace;
using FSMTransition = FSMSystemSpace.Transition;
using FSMStateID = FSMSystemSpace.StateID;

namespace SpaceShip.AI
{


    public class FollowState : FSMState, IFollow
    {
        protected AIShipBase m_body;
        protected ShipBase follow_body;
        protected Vector2 offset_vector;
        public FollowState(AIShipBase body)
        {
            stateID = (FSMStateID)AIShipStateID.FOLLOWSTATE;
            m_body = body;
        }


        public FollowState(AIShipBase body, AIShipBase follow)
        {
            stateID = (FSMStateID)AIShipStateID.FOLLOWSTATE;
            m_body = body;
            follow_body = follow;
        }
        public override void DoBeforeEntering()
        {
            if (follow_body == null)
            {
                LogUI.Log("follow fail");

                m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.ALERT);
            }
            LogUI.Log(m_body.Id + "enter follow");

        }
        public override void DoBeforeEntering<T>(T t)
        {
            if (!(t is ShipBase body))
            {
                LogUI.Log("follow fail" + t);

                m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.ALERT);
                return;
            }
            LogUI.Log(m_body.Id + "enter follow1");

            follow_body = body;
        }
        public override void DoBeforeEntering<T>(T t1, Vector2 t2)
        {
            if (!(t1 is ShipBase body))
            {
                LogUI.Log("follow fail" + t1);
                m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.ALERT);
                return;
            }
            LogUI.Log(m_body.Id + "enter follow1");

            offset_vector = t2;

            follow_body = body;
        }
        public override void DoBeforeLeaving()
        {
            LogUI.Log(m_body.Id + "leaving follow");
        }

        public override void DoingSomthing()
        {
            if (follow_body == null)
            {
                m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.ALERT);

                return;
            }
            //目标对象接近
            var forward = m_body.Forward.normalized;
            float sin = forward.Sin;
            float cos = forward.Cos;
            //LogUI.Log("sin:" + sin + " cos:" + cos);
            //LogUI.Log("offset_vector" + offset_vector);
            var offset = new Vector2(cos * offset_vector.x - sin * offset_vector.y, cos * offset_vector.x + sin * offset_vector.y);
            //LogUI.Log(offset);
            m_body.Position = Vector2.Lerp(m_body.Position, follow_body.Position + offset, m_body.movespeed);
            Vector2 vector2 = follow_body.Position - m_body.Position;
            m_body.Forward = Vector2.Lerp(m_body.Forward, vector2, m_body.rotatespeed);
            //判断结果
            TickResult();
        }


        public Body GetBody()
        {
            return m_body;
        }



        public Vector2 GetFollowPoint()
        {
            return follow_body.Position;
        }

        public bool TickResult()
        {
            //if (!m_body.IsLeader) return true;

            if (!follow_body.Label.HasFlag(m_body.Label))
            {
                float distance = Vector2.DistanceNoSqrt(m_body.Position, follow_body.Position);
                if (distance > m_body.alertrange)
                {
                    if (m_body.IsLeader)
                        m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.ALERT);
                    else m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.TEAM);
                    return true;
                }

                if (distance < m_body.followrange)
                {

                    //if(m_body.teamershiplist.Count != 0 )
                    //{
                    //    LogUI.Log("attatck team " + m_body.teamershiplist.Count);
                    //    for (int i = 0; i < m_body.teamershiplist.Count; i++)
                    //    {
                    //        AIShipBase aIShipBase = m_body.teamershiplist[i];

                    //       aIShipBase.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.ATTACK, follow_body);
                    //    }
                    //}

                    m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.ATTACK, follow_body);
                    return true;
                }
            }
            else
            {
                if (m_body.Leadership.m_fsmsystem.CurrentStateID == (FSMStateID)AIShipStateID.ATTACKSTATE)
                {
                    m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.FOLLOW, ((AttackState)m_body.Leadership.m_fsmsystem.CurrentState).attack_body);


                }
            }

            return false;
        }
    }
}