using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Common;

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

        #region 创建Actor

        public ulong GetCreateID()
        {
            return IDs++;
        }

        public ActorBase CreateActor(int actortype,int camp, double point_x, double point_y, double angle)
        {
            return CreateActor(actortype,camp, point_x, point_y, angle, GetCreateID());
        }

        public ActorBase CreateActor(int actortype, int camp,double point_x, double point_y, double angle, ulong Id)
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
                    if (actor == null) 
                        actor = new ShipActorBase(Id, level);
                    actor.SetActorId(Id);
                    actor.PrepareActor(point_x, point_y, angle);
                    actor.SetCamp(camp);
                    break;
                case ActorTypeBaseDefine.EliteShipActorA:
                    break;
                case ActorTypeBaseDefine.EliteShipActorB:
                    break;
                case ActorTypeBaseDefine.FighterShipActorA:
                    break;
                case ActorTypeBaseDefine.FighterShipActorB:
                    break;
                case ActorTypeBaseDefine.WaspShipActorA:
                    break;
                case ActorTypeBaseDefine.AnnihilationShipActor:
                    break;
                case ActorTypeBaseDefine.DroneShipActor:
                    break;
                case ActorTypeBaseDefine.PlayerShipActor:
                    break;

                #endregion

                #region 武器Acotr
                case ActorTypeBaseDefine.AntiAircraftGunActor:
                case ActorTypeBaseDefine.ContinuousLaserActor:
                case ActorTypeBaseDefine.MachineGunActor:
                case ActorTypeBaseDefine.PowerLaserActor:
                case ActorTypeBaseDefine.TimeBombActor:
                case ActorTypeBaseDefine.TorpedoActor:
                case ActorTypeBaseDefine.TrackingMissileActor:
                case ActorTypeBaseDefine.TriggerBombActor:
                    break;

                #endregion
            }

            return actor;
        }

        public ITaskEvent CreateTaskEvent(int taskcondition,int taskresult, int taskid, Dictionary<int, int> taskconditions)
        {
            ITaskEvent task = null;
            task = new TaskEventBase(taskid, level, taskcondition, taskresult);
            foreach (var i in taskconditions)
            {
                task?.AddValue(i.Key, i.Value);
            }
            return task;
        }

        #endregion


    }

  

}
