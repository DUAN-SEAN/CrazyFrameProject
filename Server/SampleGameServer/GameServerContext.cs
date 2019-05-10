using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.NetSharp;
using Crazy.ServerBase;
namespace SampleGameServer
{
    public class GameServerContext:PlayerContextBase
    {
        public override Task OnMessage(ILocalMessage msg)
        {



            return Task.CompletedTask;
        }
    }
}
