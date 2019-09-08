
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using System.Numerics;
using Box2DSharp.External;

namespace GameActorLogic
{
    public class ShipEnemyAiComponent : AIComponentBase
    {
        protected ILevelActorComponentBaseContainer level;
        protected new IShipComponentBaseContainer container;

        /// <summary>
        /// 逻辑每1ms执行一次
        /// </summary>
        protected long lastlogicalframe;
        protected long delyframe = 1000000;

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
        public ShipEnemyAiComponent(ILevelActorComponentBaseContainer level, IShipComponentBaseContainer container) : base(container)
        {
            this.level = level;

            this.container = container;
        }

        public ShipEnemyAiComponent(ShipEnemyAiComponent clone, IShipComponentBaseContainer container) : base(clone, container)
        {
            //container.GetPhysicalinternalBase().GetBody().Velocity = container.GetForward() * 10;
            this.level = container.GetLevel();
            this.container = container;
        }

        public override AIComponentBase Clone(IBaseComponentContainer container)
        {
            return new ShipEnemyAiComponent(this,container as IShipComponentBaseContainer);
        }


        //int i = 0;
        public override void TickLogical()
        {

            //if(i == 0)
            //{
            //    Log.Trace("ShipEnemyAiComponent 开始AI逻辑");
            //    i = 1;
            //}
            //Log.Trace("ShipEnemyAiComponent 正在运行AI逻辑");

            if (DateTime.Now.Ticks - lastlogicalframe > delyframe)
            {
                //Tick 敌人是否在附近
                var list = container.GetPhysicalinternalBase().GetBody()
                    .CircleDetection(level.GetEnvirinfointernalBase().GetPlayerActors().ToBodyList(), 1000);
                //Log.Trace("ShipEnemyAiComponent 附近敌人数量" + list.Count);
                if (list.Count <= 0) return;
                //这个list 里面有敌人
                //Log.Trace("ShipEnemyAiComponent 发现敌人" + list.Count);

                //追击敌人
                //container.GetPhysicalinternalBase().GetBody().FollowTarget(list[0].Position);

                //判断进入射程
                //距离小于最远射程是为了保证在移动中有更多打到敌人的机会
                bool distanceinrange = container.GetPhysicalinternalBase().GetBody()
                    .DistanceDetection(list[0].GetPosition(), 90);
                Vector2 p = new Vector2();
                if (distanceinrange)
                {
                    //Log.Trace("ShipEnemyAiComponent 敌人小于90" + list.Count);

                    var body = container.GetPhysicalinternalBase().GetBody();

                    //自动转向接口
                    if (body.FowardToTarget(list[0].GetPosition(),2) > 0.98f)
                    {
                        if(DateTime.Now.Ticks - lastfireframe > delyfireframe * 2)
                        {
                            //攻击
                            container.GetFireControlinternalBase().FireAI(0);
                            lastfireframe = DateTime.Now.Ticks;
                        }

                        //Log.Trace("ShipEnemyAiComponent 攻击敌人" + list.Count);

                    }

                }
                else
                {
                    //Log.Trace("ShipEnemyAiComponent 敌人大于90" + list.Count);

                    var body = container.GetPhysicalinternalBase().GetBody();

                   

                    //环绕
                    Double deltaX = list[0].GetPosition().X - body.GetPosition().X;
                    Double deltaY = list[0].GetPosition().Y - body.GetPosition().Y;

                    //当前方位在第一象限，确定环绕的目标地点
                    if (deltaX >= 0 && deltaY >= 0)
                    {
                        //α=90
                        if (deltaX == 0)
                        {
                            EncirclePosition_3();
                        }

                        //α=0
                        if (deltaY == 0)
                        {
                            EncirclePosition_1();
                        }

                        //α=(0,45)
                        if (deltaY / deltaX > 0 && deltaY / deltaX < 1)
                        {
                            EncirclePosition_1();
                        }
                        //α=[45,90)
                        else
                        {
                            EncirclePosition_2();
                        }
                    }
                    //当前方位在第二象限，确定环绕的目标地点
                    else if (deltaX <= 0 && deltaY >= 0)
                    {
                        //α=90
                        if (deltaX == 0)
                        {
                            EncirclePosition_3();
                        }

                        //α=180
                        if (deltaY == 0)
                        {
                            EncirclePosition_5();
                        }

                        //α=(90,135)
                        if ((-deltaX) / deltaY > 0 && (-deltaX) / deltaY < 1)
                        {
                            EncirclePosition_3();
                        }
                        //α=[135,180)
                        else
                        {
                            EncirclePosition_4();
                        }
                    }
                    //当前方位在第三象限，确定环绕的目标地点
                    else if (deltaX <= 0 && deltaY <= 0)
                    {
                        //α=270
                        if (deltaX == 0)
                        {
                            EncirclePosition_7();
                        }

                        //α=180
                        if (deltaY == 0)
                        {
                            EncirclePosition_5();
                        }

                        //α=(180,225)
                        if (deltaY / deltaX > 0 && deltaY / deltaX < 1)
                        {
                            EncirclePosition_5();
                        }
                        //α=[225,270)
                        else
                        {
                            EncirclePosition_6();
                        }

                    }
                    //当前方位在第四象限，确定环绕的目标地点
                    else if (deltaX >= 0 && deltaY <= 0)
                    {
                        //α=270
                        if (deltaX == 0)
                        {
                            EncirclePosition_7();
                        }

                        //α=0
                        if (deltaY == 0)
                        {
                            EncirclePosition_1();
                        }

                        //α=(270,315)
                        if (deltaX / (-deltaY) > 0 && deltaX / (-deltaY) < 1)
                        {
                            EncirclePosition_7();
                        }
                        //α=[315-360)
                        else
                        {
                            EncirclePosition_8();
                        }
                    }

                }
                //Log.Trace("完成一次AI逻辑 赋值");
                lastlogicalframe = DateTime.Now.Ticks;



                void EncirclePosition_1()
                {
                    p.X = list[0].GetPosition().X + 90 * 0.7071f;
                    p.Y = list[0].GetPosition().Y + 90 * 0.7071f;
                    container.GetPhysicalinternalBase().GetBody().FollowTarget(p,AIForce,AITorque);
                }

                void EncirclePosition_2()
                {
                    p.X = list[0].GetPosition().X;
                    p.Y = list[0].GetPosition().Y + 90;
                    container.GetPhysicalinternalBase().GetBody().FollowTarget(p,AIForce,AITorque);
                }

                void EncirclePosition_3()
                {
                    p.X = list[0].GetPosition().X - 90 * 0.7071f;
                    p.Y = list[0].GetPosition().Y + 90 * 0.7071f;
                    container.GetPhysicalinternalBase().GetBody().FollowTarget(p,AIForce,AITorque);
                }

                void EncirclePosition_4()
                {
                    p.X = list[0].GetPosition().X - 90;
                    p.Y = list[0].GetPosition().Y;
                    container.GetPhysicalinternalBase().GetBody().FollowTarget(p,AIForce,AITorque);
                }

                void EncirclePosition_5()
                {
                    p.X = list[0].GetPosition().X - 90 * 0.7071f;
                    p.Y = list[0].GetPosition().Y - 90 * 0.7071f;
                    container.GetPhysicalinternalBase().GetBody().FollowTarget(p,AIForce,AITorque);
                }

                void EncirclePosition_6()
                {
                    p.X = list[0].GetPosition().X;
                    p.Y = list[0].GetPosition().Y - 90;
                    container.GetPhysicalinternalBase().GetBody().FollowTarget(p,AIForce,AITorque);
                }

                void EncirclePosition_7()
                {
                    p.X = list[0].GetPosition().X + 90 * 0.7071f;
                    p.Y = list[0].GetPosition().Y - 90 * 0.7071f;
                    container.GetPhysicalinternalBase().GetBody().FollowTarget(p,AIForce,AITorque);
                }

                void EncirclePosition_8()
                {
                    p.X = list[0].GetPosition().X + 90;
                    p.Y = list[0].GetPosition().Y;
                    container.GetPhysicalinternalBase().GetBody().FollowTarget(p,AIForce,AITorque);
                }

            }

        }
    }
}
