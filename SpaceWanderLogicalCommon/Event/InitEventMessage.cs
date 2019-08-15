using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    [Serializable]
    public class InitEventMessage : BattleEventMessageBase
    {
        public ulong actorid;
        public Int32 actortype;
        public int camp;
        public double point_x;
        public double point_y;
        public double angle;
        public readonly bool haveId;
        public InitEventMessage(ulong actorid,int camp, Int32 actortype, double point_x, double point_y, double angle)
        {
            _eventMessageId = EventMessageConstDefine.InitEvent;
            this.actorid = actorid;
            this.actortype = actortype;
            this.point_x = point_x;
            this.point_y = point_y;
            this.angle = angle;
            haveId = true;
        }
        public InitEventMessage(Int32 actortype,int camp, double point_x, double point_y, double angle)
        {
            _eventMessageId = EventMessageConstDefine.InitEvent;

            this.actortype = actortype;
            this.point_x = point_x;
            this.point_y = point_y;
            this.angle = angle;
            haveId = false;
        }
    }
}
