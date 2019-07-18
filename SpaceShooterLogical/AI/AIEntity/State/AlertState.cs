using FSMSystemSpace;
using FSMTransition = FSMSystemSpace.Transition;
using FSMStateID = FSMSystemSpace.StateID;
using CrazyEngine;

namespace SpaceShip.AI
{


    public class AlertState : FSMState
    {
        protected AIShipBase m_body;
        public AlertState(AIShipBase shipBase)
        {
            stateID = (FSMStateID)AIShipStateID.ALERTSTATE;
            m_body = shipBase;
        }

        public override void DoBeforeEntering()
        {
            //LogUI.Log(m_body.Id + "enter alert");
            //if (m_body.teamershiplist.Count != 0)
            //{
            //    LogUI.Log("follow leader " + m_body.teamershiplist.Count);
            //    for (int i = 0; i < m_body.teamershiplist.Count; i++)
            //    {
            //        AIShipBase aIShipBase = m_body.teamershiplist[i];

            //        aIShipBase.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.FOLLOW, m_body);
            //    }
            //}
        }

        public override void DoingSomthing()
        {
            //TODO 警戒周围的事情 


            foreach (Body body in m_body.iSBSean.GetWorld().GetCurrentWorld().Bodies)
            {
                if (body.Label.HasFlag(m_body.Label) || body.Label.HasFlag(Label.WEAPON)) continue;
                if (body.Label.HasFlag(Label.Environment)) continue;
                if (Vector2.DistanceNoSqrt(body.Position, m_body.Position) < m_body.alertrange)
                {
                    m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.FOLLOW, body);
                    return;
                }

            }

            //if(!m_body.IsLeader)
            //    m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.TEAM);


            //end


        }

        //TODO 检测结果
        public bool TickResult()
        {
            return false;
        }

        public override void DoBeforeLeaving()
        {
            //LogUI.Log(m_body.Id + "leavting alert");

        }
    }
}