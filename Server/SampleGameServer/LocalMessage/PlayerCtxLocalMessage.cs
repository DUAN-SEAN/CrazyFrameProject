using Crazy.NetSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGameServer
{
    public class PlayerCtxLocalMessage : ILocalMessage
    {
        public int MessageId => GameServerConstDefine.MacthQueuePlayerCtxDic;



    }
}
