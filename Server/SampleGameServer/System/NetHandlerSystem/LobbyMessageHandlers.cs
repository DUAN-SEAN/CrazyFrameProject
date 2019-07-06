using Crazy.Common;
using Crazy.ServerBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.System.NetHandlerSystem
{
    [MessageHandler]
    public class C2S_UpdateOnlinePlayerMessageHandler : AMRpcHandler<C2S_UpdateOnlinePlayerList, S2C_UpdateOnlinePlayerList>
    {
        protected override void Run(ISession playerContext, C2S_UpdateOnlinePlayerList message, Action<S2C_UpdateOnlinePlayerList> reply)
        {
            GameServerPlayerContext gameServerPlayerContext = playerContext as GameServerPlayerContext;
            if (gameServerPlayerContext == null) return;

            //1 根据PlayerManager获取在线玩家集合 
            var playerContexts =  GameServer.Instance.PlayerCtxManager.GetRegisteredByStringPlayerList();
            List<string> players = new List<string>();
            foreach(var playerId in playerContexts)
            {
                players.Add(playerId.Key);
            }
            //2 封装消息发送给matchSystem
            var lm = new UpdateOnlinePlayerMessage { playerId = message.LaunchPlayerId, players = players ,reply = reply};
            GameServer.Instance.PostMessageToSystem<GameMatchSystem>(lm);
        }
    }
}
