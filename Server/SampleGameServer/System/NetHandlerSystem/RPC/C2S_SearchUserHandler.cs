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
    public class C2S_SearchUserHandler : AMRpcHandler<C2S_SearchUser, S2C_SearchUser>
    {

        protected override void Run(ISession playerContext, C2S_SearchUser message, Action<S2C_SearchUser> reply)
        {
            
        }

    }
}
