﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class GameServerDBarrierRecord
    {
        public ObjectId _id;

        public long battleId;

        public long number;

        public List<GameServerDBarrierRecordPlayer> players ;//关卡记录中玩家信息

        public DateTime startTime;

        public long costTime;//毫秒

        public DateTime endTime;

        public int grade;

        internal string Tojson()
        {
            return this.ToJson();
        }

    }
    public class GameServerDBarrierRecordPlayer
    {
        public ObjectId userId;
        public string userName;
    }
}
