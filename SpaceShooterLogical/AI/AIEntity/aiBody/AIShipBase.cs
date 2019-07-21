using System;
using FSMSystemSpace;
using FSMTransition = FSMSystemSpace.Transition;
using FSMStateID = FSMSystemSpace.StateID;
using System.Collections.Generic;
using CrazyEngine;
using SpaceShip.Base;
using SpaceShip.Factory;


namespace SpaceShip.AI
{


/// <summary>
/// 自动飞机
/// </summary>
[Serializable]
abstract public class AIShipBase : ShipBase, ITickable
{
    protected AIShipBase()
    {
        //LogUI.Log("Aiship ctor");
        

    }



    /// <summary>
    /// 将关卡ai小飞机数据加载到关卡集合中
    /// 供以后生成用
    /// 游戏还未正式开始
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    protected AIShipBase(Vector2 min, Vector2 max) : base(min, max)
    {


    }

    public override void Init(Level seanD)
    {
            base.InitWorld(seanD);
            seanD.GetAILog().RegisterAIShip(this);


           
            teamershiplist = new List<AIShipBase>();

            movespeed = 0.1f;
            rotatespeed = 0.1f;
            alertrange = 1000;
            followrange = 400;

            HP = 1;
            Armor = 0;
            maxArmor = 0;
            IntervalTime = 5;

            InitFsm();

            //isAttack = true;
        }

        protected virtual void InitFsm()
        {
            m_fsmsystem = new FSMSystem();
            FollowState followState = new FollowState(this);
            followState.AddTransition((FSMTransition)AIShipTransition.ATTACK,
                (FSMStateID)AIShipStateID.ATTACKSTATE);
            followState.AddTransition((FSMTransition)AIShipTransition.ALERT,
                (FSMStateID)AIShipStateID.ALERTSTATE);
            followState.AddTransition((FSMTransition)AIShipTransition.FOLLOW,
                (FSMStateID)AIShipStateID.FOLLOWSTATE);
            followState.AddTransition((FSMTransition)AIShipTransition.TEAM,
                (FSMStateID)AIShipStateID.TEAMSTATE);

            AttackState attackState = new AttackState(this);
            attackState.AddTransition((FSMTransition)AIShipTransition.ALERT,
                (FSMStateID)AIShipStateID.ALERTSTATE);
            attackState.AddTransition((FSMTransition)AIShipTransition.FOLLOW,
                (FSMStateID)AIShipStateID.FOLLOWSTATE);
            attackState.AddTransition((FSMTransition)AIShipTransition.TEAM,
                (FSMStateID)AIShipStateID.TEAMSTATE);

            AlertState alertState = new AlertState(this);
            alertState.AddTransition((FSMTransition)AIShipTransition.FOLLOW,
                (FSMStateID)AIShipStateID.FOLLOWSTATE);
            alertState.AddTransition((FSMTransition)AIShipTransition.TEAM,
             (FSMStateID)AIShipStateID.TEAMSTATE);

            TeamState teamState = new TeamState(this);
            teamState.AddTransition((FSMTransition)AIShipTransition.ALERT,
                (FSMStateID)AIShipStateID.ALERTSTATE);
            teamState.AddTransition((FSMTransition)AIShipTransition.ATTACK,
                (FSMStateID)AIShipStateID.ATTACKSTATE);
            teamState.AddTransition((FSMTransition)AIShipTransition.FOLLOW,
                (FSMStateID)AIShipStateID.FOLLOWSTATE);

            m_fsmsystem.AddState(teamState);
            m_fsmsystem.AddState(alertState);
            m_fsmsystem.AddState(followState);
            m_fsmsystem.AddState(attackState);

        }

        public FSMSystem m_fsmsystem;

    public void Tick()
    {
        //如果Body未激活 则不运行任何逻辑
        if (!Enable) return;

        Tickthing();
    }

    public virtual void Tickthing()
    {
        currenttime = DateTime.Now.Ticks;

        //控制自己的小队队员位置
        switch (m_fsmsystem.CurrentStateID)
        {
            //case (FSMStateID)AIShipStateID.FOLLOWSTATE:
            //    for (int i = 0; i < teamershiplist.Count && i < 4; i++)
            //    {
            //        AIShipBase aIShipBase = teamershiplist[i];
            //        if (aIShipBase.m_fsmsystem.CurrentStateID == (FSMStateID)AIShipStateID.TEAMSTATE)
            //            aIShipBase.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.FOLLOW,this, teamerposition[i]);


            //    }
            //    break;
            case (FSMStateID)AIShipStateID.ATTACKSTATE:

                for (int i = 0; i < teamershiplist.Count && i < 4; i++)
                {
                    AIShipBase aIShipBase = teamershiplist[i];

                    if (aIShipBase.m_fsmsystem.CurrentStateID != (FSMStateID)AIShipStateID.ATTACKSTATE)
                    {
                        aIShipBase.m_fsmsystem.PerformTransition((FSMTransition)AIShipTransition.FOLLOW, ((AttackState)m_fsmsystem.CurrentState).attack_body);

                    }


                }
                break;
        }





        if (currenttime - oldtime > 10000000)
        {
            if (isAttack == true)
            {
                isAttack = false;
                oldtime = DateTime.Now.Ticks;
            }

        }
        m_fsmsystem.TickState();
    }
    public override void Dispose()
    {
        base.Dispose();
       
            iSBSean.GetAILog().LogoutAIShip(this);
           //LogUI.Log("logout ai");
        

    }

    public bool IsAttack
    {
        set
        {
            isAttack = value;
            oldtime = DateTime.Now.Ticks;
        }
    }

    public Vector2 GetTeamerPosition(AIShipBase aIShipBase)
    {
        return teamerposition[teamershiplist.IndexOf(aIShipBase)];
    }

    public AIShipBase Leadership;
    public bool IsLeader;
    public Vector2[] teamerposition = { new Vector2(-1, -1), new Vector2(1, -1), new Vector2(-2, -2), new Vector2(2, -2) };
    public List<AIShipBase> teamershiplist;

    //判断是否主动攻击过
    public bool isAttack;

    public float movespeed;
    public float rotatespeed;

    public float followrange;
    public float alertrange;


    private long oldtime;
    private long currenttime;
}


public enum AIShipStateID
{
    FOLLOWSTATE = 1,
    ATTACKSTATE = 2,
    ALERTSTATE = 3,
    TEAMSTATE = 4,
    BOSSATTACKSTATE = 5

}

public enum AIShipTransition
{
    ATTACK = 1,
    FOLLOW = 2,
    ALERT = 3,
    TEAM = 4,
    BOSSATTACK = 5

}
}



