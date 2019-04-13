using System;
using System.Collections.Generic;
using System.Text;

namespace Crazy.Common
{
    public interface IMHandler
    {
        void Handle(object session, object message);
        Type GetMessageType();
    }
}
