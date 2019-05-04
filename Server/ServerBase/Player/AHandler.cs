using Crazy.Common;
using System;
using System.Collections.Generic;
using System.Text;
namespace Crazy.ServerBase
{
  
    public abstract class AMHandler<Message> : IMHandler where Message : class 
    {
        protected abstract void Run(PlayerContextBase playerContext, Message message);
        public Type GetMessageType()
        {
            return typeof(Message);
        }

        public void Handle(PlayerContextBase sender, object msg)
        {
            Message message = msg as Message;
            PlayerContextBase playerContext = sender as PlayerContextBase;
            if (message == null)
            {
                Log.Error($"消息类型转换错误: {msg.GetType().Name} to {typeof(Message).Name}");
                return;
            }
            if (playerContext == null)
            {
                Log.Error($"玩家上下文转换错误: {playerContext.GetType().Name} to {typeof(PlayerContextBase).Name}");
                return;
            }
            Run(playerContext, message);
        }
    }

    public abstract class AMRpcHandler<Request, Response> : IMHandler 
        where Request : class, IRequest where Response : class, IResponse
    {
        protected static void ReplyError(Response response, Exception e, Action<Response> reply)
        {
            Log.Error(e);
            response.Error = ErrorCode.ERR_RpcFail;
            response.Message = e.ToString();
            reply(response);
        }

        protected abstract void Run(PlayerContextBase playerContext, Request message, Action<Response> reply);

        public void Handle(PlayerContextBase sender, object message)
        {
            try
            {
                Request request = message as Request;
                PlayerContextBase playerContext = sender as PlayerContextBase;
                if (request == null)
                {
                    Log.Error($"消息类型转换错误: {message.GetType().Name} to {typeof(Request).Name}");
                }

                int rpcId = request.RpcId;

                ulong instanceId = playerContext.GetInstanceId();

                this.Run(playerContext, request, response =>
                {
                    // 等回调回来,session可以已经断开了,所以需要判断session InstanceId是否一样
                    if (playerContext.GetInstanceId() != instanceId)
                    {
                        return;
                    }

                    response.RpcId = rpcId;
                    sender.Reply(response);
                });
            }
            catch (Exception e)
            {
                throw new Exception($"解释消息失败: {message.GetType().FullName}", e);
            }
        }

        public Type GetMessageType()
        {
            return typeof(Request);
        }
    }

}
