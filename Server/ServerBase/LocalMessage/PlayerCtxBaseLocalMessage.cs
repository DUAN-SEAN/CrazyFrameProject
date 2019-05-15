using Crazy.Common;
using Crazy.NetSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crazy.ServerBase
{
    /// <summary>
    /// 装载着所有本地逻辑消息
    /// </summary>



    public class NetClientMessage : ILocalMessage
    {
        public int MessageId => ServerBaseLocalMesssageIDDef.NetMessage;

        public MessageInfo MessageInfo;
    }

    public class RpcNetClientMessage : ILocalMessage
    {
        public int MessageId => ServerBaseLocalMesssageIDDef.RpcNetMessage;

        public MessageInfo MessageInfo;
    }
}
