﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    //Game服务器的消息字段声明
    public class GameServerConstDefine
    {
        //为了与ServerBase进行区分 这里只做了

        #region MatchSystem        
        public const Int32 MatchSystemCreateMatchTeam = 1002;//创建队伍
        public const Int32 MatchSystemJoinMatchTeam = 1003;//进入队伍
        public const Int32 MatchSystemExitMatchTeam = 1004;//退出队伍
        #endregion



    }
    /// <summary>
    /// playercollecion各个字段的定义key
    /// </summary>
    public class SampleGameServerDBItemDefine
    {
        /// <summary>
        /// 所属的数据库
        /// </summary>
        public const String DATABASE = "SpaceShooter";

        /// <summary>
        /// 玩家集合
        /// </summary>
        public const String COLLECTION_PLAYERS = "Players";
        /// <summary>
        /// 关卡战斗记录集合
        /// </summary>
        public const String COLLECTION_BARRIERRECORD = "BarrierRecords";
        /// <summary>
        /// 数据库记录自生成id
        /// </summary>
        public const String PLAYER_ID = "_id";

        /// <summary>
        /// 玩家id
        /// </summary>
        public const String PLAYER_USERID = "userId";

        /// <summary>
        /// 玩家昵称
        /// </summary>
        public const String PLAYER_USERNAME = "userName";

        /// <summary>
        /// 玩家密码
        /// </summary>
        public const String PLAYER_PASSWORD = "passWord";

        /// <summary>
        /// 玩家创建时间
        /// </summary>
        public const String PLAYER_CREATETIME = "createTime";
    }
}
