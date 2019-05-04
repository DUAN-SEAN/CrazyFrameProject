using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using Crazy.ServerBase;
namespace SampleGameServer
{
    [MessageHandler]
    public class C2S_SearchUserHandler : AMRpcHandler<C2S_SearchUser, S2C_SearchUser>
    {
        protected override void Run(PlayerContextBase playerContext, C2S_SearchUser message, Action<S2C_SearchUser> reply)
        {
            var response = new S2C_SearchUser();
            try
            {
                Log.Msg(message);

                response.Account = "Sean Duan";

            }catch(Exception e)
            {
                ReplyError(response,e,reply);
                return;
            }
            reply(response);
        }
    }
}
