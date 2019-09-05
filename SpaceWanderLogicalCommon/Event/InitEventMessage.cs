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
        public ulong onwerid;
        public Int32 actortype;
        public int camp;
        public float point_x;
        public float point_y;
        public float relatpoint_x;
        public float relatpoint_y;
        public float angle;
        public readonly bool haveId;
        public bool IsPlayer;
        public Int32 weapontype_a;
        public Int32 weapontype_b;
        public string name;

        public InitEventMessage(ulong actorid, int camp, Int32 actortype, float point_x, float point_y, float angle,
            bool isPlayer = false, Int32 weapontype_a = 0, Int32 weapontype_b = 0, string name = "",
            ulong ownerid = ulong.MaxValue, float relatpoint_x = 0, float relatpoint_y = 0)
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
            this.name = name;
            this.camp = camp;
            this.onwerid = ownerid;
            this.relatpoint_x = relatpoint_x;
            this.relatpoint_y = relatpoint_y;
        }
        public InitEventMessage(Int32 actortype,int camp, float point_x, float point_y, float angle,bool isPlayer = false, Int32 weapontype_a = 0, Int32 weapontype_b = 0, string name = "", ulong ownerid = ulong.MaxValue, float relatpoint_x = 0, float relatpoint_y = 0)
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
            this.name = name;
            this.camp = camp;
            this.onwerid = ownerid;
            this.relatpoint_x = relatpoint_x;
            this.relatpoint_y = relatpoint_y;
        }
    }
}
