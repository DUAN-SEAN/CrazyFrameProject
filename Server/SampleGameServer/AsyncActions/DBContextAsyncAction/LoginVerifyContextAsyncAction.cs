using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGameServer
{
    public class LoginVerifyContextAsyncAction:VerifyContextAsyncAction
    {
        public LoginVerifyContextAsyncAction(GameServerContext gameServerContext,string account,string password) : base(gameServerContext,gameServerContext.m_gameUserId,false,false)
        {
            this.account = account;
            this.password = password;
        }
        /// <summary>
        /// 在这里写具体的登陆逻辑
        /// </summary>
        /// <returns></returns>
        public override Task Execute()
        {
            


            return base.Execute();
        }
        public override void OnResult()
        {
            completionSource.SetResult(true);
            //gameServerContext.Send();
        }
        // RPC消息的任务完成句柄
        private TaskCompletionSource<bool> completionSource;

        private GameServerContext gameServerContext;

        private string account;

        private string password;
    }
}
