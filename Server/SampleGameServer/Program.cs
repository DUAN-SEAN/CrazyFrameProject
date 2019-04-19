using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.ServerBase.Configure;
namespace SampleGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            GameServer gameServer = new GameServer();
            gameServer.Initialize<ServerBaseGlobalConfigure>()
        }
    }
}
