using Crazy.ServerBase.Configure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Crazy.NetSharp;

namespace Crazy.ServerBase
{
    public class ServerBase:IServiceEventHandler
    {
        /// <summary>
        /// 服务器初始化 配置文件初始化、协议字典初始化、网络初始化

        /// </summary>
        /// <param name="globalPath">全局配置文件路径</param>
        /// <returns></returns>
        public virtual bool Initialize(string globalPath,Protocol.MessageDispather messageDispather,Protocol.OpcodeTypeDictionary opcodeTypeDictionary)
        {
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

        public Task<IClientEventHandler> OnConnect(IClient client)
        {
            throw new NotImplementedException();
        }

        public void OnException(Exception ex)
        {
            throw new NotImplementedException();
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
        private Int32 m_id;
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
        private Protocol.MessageDispather MessageDispather;
        /// <summary>
        /// 协议字典
        /// </summary>
        private Protocol.OpcodeTypeDictionary OpcodeTypeDic;
    }
}
