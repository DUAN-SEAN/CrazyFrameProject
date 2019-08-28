using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.NetSharp;
using Crazy.ServerBase;

namespace FileManagerServer
{
    public class FileServerPlayerContext:PlayerContextBase
    {
        public override Task OnMessage(ILocalMessage msg)
        {
            return base.OnMessage(msg);
        }
    }
}
