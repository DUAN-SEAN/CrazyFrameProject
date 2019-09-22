using Crazy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public class EnemyInitTaskCondition : ITaskCondition
    {
        protected Dictionary<int, int> Currentvalue;
        private TaskEventBase taskEventBase;
        private ILevelActorComponentBaseContainer levelActor;
        private bool IsInitDone;
        /// <summary>
        /// 已生成数量
        /// </summary>
        protected int initNum = 0;

        private Vector2D[] EnemyPoint = { new Vector2D(50, 50), new Vector2D(-50, -50), new Vector2D(50, -50), new Vector2D(-50, 50) };

        public EnemyInitTaskCondition(TaskEventBase taskEventBase, ILevelActorComponentBaseContainer levelActor)
        {
            this.taskEventBase = taskEventBase;
            this.levelActor = levelActor;
            Currentvalue = new Dictionary<int, int>();
            IsInitDone = false;
        }
        
        public Dictionary<int, int> ConditionCurrentValues
        {
            get => Currentvalue;
            set => Currentvalue = value;
        }

        public void Dispose()
        {
        }

        public int GetCurrentValue()
        {
            return initNum;
        }

        public int GetTargetValue()
        {
            foreach (var item in taskEventBase.GetTaskValues())
            {
                if (item.Key.isActorTypeNumber())
                {
                    return item.Value;
                }
            }

            return 0;
        }

        public void StartCondition()
        {
            foreach(var item in taskEventBase.GetTaskValues())
            {
                if (item.Key.isActorTypeNumber())
                {
                    for(int i = 0; i < item.Value; i++)
                    {
                        initNum++;
                        levelActor.AddEventMessagesToHandlerForward(new InitEventMessage(actorid: levelActor.GetCreateInternalComponentBase().GetCreateID(), actortype: item.Key, camp: LevelActorBase.EnemyCamp, point_x: EnemyPoint[i % EnemyPoint.Length].X, point_y: EnemyPoint[i % EnemyPoint.Length].Y, angle: 0, LinerDamping: 0.1f)) ;
                        //Log.Trace("EnemyInitTaskCondition: StartCondition 任务id：" + taskEventBase.GetTaskId() + " 生成第" + i + "个对象 key:" + item.Key);
                    }
                }
            }
            IsInitDone = true;
        }

        public bool TickCondition()
        {
            return IsInitDone;
        }
    }
}
