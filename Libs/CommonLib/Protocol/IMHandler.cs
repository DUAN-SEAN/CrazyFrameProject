using Crazy.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crazy.Common
{
    public interface IMHandler
    {
        void Handle(object sender, object message);
        Type GetMessageType();
    }


}
