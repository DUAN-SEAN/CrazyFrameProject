using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
namespace Crazy.ServerBase
{
    [MessageHandler]
    public class C2S_LoginMessageHandler : AMHandler<C2S_SearchUser>
    {
        protected override void Run(PlayerContextBase playerContext, C2S_SearchUser message)
        {
            
        }
    }
}
