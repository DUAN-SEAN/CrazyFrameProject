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
        public bool IsPlayer;
        public Int32 weapontype_a;
        public Int32 weapontype_b;

        public InitEventMessage(ulong actorid,int camp, Int32 actortype, double point_x, double point_y, double angle,bool isPlayer = false, Int32 weapontype_a = 0, Int32 weapontype_b = 0)
        {
            _eventMessageId = EventMessageConstDefine.InitEvent;
            this.actorid = actorid;
            this.actortype = actortype;
            this.weapontype_a = weapontype_a;
            this.weapontype_b = weapontype_b;
            this.point_x = point_x;
            this.point_y = point_y;
            this.angle = angle;
            haveId = true;
            IsPlayer = isPlayer;
        }
        public InitEventMessage(Int32 actortype,int camp, double point_x, double point_y, double angle,bool isPlayer = false, Int32 weapontype_a = 0, Int32 weapontype_b = 0)
        {
            _eventMessageId = EventMessageConstDefine.InitEvent;
            this.actortype = actortype;
            this.weapontype_a = weapontype_a;
            this.weapontype_b = weapontype_b;
            this.point_x = point_x;
            this.point_y = point_y;
            this.angle = angle;
            haveId = false;
            IsPlayer = isPlayer;
        }
    }
}
