using Crazy.NetSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 这里存放所有与匹配相关的本地消息
/// </summary>
namespace GameServer
{
    /// <summary>
    /// 创建队伍
    /// </summary>
    public class CreateMatchTeamMessage : ILocalMessage
    {
        public int MessageId => GameServerConstDefine.MatchSystemCreateMatchTeam;

        public string playerId;//玩家Id
    }
    /// <summary>
    /// 进入队伍
    /// </summary>
    public class JoinMatchTeamMessage : ILocalMessage
    {
        public int MessageId => GameServerConstDefine.MatchSystemJoinMatchTeam;

        public string playerId;//玩家Id

        public UInt64 teamId;//队伍Id
    }
    /// <summary>
    /// 退出队伍
    /// </summary>
    public class ExitMatchTeamMessage : ILocalMessage
    {
        public int MessageId => GameServerConstDefine.MatchSystemExitMatchTeam;

        public string playerId;//玩家Id

        public UInt64 teamId;//队伍Id
    }
    /// <summary>
    /// 进入匹配队列
    /// </summary>
    public class JoinMatchQueueMessage : ILocalMessage
    {
        public int MessageId => GameServerConstDefine.MatchSystemJoinMatchQueue;
        public UInt64 teamId;//队伍Id
        public string playerId;//队长Id 意思就是队伍发起人 也可以只有一个人
        public Int32 barrierId;//关卡Id 每一个关卡都有对应的Id  
    }
    /// <summary>
    /// 离开匹配队列
    /// </summary>
    public class ExitMatchQueueMessage : ILocalMessage
    {
        public int MessageId => GameServerConstDefine.MatchSystemJoinMatchQueue;
        public UInt64 teamId;//队伍Id
        public string playerId;//玩家Id 队伍中的玩家都能退出匹配队列
        public Int32 barrierId;//关卡Id 每一个关卡都有对应的Id 
    }
    /// <summary>
    /// 匹配队列完成一个
    /// 向MatchSystem发送
    /// </summary>
    public class MatchQueueCompleteSingleMessage : ILocalMessage
    {
        public int MessageId => GameServerConstDefine.MatchQueueCompleteSingle;
        public List<UInt64> teamIds = new List<UInt64>();
        public int barrierId;
    }
    public class MatchTeamUpdateInfoMessage : ILocalMessage
    {
        public int MessageId => GameServerConstDefine.MatchSysteamMatchTeamUpdateInfo;
        public UInt64 teamId;//队伍Id
    }



}
