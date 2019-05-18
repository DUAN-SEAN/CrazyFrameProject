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

        public UInt64 playerId;//玩家Id
    }
    /// <summary>
    /// 进入队伍
    /// </summary>
    public class JoinMatchTeamMessage : ILocalMessage
    {
        public int MessageId => GameServerConstDefine.MatchSystemJoinMatchTeam;

        public UInt64 playerId;//玩家Id

        public UInt64 teamId;//队伍Id
    }
    /// <summary>
    /// 退出队伍
    /// </summary>
    public class ExitMatchTeamMessage : ILocalMessage
    {
        public int MessageId => GameServerConstDefine.MatchSystemExitMatchTeam;

        public UInt64 playerId;//玩家Id

        public UInt64 teamId;//队伍Id
    }

    public class JoinMatchQueueMessage : ILocalMessage
    {
        public int MessageId => GameServerConstDefine.;
    }
}
