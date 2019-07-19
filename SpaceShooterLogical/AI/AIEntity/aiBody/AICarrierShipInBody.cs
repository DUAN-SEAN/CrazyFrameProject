using System;
using FSMSystemSpace;
using FSMTransition = FSMSystemSpace.Transition;
using FSMStateID = FSMSystemSpace.StateID;
using System.Collections.Generic;
using SpaceShip.Base;
using CrazyEngine;
using SpaceShip.Factory;
namespace SpaceShip.AI
{

    public class AICarrierShipInBody : AIShipBase, ITickable
    {
        public AICarrierShipInBody()
        {
            //LogUI.Log("carrier ctor");
            IsLeader = true;
            m_fsmsystem = new FSMSystem();
            FollowState followState = new FollowState(this);
            followState.AddTransition((FSMTransition)AIShipTransition.ATTACK,
                (FSMStateID)AIShipStateID.BOSSATTACKSTATE);
            followState.AddTransition((FSMTransition)AIShipTransition.ALERT,
                (FSMStateID)AIShipStateID.ALERTSTATE);

            //做新的大飞船攻击attack
            BossAttackState attackState = new BossAttackState(this);
            attackState.AddTransition((FSMTransition)AIShipTransition.ALERT,
                (FSMStateID)AIShipStateID.ALERTSTATE);
            attackState.AddTransition((FSMTransition)AIShipTransition.FOLLOW,
                (FSMStateID)AIShipStateID.FOLLOWSTATE);
            //attackState.AddTransition((FSMTransition)AIShipTransition.TEAM,
            //(FSMStateID)AIShipStateID.TEAMSTATE);

            AlertState alertState = new AlertState(this);
            alertState.AddTransition((FSMTransition)AIShipTransition.FOLLOW,
                (FSMStateID)AIShipStateID.FOLLOWSTATE);

            //TeamState teamState = new TeamState(this);
            //teamState.AddTransition((FSMTransition)AIShipTransition.ALERT,
            //    (FSMStateID)AIShipStateID.ALERTSTATE);
            //teamState.AddTransition((FSMTransition)AIShipTransition.ATTACK,
            //    (FSMStateID)AIShipStateID.BOSSATTACKSTATE);

            //m_fsmsystem.AddState(teamState);
            m_fsmsystem.AddState(alertState);
            m_fsmsystem.AddState(followState);
            m_fsmsystem.AddState(attackState);

            movespeed = 0.001f;
            rotatespeed = 0.001f;
            alertrange = 6000;
            followrange = 4000;
            maxSmallShip = 20;

            HP = 10;
            Armor = 10;
            maxArmor = 10;
            IntervalTime = 5;
            oldtime = DateTime.Now.Ticks;
            //isAttack = true;
            teamershiplist = new List<AIShipBase>();
        }

        public AICarrierShipInBody(Vector2 min, Vector2 max) : base(min, max)
        {


        }

        



        #region 确定大飞机特殊位置 武器位置等
        public Vector2 GetShotPositionOne()
        {
            //var body = shipinworld as CarrierShipInWorld;
            //return new Vector2(body.shotSpawnOne.position.x, body.shotSpawnOne.position.z);
            return Position;
        }
        public Vector2 GetShotPositionTwo()
        {
            //var body = shipinworld as CarrierShipInWorld;
            //return new Vector2(body.shotSpawnTwo.position.x, body.shotSpawnTwo.position.z);
            return Position;

        }
        public Vector2 GetMisslePositionOne()
        {
            //var body = shipinworld as CarrierShipInWorld;
            //return new Vector2(body.missleSpawnOne.position.x, body.missleSpawnOne.position.z);
            return Position;

        }
        public Vector2 GetMisslePositionTwo()
        {
            //var body = shipinworld as CarrierShipInWorld;
            //return new Vector2(body.missleSpawnTwo.position.x, body.missleSpawnTwo.position.z);
            return Position;

        }
        public Vector2 GetShipOutPositionOne()
        {
            //var body = shipinworld as CarrierShipInWorld;
            //return new Vector2(body.aircraftSpawnOne.position.x, body.aircraftSpawnOne.position.z);
            return Position;

        }
        public Vector2 GetShipOutPositionTwo()
        {
            //var body = shipinworld as CarrierShipInWorld;
            //return new Vector2(body.aircraftSpawnTwo.position.x, body.aircraftSpawnTwo.position.z);
            return Position;

        }
        #endregion

        public override void Tickthing()
        {



            currenttime = DateTime.Now.Ticks;


            //控制自己的小队队员位置

            for (int i = 0; i < teamershiplist.Count; i++)
            {
                AIShipBase aIShipBase = teamershiplist[i];
                Vector2 posi = Position;
                Vector2 forward = Forward;
                switch (i)
                {
                    case 0:
                        posi = new Vector2(posi.x - 1, posi.y - 1);

                        break;
                    case 1:
                        posi = new Vector2(posi.x - 2, posi.y - 2);
                        break;
                    case 2:
                        posi = new Vector2(posi.x + 1, posi.y - 1);
                        break;
                    case 3:
                        posi = new Vector2(posi.x + 2, posi.y - 2);
                        break;
                }
                aIShipBase.Position = posi;
                aIShipBase.Forward = forward;
            }


            //LogUI.Log("carrier tick" + teamershiplist.Count);


            if (currenttime - oldtime > 10000000)
            {
                if (isAttack == true)
                {
                    isAttack = false;
                    oldtime = DateTime.Now.Ticks;
                }


            }
            if (currenttime - oldtimeinit > 10000000)
            {
                //LogUI.Log("ships init");

                if (initableShip > 0 && initShips < maxSmallShip)
                {
                    var body = BodyFactory.Instance.LoadShipBodyByType<AISmallShipInBody>((Level)iSBSean,Label, GetShipOutPositionOne(), GetShipOutPositionOne() + new Vector2(1, 1), Forward);
                    iSBSean.GetBodyMessages().Add(new BodyMessage(BodyMessageID.SmallShip1, body));
                    initShips++;
                    initableShip--;
                }
                if (initableShip > 0 && initShips < maxSmallShip)
                {
                    var body = BodyFactory.Instance.LoadShipBodyByType<AISmallShipInBody>((Level)iSBSean,Label, GetShipOutPositionTwo(), GetShipOutPositionTwo() + new Vector2(1, 1), Forward);
                    iSBSean.GetBodyMessages().Add(new BodyMessage(BodyMessageID.SmallShip1, body));

                    initShips++;
                    initableShip--;
                }

                oldtimeinit = DateTime.Now.Ticks;
            }
            if (currenttime - oldtimenew > 50000000)
            {
                //LogUI.Log("ships init add");


                if (initableShip < maxSmallShip)
                    initableShip++;
                oldtimenew = DateTime.Now.Ticks;
            }
            m_fsmsystem.TickState();
        }
        /// <summary>
        /// 自动回复护盾值
        /// </summary>
        private void AutoAddArmor()
        {
            if (Armor == maxArmor || HP <= 0)
                _oldTime = DateTime.Now.Ticks;

            if (currenttime - _oldTime >= 50000000)
            {
                if (Armor < maxArmor)
                    Armor++;
                _oldTime = DateTime.Now.Ticks;

            }
            _oldTime = DateTime.Now.Ticks;
        }


        public override void Dispose()
        {
            //LogUI.Log("carrier dispose");
            base.Dispose();

        }

        private long _oldTime;
        public int initableShip;
        public int initShips;
        public int maxSmallShip;

        private long oldtime;

        private long oldtimenew;
        private long oldtimeinit;
        private long currenttime;
    }

}