using Crazy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.System.NetHandlerSystem
{

    [MessageHandler]
    public class C2S_CreateMatchTeamMessageHandler : AMHandler<C2S_CreateMatchTeam>
    {
        protected override void Run(ISession playerContext, C2S_CreateMatchTeam message)
        {

        }
    }

    [MessageHandler]
    public class C2S_JoinMatchTeamMessageHandler : AMHandler<C2S_JoinMatchTeam>
    {
        protected override void Run(ISession playerContext, C2S_JoinMatchTeam message)
        {
            
        }
    }

    [MessageHandler]
    public class C2S_JoinMatchQueueMessageHandler : AMHandler<C2S_JoinMatchQueue>
    {
        protected override void Run(ISession playerContext, C2S_JoinMatchQueue message)
        {
            
        }
    }
}
