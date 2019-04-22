using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crazy.ServerBase
{
    public class ServerBaseLocalMesssageIDDef
    {
        public const Int32 LocalMsgAsyncActionResult = 2;

        public const Int32 NetMessage = 1001;



    }
    public class ErrorCode
    {
        public const Int32 MessageOk = 0;
        public const Int32 RpcResponseError = 1;

        public static bool IsRpcNeedThrowException(int errorCode)
        {
            if (errorCode == RpcResponseError)
                return true;
            return false;
        }
    }
}
