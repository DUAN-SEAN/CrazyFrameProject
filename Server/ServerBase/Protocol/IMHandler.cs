using Crazy.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Crazy.ServerBase;
namespace Crazy.Common
{
    public interface IMHandler
    {
        void Handle(PlayerContextBase sender, object message);
        Type GetMessageType();
    }


}
