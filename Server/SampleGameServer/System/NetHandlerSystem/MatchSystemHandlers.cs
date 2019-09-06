using Crazy.Common;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.ServerBase;

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
            GameServerPlayerContext context = playerContext as GameServerPlayerContext;
            if(context.ContextStringName != message.LaunchPlayerId)
            {
                Log.Debug("发起人和退出玩家不一致，不能执行逻辑");
                return;
            }
            var lm = new JoinMatchTeamMessage();

            lm.playerId = message.LaunchPlayerId;
            lm.teamId = message.MatchTeamId;
            
            GameServer.Instance.PostMessageToSystem<GameMatchSystem>(lm);
        }
    }
    /// <summary>
    /// 退出匹配队伍
    /// </summary>
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
    public class C2S_GetMatchTeamInfoMessageHandler : AMHandler<C2S_GetMatchTeamInfo>
    {
        /// <summary>
        /// 获取一次队伍信息
        /// </summary>
        /// <param name="playerContext"></param>
        /// <param name="message"></param>
        protected override void Run(ISession playerContext, C2S_GetMatchTeamInfo message)
        {
            //向匹配系统发送获取队伍信息 并发送给发起人
            var lm = new MatchTeamUpdateInfoMessage();
            lm.teamId = message.MatchTeamId;
            GameServer.Instance.PostMessageToSystem<GameMatchSystem>(lm);
        }
    }

    [MessageHandler]
    public class C2S_JoinMatchQueueMessageHandler : AMHandler<C2S_JoinMatchQueue>
    {
        protected override void Run(ISession playerContext, C2S_JoinMatchQueue message)
        {
            //TODO:队伍的队长发起匹配交给匹配系统执行逻辑
            var lm = new JoinMatchQueueMessage();
            lm.barrierId = message.BarrierId;
            lm.playerId = message.LaunchPlayerId;
            lm.teamId = message.MatchTeamId;
            GameServer.Instance.PostMessageToSystem<GameMatchSystem>(lm);
        }
    }
    
    [MessageHandler]
    public class C2S_ExitMatchQueueMessageHandler : AMHandler<C2S_ExitMatchQueue>
    {
        protected override void Run(ISession playerContext, C2S_ExitMatchQueue message)
        {
            GameServerPlayerContext context = playerContext as GameServerPlayerContext;
            if (!context.IsAvaliable()) return; 
            ExitMatchQueueMessage lm = new ExitMatchQueueMessage { playerId = message.LaunchPlayerId, teamId = message.MatchTeamId};
            GameServer.Instance.PostMessageToSystem<GameMatchSystem>(lm);

        }
    }
    [MessageHandler]
    public class C2S_InviteMatchTeamMessageHandler:AMHandler<C2S_InvitePlayerMatchTeam>
    {
        protected override void Run(ISession playerContext, C2S_InvitePlayerMatchTeam message)
        {
            GameServerPlayerContext context = playerContext as GameServerPlayerContext;

            GameServer.Instance.PlayerCtxManager.SendSingleLocalMessage(new SystemSendNetMessage{ PlayerId = message.AimPlayerId,Message = new S2C_InvitePlayerMatchTeam{ LaunchPlayerId = message.LaunchPlayerId,MatchTeamId = message.MatchTeamId}}, message.AimPlayerId);
        }
    }
}
