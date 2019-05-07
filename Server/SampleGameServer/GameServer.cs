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

            if(!base.Initialize<GlobalConfigureType, PlayerContextBase>(globalPath, plyaerContextType, messagePraser, serverName))
            {
                return false;
            }
            //获取当前服务器的配置文件
            m_gameServerGlobalConfig = base.m_globalConfigure as SampleGameServer.Configure.GameServerGlobalConfig;
            //设置AsyncActionQueuePool
            AsyncActionQueuePool = new VerifyAsyncActionSequenceQueuePool(m_gameServerGlobalConfig.ServerContext.AsyncActionQueueCount);

            //下面可以写启动逻辑线程 将上述游戏逻辑丢到逻辑线程中处理

            return true;
        }
        /// <summary>
        /// 获取当前服务器特定配置数据
        /// </summary>
        public SampleGameServer.Configure.GameServerGlobalConfig m_gameServerGlobalConfig { get; private set; }
        /// <summary>
        /// 服务器用于顺序化AsyncAction的队列池，根据每个Context的UserId来分配该Context对应的AsyncAction所属的队列
        /// </summary>
        public VerifyAsyncActionSequenceQueuePool AsyncActionQueuePool { get; private set; }
    }
}
