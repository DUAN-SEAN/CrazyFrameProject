using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
namespace SampleGameServer
{
    class Program
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            


            GameServer gameServer = new GameServer();
            if(!gameServer.Initialize<ServerBaseGlobalConfigure,GameServerContext>
                (@"GameServerConfigure.config",typeof(GameServerContext),new ProtobufPacker(), "GameServer"))
            {
                Log.Error("初始化服务器错误");
            }
            while (true)
            {
                string cmd =  Console.ReadLine();
                Log.Fatal(cmd);
            }
        }
    }
}
