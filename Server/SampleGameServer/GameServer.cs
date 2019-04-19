using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.ServerBase;

namespace SampleGameServer
{
    public class GameServer:ServerBase
    {
        public GameServer():base()
        {
            m_instance = this;
        }
        protected static new ServerBase m_instance;
        public static new ServerBase Instance
        {
            get { return m_instance; }
        }

    }
}
