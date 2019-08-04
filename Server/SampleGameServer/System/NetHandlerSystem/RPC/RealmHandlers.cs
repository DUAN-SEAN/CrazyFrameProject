using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using Crazy.ServerBase;
namespace GameServer
{
    /// <summary>
    /// 由于网络消息handler由dispatcher分配调用，而dispatcher由玩家现场的OnMessage调用 所以保证了单个玩家现场
    /// 消息执行顺序的次序
    /// </summary>
    [MessageHandler]
    public class C2S_LoginMessageHandler : AMRpcHandler<C2S_LoginMessage, S2C_LoginMessage>
    {
        protected override async void Run(ISession playerContext, C2S_LoginMessage message, Action<S2C_LoginMessage> reply)
        {
            GameServerPlayerContext gameServerContext = playerContext as GameServerPlayerContext;
            S2C_LoginMessage response = new S2C_LoginMessage();
            try
            {
                Log.Info($"登陆 {message} ");
                await gameServerContext.OnAuthByLogin(message,reply);

                //原始的Action方法不再使用 目前逻辑都在playerContext

            }catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
    [MessageHandler]
    public class C2S_RegisterMessageHandler : AMRpcHandler<C2S_RegisterMessage, S2C_RegisterMessage>
    {
        protected override void Run(ISession playerContext, C2S_RegisterMessage message, Action<S2C_RegisterMessage> reply)
        {
            GameServerPlayerContext gameServerContext = playerContext as GameServerPlayerContext;
            S2C_RegisterMessage response = new S2C_RegisterMessage();
            try
            {
                new RegsiterVerifyContextAsyncAction(gameServerContext, message.Account, message.Password, reply,true,true).Start();
            }
            catch(Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
    /// <summary>
    /// 玩家重连或验证发送给服务器的消息 
    /// </summary>
    [MessageHandler]
    public class C2S_ReConnectingByLoginMessageHandler : AMHandler<C2S_ReConnectByLogin>
    {
        protected override async void Run(ISession playerContext, C2S_ReConnectByLogin message)
        {
            GameServerPlayerContext gameServerContext = playerContext as GameServerPlayerContext;
            await gameServerContext.OnAuthReCByLogin(message);
        }
    }
}
