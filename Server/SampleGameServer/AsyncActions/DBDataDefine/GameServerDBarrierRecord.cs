using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGameServer
{
    public class GameServerDBarrierRecord
    {
        public ObjectId _id;

        public long battleId;

        public long number;

        public List<GameServerDBarrierRecordPlayer> players = new List<GameServerDBarrierRecordPlayer>();//关卡记录中玩家信息

        public DateTime startTime;

        public DateTime costTime;

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
