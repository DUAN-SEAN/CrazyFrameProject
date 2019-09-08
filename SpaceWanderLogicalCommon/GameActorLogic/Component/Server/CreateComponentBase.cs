using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;

namespace GameActorLogic
{
    public class CreateComponentBase :
        ICreateComponentBase,
        ICreateInternalComponentBase
    {
        /// <summary>
        /// ActorID
        /// </summary>
        protected ulong IDs;

        protected ILevelActorComponentBaseContainer level;
        public CreateComponentBase(ILevelActorComponentBaseContainer level)
        {
            IDs = 0;
            this.level = level;
        }

        public void Dispose()
        {
            level = null;
        }

        #region 创建Actor

        public ulong GetCreateID()
        {
            return IDs++;
        }

        public ActorBase CreateActor(int actortype,int camp, float Vector2_x, float Vector2_y, float angle, bool isPlayer = false, Int32 weapontype_a = 0, Int32 weapontype_b = 0,string name = "")
        {
            return CreateActor(actortype, camp, Vector2_x, Vector2_y, angle, GetCreateID(), isPlayer, weapontype_a,
                weapontype_b, name);
        }

        public ActorBase CreateActor(int actortype, int camp, float Vector2_x, float Vector2_y, float angle, ulong Id, bool isPlayer = false, Int32 weapontype_a = 0, Int32 weapontype_b = 0,string name ="")
        {
            ActorBase actor = null;
            //从配置文件中获取Actor
            level.GetConfigComponentInternalBase().GetActorClone(actortype, out actor);

            switch (actortype)
            {
                case ActorTypeBaseDefine.ActorNone:
                    //actor = new ActorBase();
                    break;

                #region 飞船Actor
                case ActorTypeBaseDefine.ShipActorNone:
                case ActorTypeBaseDefine.EliteShipActorA:
                case ActorTypeBaseDefine.EliteShipActorB:
                case ActorTypeBaseDefine.FighterShipActorA:
                case ActorTypeBaseDefine.FighterShipActorB:
                case ActorTypeBaseDefine.WaspShipActorA:
                case ActorTypeBaseDefine.AnnihilationShipActor:
                case ActorTypeBaseDefine.DroneShipActor:
                case ActorTypeBaseDefine.PlayerShipActor:
                    if (actor == null)
                        actor = new ShipActorBase(Id, actortype, level);
                    actor.SetActorId(Id);
                    actor.PrepareActor(Vector2_x, Vector2_y, angle);
                    actor.SetCamp(camp);
                    if (isPlayer)
                    {
                        var ship = (ShipActorBase) actor;
                        ship.SetActorName(name);
                        ship.CreateAiComponent(null);
                        ship.InitializeFireControl(new List<int>
                        {
                            weapontype_a, weapontype_b
                        });
                    }

                    break;


                #endregion

                #region 武器Acotr

                #region 默认向前飞型武器
                case ActorTypeBaseDefine.AntiAircraftGunActor:
                case ActorTypeBaseDefine.MachineGunActor:
                case ActorTypeBaseDefine.PowerLaserActor:
                case ActorTypeBaseDefine.TorpedoActor:
                #endregion

                #region 跟踪型武器

                case ActorTypeBaseDefine.TrackingMissileActor:

                #endregion



                case ActorTypeBaseDefine.ContinuousLaserActor:
                case ActorTypeBaseDefine.TimeBombActor:
                case ActorTypeBaseDefine.TriggerBombActor:
                    if (actor != null)
                    {
                        actor.SetActorId(Id);
                        actor.PrepareActor(Vector2_x, Vector2_y, angle);
                        actor.SetCamp(camp);
                    }
                   
                    break;

                #endregion
            }

            return actor;
        }

        public ITaskEvent CreateTaskEvent(int taskcondition,int taskresult, int taskid, Dictionary<int, int> taskconditions,string des)
        {
            ITaskEvent task = null;
            //Log.Trace("创建任务" + taskid);
            task = new TaskEventBase(taskid, level, taskcondition, taskresult, taskconditions,des);
           
            return task;
        }

        #endregion                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           


    }

  

}
