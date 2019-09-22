using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class GameServerDBPlayer
    {
        
        public ObjectId _id;


        public string userName;

        public string passWord;

        public List<GameServerDBPlayerBarrierRatings> barrierRatings = new List<GameServerDBPlayerBarrierRatings>();

        public GameServerDBPlayerShip playerShip = new GameServerDBPlayerShip();

        public DateTime createTime;

        internal string DisPlayer()
        {
            return this.ToJson();
        }


    }

    public class GameServerDBPlayerShip
    {
        /// <summary>
        /// 飞船Id
        /// </summary>
        public Int32 shipId =1004;
        /// <summary>
        /// 飞船类型
        /// </summary>
        public Int32 shipType = 1004;
        /// <summary>
        /// 飞船名称
        /// </summary>
        public string shipName = "空";
        /// <summary>
        /// 配置武器a
        /// </summary>
        public Int32 weapon_a = 1012;
        /// <summary>
        /// 配置武器b
        /// </summary>
        public Int32 weapon_b = 1013;
        /// <summary>
        /// 最后修改的时间
        /// </summary>
        public DateTime modifyTime;
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
