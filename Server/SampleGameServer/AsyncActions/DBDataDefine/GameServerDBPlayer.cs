using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGameServer
{
    public class GameServerDBPlayer
    {
        
        public ObjectId _id;


        public string userName;

        public string passWord;

        public List<GameServerDBPlayerBarrierRatings> barrierRatings = new List<GameServerDBPlayerBarrierRatings>();

        public DateTime createTime;

        internal string DisPlayer()
        {
            return this.ToJson();
        }


    }
    public class GameServerDBPlayerBarrierRatings
    {
        public long number;//关卡编号

        public int betterGrade;//最佳评分

        public List<GameServerDBPlayerBarrierRatingsRecords> records = new List<GameServerDBPlayerBarrierRatingsRecords>();
        
    }
    public class GameServerDBPlayerBarrierRatingsRecords
    {
        public long battleId;

        public int grade;
    }
}
