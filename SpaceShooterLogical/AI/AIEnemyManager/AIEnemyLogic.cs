using System;
using System.Collections.Generic;
using SpaceShip.Base;
using SpaceShip.Factory;
using CrazyEngine;
namespace SpaceShip.AI
{


    public class AIEnemyLogic : ITickable,IAILog
    {
       
        public AIEnemyLogic()
        {
            m_aishipList = new List<AIShipBase>();
            m_enviromentList = new List<EnviromentInBody>();
            m_LeaderShipList = new List<AIShipBase>();
        }




       

        public void Tick()
        {
            for (int i = 0; i < m_aishipList.Count; i++)
            {
                m_aishipList[i].Tick();
            }

            //LogUI.Log(m_aishipList.Count);
            for (int i = 0; i < m_enviromentList.Count; i++)
            {
                m_enviromentList[i].Tick();
            }
            //LogUI.Log(m_LeaderShipList.Count);
        }


        #region 注册小飞机

        public void RegisterAIShip(AIShipBase shipBase)
        {
            if (!m_aishipList.Contains(shipBase))
                m_aishipList.Add(shipBase);
        }

        public void LogoutAIShip(AIShipBase shipBase)
        {
            //LogUI.Log("Ai Logout done");
            if (m_aishipList.Contains(shipBase))
                m_aishipList.Remove(shipBase);
            if (m_LeaderShipList.Contains(shipBase))
                m_LeaderShipList.Remove(shipBase);
           
        }

        #endregion

        #region 注册环境AI
        public void RegisterEnviroment(EnviromentInBody enviromentInBody)
        {
            
            m_enviromentList.Add(enviromentInBody);

        }
        public void LogoutEnviroment(EnviromentInBody enviromentInBody)
        {
            m_enviromentList.Remove(enviromentInBody);
        }
        #endregion


        #region 物体全体dispose
        public void Dispose()
        {
            AIShipDone();

            EnviromentDone();

            m_aishipList = null;
            m_enviromentList = null;
            m_LeaderShipList = null;
        }


        public void AIShipDone()
        {
            for (int i = 0; i < m_aishipList.Count; i++)
            {
                m_aishipList[i].Dispose();
            }
            //foreach(AIShipBase aIShipBase in m_aishipList)
            //{
            //    aIShipBase.Destroy();
            //}
            m_LeaderShipList.Clear();
            m_aishipList.Clear();
        }
        public void EnviromentDone()
        {

            for (int i = 0; i < m_enviromentList.Count; i++)
            {
                m_enviromentList[i].Dispose();
            }


            m_enviromentList.Clear();

        }

        public List<AIShipBase> getLeaderShips()
        {
            return m_LeaderShipList;
        }
        #endregion



        public bool isAiStop;

        public List<AIShipBase> m_LeaderShipList;

        public List<EnviromentInBody> m_enviromentList;
        public List<AIShipBase> m_aishipList;
       

    }
    
}