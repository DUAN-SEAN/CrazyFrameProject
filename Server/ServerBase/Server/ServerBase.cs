using Crazy.ServerBase.Configure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Crazy.NetSharp;
using Crazy.Common;
namespace Crazy.ServerBase
{
    public partial class ServerBase:IServiceEventHandler
    {
        public ServerBase()
        {
            m_instance = this;
        }
        /// <summary>
        /// 服务器初始化 配置文件初始化、协议字典初始化、网络初始化
        /// </summary>
        /// <typeparam name="GlobalConfigureType"></typeparam>
        /// <param name="globalPath"></param>
        /// <param name="plyaerContextType"></param>
        /// <param name="messageDispather"></param>
        /// <param name="opcodeTypeDictionary"></param>
        /// <param name="messagePraser"></param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public virtual bool Initialize<GlobalConfigureType>(string globalPath,Type plyaerContextType,MessageDispather messageDispather,OpcodeTypeDictionary opcodeTypeDictionary,IMessagePacker messagePraser,string serverName)
             where GlobalConfigureType : ServerBaseGlobalConfigure, new()
        {
            if (!InitlizeLogConfigure())
            {
                Log.Error("初始化日志系统失败");
                return false;
            }

            if(messageDispather == null)
            {
                Log.Error("messageDispather = null");
                return false;
            }
            if(opcodeTypeDictionary == null)
            {
                Log.Error("opcodeTypeDictionary = null");
                return false;
            }
            MessageDispather = messageDispather;
            OpcodeTypeDic = opcodeTypeDictionary;
            m_messagePraser = messagePraser;
            //初始化配置文件
            if (!InitlizeServerConfigure<GlobalConfigureType>(globalPath,serverName))
            {
                Log.Error("初始化配置文件失败");
                return false;
            }
            //初始化上下文管理器
            if (!InitializePlayerContextManager(plyaerContextType,m_configServer.maxPlayerCtx))
            {
                Log.Error("初始化玩家上下文管理器失败");
                return false;
            }
            //初始化网络
            if (!InitializeNetWork())
            {
                Log.Error("配置网络出现错误");
                return false;
            }

            return true;
        }
        /// <summary>
        /// 初始化网络
        /// </summary>
        private bool InitializeNetWork()
        {
            if(m_configServer == null)
            {
                Log.Error("配置文件未找到当前服务器的配置信息");
                return false;
            }
            
            //生成服务 监听端口
            m_service = new Service();
            if(m_service == null)
            {
                Log.Error("服务启动失败");
                return false;
            }
            m_service.Start(System.Net.IPAddress.Any, 20001, this);
            return true;
        }


        /// <summary>
        /// 关闭服务，之后可以选择重启
        /// </summary>
        /// <returns></returns>
        public virtual bool Dispose()
        {
            return true;
        }
        /// <summary>
        /// 由网络层Service触发的事件
        /// 内部将注册玩家上下文
        /// </summary>
        /// <param name="client">网络层Client</param>
        /// <returns></returns>
        public Task<IClientEventHandler> OnConnect(IClient client)
        {
            if (client == null)
            {
                Log.Error("OnConnect, but client is null");
            }

            Log.Debug("Ready one OnConnect");

            // 创建一个PlayerContext对象
            // 注意这里的PlayerConetext和玩家逻辑线程上下文不一样 这里只允许客户端与服务器的上下文注册
            var playerCtx = PlayerCtxManager.AllocPlayerContext() as PlayerContextBase;
            if (playerCtx == null)
            {
                Log.Error("OnConnect player context allocted failed");
                return Task.FromResult<IClientEventHandler>(null);

            }

       
            client.SetSocketRecvBufferSize(m_globalConfigure.Global.Network.SocketInputBufferLen);
            client.SetSocketSendBufferSize(m_globalConfigure.Global.Network.SocketOutputBufferLen);

            // 将client和ctx关联起来
            playerCtx.AttachClient(client,OpcodeTypeDic);

            // 通知玩家现场连接完成
            playerCtx.OnConnected();

            // 包装一个完成的Task返回 
            return Task.FromResult<IClientEventHandler>(playerCtx);
        }

        public void OnException(Exception ex)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 初始化底层的玩家上下文对象管理器。
        /// </summary>
        /// <param name="playerContextType">业务层最终玩家上下文对象类型，必须有无参数的构造函数。</param>
        /// <param name="playerContextMax"></param>
        /// <returns>初始化是否成功。</returns>
        protected virtual bool InitializePlayerContextManager(Type playerContextType, int playerContextMax = int.MaxValue)
        {
            try
            {
                PlayerCtxManager = new PlayerContextManager();
                PlayerCtxManager.Initialize(playerContextType, m_serverId, playerContextMax);
            }
            catch (Exception e)
            {
                Log.Error($"ServerBase::InitializePlayerContextManager{e}");
                return false;
            }

            return true;
        }

        //配置文件读取 服务器全局配置、游戏配置、NLog配置、mongoDB配置

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <typeparam name="TGlobalConfigClass"></typeparam>
        /// <param name="globalConfPath"></param>
        /// <param name="serverDNName"></param>
        /// <returns></returns>
        protected virtual bool InitlizeServerConfigure<TGlobalConfigClass>(string globalConfPath,
            string serverDNName)
            where TGlobalConfigClass :ServerBaseGlobalConfigure, new()
        {
            //1:读取本地配置文件


            //2:通过查找serverName这唯一的服务器名称查找对应的服务配置并初始化m_server(Global.Server)


            return true;

        }
        protected virtual bool InitlizeLogConfigure()
        {
            Log.Info("初始化日志成功");
            return true;
        }

        Task<NetSharp.IClientEventHandler> IServiceEventHandler.OnConnect(IClient client)
        {
            throw new NotImplementedException();
        }



        //服务器解包和封包机制 采取protobuf

        private IMessagePacker m_messagePraser;
        /// <summary>
        /// 玩家现场管理类
        /// </summary>
        private PlayerContextManager PlayerCtxManager;

        /// <summary>
        /// 提供网络服务
        /// </summary>
        private Service m_service;
        /// <summary>
        /// 配置文件
        /// </summary>
        private ServerBaseGlobalConfigure m_globalConfigure;
        /// <summary>
        /// 服务器Id
        /// </summary>
        private Int32 m_serverId;
        /// <summary>
        /// 服务器名称
        /// </summary>
        private string m_serverName;
        /// <summary>
        ///当前服务器的配置
        /// </summary>
        private Crazy.ServerBase.Configure.Server m_configServer;
        /// <summary>
        /// 消息分发 所有走网络的消息都通过这个进行分发
        /// </summary>
        private MessageDispather MessageDispather;
        /// <summary>
        /// 协议字典
        /// </summary>
        private OpcodeTypeDictionary OpcodeTypeDic;

        /// <summary>
        /// 获取关联ServerBase类的句柄。
        /// 继承类可以覆盖来获得更具体的类型。
        /// </summary>
        protected static ServerBase m_instance;
        public static ServerBase Instance
        {
            get { return m_instance; }
        }
    }
}
