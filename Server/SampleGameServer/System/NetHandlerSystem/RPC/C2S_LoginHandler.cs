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
    public class C2S_LoginHandler : AMRpcHandler<C2S_Login, S2C_Login>
    {
        protected override void Run(ISession playerContext, C2S_Login message, Action<S2C_Login> reply)
        {
            GameServerPlayerContext gameServerContext = playerContext as GameServerPlayerContext;
            S2C_Login response = new S2C_Login();


        }
    }
}
