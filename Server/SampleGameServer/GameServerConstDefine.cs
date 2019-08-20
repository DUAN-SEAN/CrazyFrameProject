using System;
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
        #region GameServerPlayerCtx 1001-1050
        /// <summary>
        /// 服务器本地消息：当ContextTransform完成发送给目标ctx
        /// </summary>
        public const Int32 LocalMsgGameServerContextTransformOK = 1001;
        /// <summary>
        /// 服务器本地消息:关闭当前玩家现场
        /// </summary>
        public const Int32 LocalMsgShutdownContext = 1002;
        #endregion
        #region MatchSystem 1051-1100  
        /// <summary>
        /// 创建队伍
        /// </summary>
        public const Int32 MatchSystemCreateMatchTeam = 1051;
        /// <summary>
        /// 进入队伍
        /// </summary>
        public const Int32 MatchSystemJoinMatchTeam = 1052;
        /// <summary>
        /// 退出队伍
        /// </summary>
        public const Int32 MatchSystemExitMatchTeam = 1053;
        /// <summary>
        /// 进入匹配队列
        /// </summary>
        public const Int32 MatchSystemJoinMatchQueue = 1054;
        /// <summary>
        /// 退出匹配队列
        /// </summary>
        public const Int32 MatchSystemExitMatchQueue = 1055;
        /// <summary>
        /// 匹配队列完成一项
        /// </summary>
        public const Int32 MatchQueueCompleteSingle = 1056;
        /// <summary>
        /// 更新匹配队伍的信息
        /// </summary>
        public const Int32 MatchSysteamMatchTeamUpdateInfo = 1057;
        /// <summary>
        /// 玩家现场被关闭时需要向匹配系统发送一个消息
        /// </summary>
        public const Int32 MatchSystemPlayerShutdown = 1058;
        /// <summary>
        /// 更新在线玩家集合，消息为来自客户端rpc请求
        /// </summary>
        public const Int32 UpdateOnlinePlayerList = 1059;
        #endregion
        #region BattleSystem 1101-1200
        /// <summary>
        /// 创建战斗关卡
        /// </summary>
        public const Int32 BattleSystemCreateBattleBarrier = 1101;
        /// <summary>
        /// 退出战斗关卡
        /// </summary>
        public const Int32 BattleSystemExitBattleBarrier = 1102;
        /// <summary>
        /// 上传战斗指令
        /// </summary>
        public const Int32 BattleSystemCommandUpLoad = 1103;
        /// <summary>
        /// 客户端准备战斗
        /// </summary>
        public const Int32 BattleSystemClientReadyBattle = 1104;
        /// <summary>
        /// 关闭玩家现场
        /// </summary>
        public const Int32 BattleSystemPlayerShutdown = 1105;
        /// <summary>
        /// 战斗系统需要释放battleTimer
        /// </summary>
        public const Int32 BattleSystemNeedReleaseBattleTimer = 1106;
        #endregion

        /// <summary>
        /// 玩家现场上的timer扫描时间间隔，单位毫秒
        /// </summary>
        public const Int32 GameServerPlayerCtxTimerPeriod = 1000;//1s
        /// <summary>
        /// Battle最多等待玩家时长
        /// </summary>
        public const Int32 BattleSystemWaitPlayerReadySuperiorTime = 60000;//1分钟




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

        /// <summary>
        /// 玩家飞船配置
        /// </summary>
        public const String PLAYER_SHIPINFO = "playerShip";
    }
}
