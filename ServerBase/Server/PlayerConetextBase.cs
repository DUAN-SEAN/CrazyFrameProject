using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.NetSharp;
namespace Crazy.ServerBase
{
    public class PlayerConetextBase : IClientEventHandler
    {
        #region IClientEventHandler
        public Task<int> OnData(byte[] buffer, int dataAvailable)
        {
            throw new NotImplementedException();
        }

        public Task OnDisconnected()
        {
            throw new NotImplementedException();
        }

        public void OnException(Exception e)
        {
            throw new NotImplementedException();
        }

        public Task OnMessage(ILocalMessage msg)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
