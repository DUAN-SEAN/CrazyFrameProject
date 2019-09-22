using Box2DSharp.External;
using Crazy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class FollowAiComponent : AIComponentBase
    {
        protected ILevelActorComponentBaseContainer level;
        protected new IWeaponBaseComponentContainer container;

        /// <summary>
        /// 逻辑每1ms执行一次
        /// </summary>
        protected long lastlogicalframe;
        protected long delyframe = 5000;

        /// <summary>
        /// 开火逻辑 每
        /// </summary>
        protected long lastfireframe;
        protected long delyfireframe = 1000000 * 2;
        /// <summary>
        /// AI施力
        /// </summary>
        protected float AIForce = 1000;
        /// <summary>
        /// AI施转矩
        /// </summary>
        protected float AITorque = 2;

        /// <summary>
        /// ai索敌范围
        /// </summary>
        protected float AIdis;

        public FollowAiComponent(ILevelActorComponentBaseContainer level, IWeaponBaseComponentContainer container,float force,float torque,float dis) : base(container)
        {
            this.level = level;

            this.container = container;

            this.AIForce = force;
            this.AITorque = torque;

            this.AIdis = dis;
        }

        public FollowAiComponent(FollowAiComponent clone, IWeaponBaseComponentContainer container) : base(clone, container)
        {
            //container.GetPhysicalinternalBase().GetBody().Velocity = container.GetForward() * 10;
            this.level = container.GetLevel();
            this.container = container;
            this.AIForce = clone.AIForce;
            this.AITorque = clone.AITorque;
            this.AIdis = clone.AIdis;
        }

        public override AIComponentBase Clone(IBaseComponentContainer container)
        {
            return new FollowAiComponent(this, container as IWeaponBaseComponentContainer);
        }


        int i = 0;
        public override void TickLogical()
        {

            //if (i == 0)
            //{
            //    //Log.Trace("ShipEnemyAiComponent 开始AI逻辑");
            //    container.AddThrust(AIForce);
            //    i = 1;
            //}
            //Log.Trace("ShipEnemyAiComponent 正在运行AI逻辑");

            if (DateTime.Now.Ticks - lastlogicalframe > delyframe)
            {
                //Tick 敌人是否在附近
                var list = container.GetPhysicalinternalBase().GetBody()
                    .CircleDetection(level.GetEnvirinfointernalBase().GetShipActorsByCamp(container.GetCamp()).ToBodyList(), AIdis);
                //Log.Trace("ShipEnemyAiComponent 附近敌人数量" + list.Count);
                if (list.Count <= 0) return;
                //这个list 里面有敌人
                //Log.Trace("ShipEnemyAiComponent 发现敌人" + list.Count);


                //朝向跟随敌人
                //container.GetPhysicalinternalBase().GetBody().SomeAreaFowardToTarget(list[0].GetPosition(), AITorque);

                //追击敌人
                container.GetPhysicalinternalBase().GetBody().FollowTarget(list[0].GetPosition(), AIForce, AITorque);

                lastlogicalframe = DateTime.Now.Ticks;
            }

            

        }
    }
}
