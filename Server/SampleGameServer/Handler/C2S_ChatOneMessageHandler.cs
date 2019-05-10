using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
namespace Crazy.ServerBase
{
    [MessageHandler]
    public class C2S_ChatOneMessageHandler : AMHandler<ChatOneMessage>
    {
        protected override void Run(ISession playerContext, ChatOneMessage message)
        {
            
        }
    }
}
