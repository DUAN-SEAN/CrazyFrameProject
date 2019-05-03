using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
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

        public override bool Initialize<GlobalConfigureType, PlayerContextBase>(string globalPath, Type plyaerContextType, IMessagePacker messagePraser, string serverName)
        {
            TypeManager.Instance.Add(DLLType.Common,Assembly.GetAssembly(typeof(TypeManager)));
            TypeManager.Instance.Add(DLLType.ServerBase, Assembly.GetAssembly(typeof(ServerBase)));
            TypeManager.Instance.Add(DLLType.GameServer, Assembly.GetAssembly(typeof(GameServer)));

            return base.Initialize<GlobalConfigureType, PlayerContextBase>(globalPath, plyaerContextType, messagePraser, serverName);
        }
    }
}
