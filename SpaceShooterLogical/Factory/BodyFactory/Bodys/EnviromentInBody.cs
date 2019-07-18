using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine;
using SpaceShip.Base;

namespace SpaceShip.Factory
{
    using System;
    public class EnviromentInBody : CircleEntity, ITickable
    {
        public EnviromentInBody()
        {
            AIEnemyLogic.Instance.RegisterEnviroment(this);
        }

        public EnviromentInBody(Vector2 vector, float radius) : base(vector, radius)
        {
            //AIEnemyManager.Instance.RegisterEnviroment(this);
        }

        public void Tick()
        {
            //LogUI.Log(enviromentinworld);
        }

        public override void Dispose()
        {
            //LogUI.Log("label" + Label);
            //if (enviromentinworld == null)
            //{
            //    LogUI.Log("enviroment null");
            //}
            //else
            //    enviromentinworld.Destroy();

            base.Dispose();
            if (AIEnemyLogic.Instance.m_enviromentList.Contains(this))
                AIEnemyLogic.Instance.LogoutEnviroment(this);
        }
    }

}
