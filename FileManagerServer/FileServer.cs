using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using Crazy.ServerBase;

namespace FileManagerServer
{
    public class FileServer:ServerBase
    {
        public FileServer() : base()
        {
            m_instance = this;
        }
        /// <summary>
        /// 静态实例
        /// </summary>
        public new static FileServer Instance => (FileServer)(ServerBase.Instance);

        public override bool Initialize<GlobalConfigureType, PlayerContextBase>(string globalPath, Type plyaerContextType, IMessagePacker messagePraser, string serverName)
        {
            //初始化程序集
            TypeManager.Instance.Add(DLLType.Common, Assembly.GetAssembly(typeof(TypeManager)));
            TypeManager.Instance.Add(DLLType.ServerBase, Assembly.GetAssembly(typeof(ServerBase)));
            TypeManager.Instance.Add(DLLType.GameServer, Assembly.GetAssembly(typeof(FileServer)));

            if (!base.Initialize<GlobalConfigureType, PlayerContextBase>(globalPath, plyaerContextType, messagePraser, serverName))
            {
                return false;
            }
            
            

            return true;
        }

        public override bool InitlizeServerConfigure<TGlobalConfigClass>(string globalConfPath, string serverDNName)
        {
            var initFlag =  base.InitlizeServerConfigure<TGlobalConfigClass>(globalConfPath, serverDNName);
            if (!initFlag) return initFlag;

            foreach (var server in m_globalConfigure.Global.Servers)
            {
                if (server.Name == serverDNName)
                {
                    m_configServer = server;
                    m_serverId = m_configServer.Id;
                }
            }

            return true;
        }
    }
}
