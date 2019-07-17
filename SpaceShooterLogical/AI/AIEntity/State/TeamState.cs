using System;
using FSMSystemSpace;
using FSMTransition = FSMSystemSpace.Transition;
using FSMStateID = FSMSystemSpace.StateID;

public class TeamState : FSMState
{
	protected AIShipBase m_body;
   
    public TeamState()
    {
    }
	public TeamState(AIShipBase body)
	{
		stateID = (FSMStateID)AIShipStateID.TEAMSTATE;
		m_body = body;
        
        
        
	}

	public override void DoBeforeEntering()
	{
		LogUI.Log(m_body.Id+"enter Teamstate");
        if (m_body.IsLeader)
        {
            if (!AIEnemyLogic.Instance.LeaderShips.Contains(m_body))
                AIEnemyLogic.Instance.LeaderShips.Add(m_body);

            return;
        }
        if (m_body.Leadership != null)
        {
            LogUI.Log("leadership not null");
            //if(m_body.Leadership.teamershiplist)
            //m_body.Leadership.teamershiplist.Add(m_body);
            return;
        }
        if (AIEnemyLogic.Instance.LeaderShips.Count == 0)
        {
            LogUI.Log("leaderships count 0");

            AIEnemyLogic.Instance.LeaderShips.Add(m_body);
            m_body.IsLeader = true;
            return;
        }
        foreach(AIShipBase aIShipBase in AIEnemyLogic.Instance.LeaderShips)
        {
            if (aIShipBase.teamershiplist.Count == 4) continue;
            LogUI.Log("aiteamers:" + aIShipBase.teamershiplist.Count);
            aIShipBase.teamershiplist.Add(m_body);
            m_body.Leadership = aIShipBase;
            
            break;
        }
        if (m_body.Leadership == null)
        {
            LogUI.Log("Leadership set self");

            AIEnemyLogic.Instance.LeaderShips.Add(m_body);
            m_body.IsLeader = true;
            return;
        }
    }

    public override void DoBeforeLeaving()
    {
        LogUI.Log(m_body.Id+"leaving Teamstate");
        //if (m_body.Leadership != null) m_body.Leadership.teamershiplist.Remove(m_body);
        //m_body.IsLeader = false;
        //m_body.Leadership = null;
        //m_body.teamershiplist.Clear();
    }

    public override void DoingSomthing()
    {
        //将自己的移动交给AI队长 由队长实体控制（AIShipBase）
        //LogUI.Log("teamstate"+m_body.IsLeader);
        if (!m_body.IsLeader)
        {
            m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.FOLLOW, m_body.Leadership, m_body.Leadership.GetTeamerPosition(m_body));
            return;
        }
        LogUI.Log(m_body.Id+"leader go alert");

        //队长转到警戒状态
        m_body.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.ALERT);
    }

}
