using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using Crazy.NetSharp;
using Crazy.ServerBase;

namespace GameServer
{
    public sealed class GameServer:ServerBase
    {
        public GameServer():base()
        {
            m_instance = this;
        }
        /// <summary>
        /// 静态实例
        /// </summary>
        public static new GameServer Instance
        { get { return (GameServer)(ServerBase.Instance); } }

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
            m_gameServerGlobalConfig = base.m_globalConfigure as global::GameServer.Configure.GameServerGlobalConfig;
            //设置AsyncActionQueuePool
            AsyncActionQueuePool = new SampleGameServerAsyncActionSequenceQueuePool(m_gameServerGlobalConfig.ServerContext.AsyncActionQueueCount);

            //数据库配置
            var dbConfig = m_gameServerGlobalConfig.DBConfigInfos[0];
            Log.Info($"ip:{dbConfig.ConnectHost} port:{dbConfig.Port} serviceName:{dbConfig.DataBase} username:{dbConfig.UserName} password:{dbConfig.Password}");

            //MongoDBHelper.CreateDBClient(); //测试
            
            //mongodb测试
            MongoDBHelper.Test();

            //初始化功能服务的各个模块系统
            if (!InitializeSystem())
            {
                Log.Info("初始化模块系统失败");
                return false;
            }




            //下面可以写启动逻辑线程 将上述游戏逻辑丢到逻辑线程中处理
             

            return true;
        }
        /// <summary>
        /// 初始化服务器的各个系统
        /// </summary>
        public bool InitializeSystem()
        {
            // 匹配系统初始化
            if (!m_gameMatchSystem.Initialize())
            {
                Log.Error("初始化匹配系统失败");
                return false;
            }

            return true;
        }


        /// <summary>
        /// 向功能系统发送本地消息
        /// </summary>
        /// <typeparam name="System"></typeparam>
        /// <param name="msg"></param>
        public void PostMessageToSystem<System>(ILocalMessage msg) where System:BaseSystem
        
        {
            BaseSystem baseSystem = m_systemDic[typeof(System)];
            if (baseSystem == null) return;
            baseSystem.PostLocalMessage(msg);
        }


        private readonly Dictionary<Type, BaseSystem> m_systemDic = new Dictionary<Type, BaseSystem>();

        private GameMatchSystem m_gameMatchSystem;
        /// <summary>
        /// 获取当前服务器特定配置数据
        /// </summary>
        public Configure.GameServerGlobalConfig m_gameServerGlobalConfig { get; private set; }
        /// <summary>
        /// 服务器用于顺序化AsyncAction的队列池，根据每个Context的UserId来分配该Context对应的AsyncAction所属的队列
        /// </summary>
        public SampleGameServerAsyncActionSequenceQueuePool AsyncActionQueuePool { get; private set; }
    }
}
