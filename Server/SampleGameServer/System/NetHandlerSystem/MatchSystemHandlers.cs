using Crazy.Common;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.System.NetHandlerSystem
{

    [MessageHandler]
    public class C2S_CreateMatchTeamMessageHandler : AMHandler<C2S_CreateMatchTeam>
    {
        protected override void Run(ISession playerContext, C2S_CreateMatchTeam message)
        {
            Log.Info(message.ToJson());
            //向MatchSystem发送创建队伍消息
            var lm = new CreateMatchTeamMessage { playerId = message.PlayerId };
            GameServer.Instance.PostMessageToSystem<GameMatchSystem>(lm);
            
        }
    }

    [MessageHandler]
    public class C2S_JoinMatchTeamMessageHandler : AMHandler<C2S_JoinMatchTeam>
    {
        protected override void Run(ISession playerContext, C2S_JoinMatchTeam message)
        {
            Log.Info(message.ToJson());
            var lm = new JoinMatchTeamMessage();
            lm.playerId = message.LaunchPlayerId;
            lm.teamId = message.MatchTeamId;
            GameServer.Instance.PostMessageToSystem<GameMatchSystem>(lm);
        }
    }
    [MessageHandler]
    public class C2S_ExitMatchTeamMessageHandler : AMHandler<C2S_ExitMatchTeam>
    {
        protected override void Run(ISession playerContext, C2S_ExitMatchTeam message)
        {
            Log.Info(message.ToJson());
            var lm = new ExitMatchTeamMessage();
            lm.playerId = message.LaunchPlayerId;
            lm.teamId = message.MatchTeamId;
            GameServer.Instance.PostMessageToSystem<GameMatchSystem>(lm);
        }
    }
    [MessageHandler]
    public class C2S_JoinMatchQueueMessageHandler : AMHandler<C2S_JoinMatchQueue>
    {
        protected override void Run(ISession playerContext, C2S_JoinMatchQueue message)
        {
            
        }
    }
}
