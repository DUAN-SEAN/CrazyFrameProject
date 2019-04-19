using Crazy.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crazy.ServerBase.Protocol
{
    public interface IMHandler
    {
        void Handle(PlayerContextBase playerContextBase, IMessage message);
        Type GetMessageType();
    }
}
