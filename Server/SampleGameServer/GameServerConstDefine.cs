using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGameServer.DB
{
    //Game服务器的消息字段声明
    public class GameServerConstDefine
    {

    }
    /// <summary>
    /// playercollecion各个字段的定义key
    /// </summary>
    public class SampleGamePlayerDBItemDefine
    {
        /// <summary>
        /// 所属的数据库
        /// </summary>
        public const String DATABASE = "Space Shooter";

        /// <summary>
        /// 数据库collection的名字
        /// </summary>
        public const String COLLECTION = "Players";

        /// <summary>
        /// 数据库记录自生成id
        /// </summary>
        public const String PLAYER_ID = "_id";

        /// <summary>
        /// 玩家id
        /// </summary>
        public const String PLAYER_USERID = "UserId";

        
    }
}
