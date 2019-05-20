using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using Crazy.NetSharp;
using Crazy.ServerBase;
namespace GameServer
{
    /// <summary>
    /// 玩家现场
    /// </summary>
    public sealed class GameServerPlayerContext:PlayerContextBase
    {
        /// <summary>
        /// 直接重写
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public override async Task OnMessage(ILocalMessage msg)
        {
            switch (msg.MessageId)
            {
                //AsyncAction消息的回调 只有当AsyncAction和某个玩家绑定时才会执行Result的事件
                case ServerBaseLocalMesssageIDDef.LocalMsgAsyncActionResult:
                    ContextAsyncAction contextAsyncAction = msg as ContextAsyncAction;
                    contextAsyncAction.OnResultInternal();//执行回调

                    break;
                case ServerBaseLocalMesssageIDDef.NetMessage://如何是网络消息就通过分发器进行分发
                    NetClientMessage message = msg as NetClientMessage;
                    MessageDispather.Instance.Handle(this, message.MessageInfo); //在这里将消息进行分发  直接调用逻辑handler
                    break;
                case ServerBaseLocalMesssageIDDef.RpcNetMessage:
                    RpcNetClientMessage rpcmessage = msg as RpcNetClientMessage;
                    IResponse response = rpcmessage.MessageInfo.Message as IResponse;
                    if (response == null)
                    {
                        throw new Exception($"flag is response, but message is not! {rpcmessage.MessageInfo.Opcode}");
                    }
                    Action<IResponse> action;
                    if (!m_requestCallback.TryGetValue(response.RpcId, out action))
                    {
                        return;
                    }
                    m_requestCallback.Remove(response.RpcId);
                    action(response);//这里处理逻辑 返回await
                    break;
                default: break;
            }


            return ;
        }
    }
}
