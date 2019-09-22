using Crazy.Common;
using Crazy.ServerBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.System.NetHandlerSystem
{
    /// <summary>
    /// 玩家更新在线玩家信息
    /// </summary>
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

    [MessageHandler]
    public class C2S_GetPlayerShipInfoMessageHandler:AMRpcHandler<C2S_ShipInfoReq,S2C_ShipInfoAck>
    {
        protected async override void Run(ISession playerContext, C2S_ShipInfoReq message, Action<S2C_ShipInfoAck> reply)
        {
            GameServerPlayerContext gameServerPlayerContext = playerContext as GameServerPlayerContext;
            S2C_ShipInfoAck response = new S2C_ShipInfoAck();
            var ship =  await gameServerPlayerContext.GetCurrentShipInfoFormDB();
            if (ship != null)
            {
                response.ShipId = ship.shipId;
                response.ShipType = ship.shipType;
                response.ShipName = ship.shipName;
                response.WeaponA = ship.weapon_a;
                response.WeaponB = ship.weapon_b;
                reply(response);

            }
        }
    }
    [MessageHandler]
    public class C2S_UpLoadPlayerShipInfoMessageHandler:AMRpcHandler<C2S_UpLoadShipInfoReq,S2C_UpLoadShipInfoAck>
    {
        protected async override void Run(ISession playerContext, C2S_UpLoadShipInfoReq message, Action<S2C_UpLoadShipInfoAck> reply)
        {
            GameServerPlayerContext gameServerPlayerContext = playerContext as GameServerPlayerContext;

            S2C_UpLoadShipInfoAck response = new S2C_UpLoadShipInfoAck();
            GameServerDBPlayerShip shipInfo = new GameServerDBPlayerShip();
            shipInfo.shipId = message.ShipId;
            shipInfo.shipName = message.ShipName;
            shipInfo.shipType = message.ShipType;
            shipInfo.weapon_a = message.WeaponA;
            shipInfo.weapon_b = message.WeaponB;
            var flag = await gameServerPlayerContext.UpLoadShipInfoToDB(shipInfo);
            if (flag)
            {
                response.State = S2C_UpLoadShipInfoAck.Types.State.Ok;
            }
            else
            {
                response.State = S2C_UpLoadShipInfoAck.Types.State.Fail;
            }

            reply(response);
        }
    }

    [MessageHandler]
    public class C2S_SpeakTeamReqMessageHandler:AMHandler<C2S_SpeakToTeamReq>
    {
        protected override void Run(ISession playerContext, C2S_SpeakToTeamReq message)
        {
            S2C_SpeakToTeamAck ack = new S2C_SpeakToTeamAck{Data = message.Data,LaunchPlayerId = message.LaunchPlayerId,MatchTeamId = message.MatchTeamId};
            
            
            GameServer.Instance.PlayerCtxManager.BroadcastLocalMessagebyPlayerId(new SystemSendNetMessage{Message = ack}, message.PlayerIds.ToList());
            

        }
    }
}
