using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using Crazy.ServerBase;
namespace GameServer
{
    [MessageHandler]
    public class C2S_LoginMessageHandler : AMRpcHandler<C2S_LoginMessage, S2C_LoginMessage>
    {
        protected override void Run(ISession playerContext, C2S_LoginMessage message, Action<S2C_LoginMessage> reply)
        {
            GameServerPlayerContext gameServerContext = playerContext as GameServerPlayerContext;
            S2C_LoginMessage response = new S2C_LoginMessage();
            try
            {

                response.State = S2C_LoginMessage.Types.State.Ok;
                response.PlayerId = gameServerContext.ContextId;//将当前玩家现场的Id发送给客户端

            }catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
            reply(response);


        }
    }
    [MessageHandler]
    public class C2S_RegisterMessageHandler : AMRpcHandler<C2S_RegisterMessage, S2C_RegisterMessage>
    {
        protected override void Run(ISession playerContext, C2S_RegisterMessage message, Action<S2C_RegisterMessage> reply)
        {
            S2C_RegisterMessage response = new S2C_RegisterMessage();
            try
            {



            }catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
            reply(response);
        }
    }
}
