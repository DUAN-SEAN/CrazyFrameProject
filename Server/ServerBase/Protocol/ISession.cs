using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Crazy.Common;
namespace Crazy.ServerBase
{
    public interface ISession
    {
     
        void Reply(IResponse message);
        void Send(IMessage message);
        void Send(MemoryStream message);
        void Send(byte flag, IMessage message);
        void Send(byte flag, ushort opcode, object message);

        Task<IResponse> Call(IRequest request);
        Task<IResponse> Call(IRequest request, CancellationToken cancellationToken);

        Dictionary<int,Action<IResponse>> GetRPCActionDic();//获取RCP的委托集合
    }

   
}
