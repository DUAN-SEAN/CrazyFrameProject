using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Crazy.Common;

namespace GameServer.Configure
{
    [Serializable]
    [XmlRoot("Configure")]
    public class GameServerGlobalConfig:ServerBaseGlobalConfigure
    {

        /// <summary>
        /// 服务器数据库列表
        /// </summary>
        [XmlArray("DBConfig"), XmlArrayItem("Database")]
        public DBConfigInfo[] DBConfigInfos { get; set; }

        /// <summary>
        /// 服务器配置
        /// </summary>
        [XmlElement("ServerContext")]
        public GameServerContext ServerContext { get; set; }
        /// <summary>
        /// 关卡配置
        /// </summary>
        [XmlArray("BarrierConfig"), XmlArrayItem("Barrier")]
        public GameBarrierConfig[] BarrierConfigs { get; set; }
        /// <summary>
        /// 匹配队伍数
        /// </summary>
        [XmlElement("GameMatchTeam")]
        public GameMatchTeamConfig GameMatchTeam { get; set; }
        /// <summary>
        /// 玩家现场配置
        /// </summary>
        [XmlElement("GameServerPlayerContext")]
        public GameServerPlayerContext GameServerPlayerContext { get; set; }

    }
    [Serializable]
    public class DBConfigInfo
    {
        
       
        [XmlAttribute("ConnectHost")]
        public String ConnectHost { get; set; }

        [XmlAttribute("Port")]
        public UInt16 Port { get; set; }

        [XmlAttribute("DataBase")]
        public String DataBase { get; set; }

        [XmlAttribute("UserName")]
        public String UserName { get; set; }

        [XmlAttribute("Password")]
        public String Password { get; set; }
    }
    [Serializable]
    public class GameServerContext
    {
        [XmlAttribute("AsyncActionQueueCount")]
        public UInt32 AsyncActionQueueCount { get; set; }

        /// <summary>
        /// 心跳包的发送间隔
        /// </summary>
        [XmlAttribute("HeartBeatTimerPeriod")]
        public int HeartBeatTimerPeriod { get; set; }

    }

    /// <summary>
    /// 游戏匹配配置信息，实例代表一个匹配队列
    /// </summary>
    [Serializable]
    public class GameBarrierConfig 
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlAttribute("Level")]
        public int Level { get; set; }


        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("MemberCount")]
        public int MemberCount { get; set; }

    }
    [Serializable]
    public class GameMatchTeamConfig
    {
        //最大队伍数
        [XmlAttribute("TeamMaxCount")]
        public int MaxCount { get; set; }
        //队伍最大容量
        [XmlAttribute("TeamCapacity")]
        public int TeamCapacity { get; set; }
    }
    [Serializable]
    public class GameServerPlayerContext
    {
        /// <summary>
        /// 玩家连接状态下还没有进入认证完成状态的超时时间，时间单位：毫秒
        /// </summary>
        [XmlAttribute("ConnectTimeOut")]
        public Double ConnectTimeOut { get; set; }

        /// <summary>
        /// 玩家断线状态下的超时时间，时间单位：毫秒
        /// </summary>
        [XmlAttribute("DisconnectTimeOut")]
        public Double DisconnectTimeOut { get; set; }

        /// <summary>
        /// sessiontoken的过期时间，时间单位：毫秒
        /// </summary>
        [XmlAttribute("SessionTokenTimeOut")]
        public Double SessionTokenTimeOut { get; set; }
        /// <summary>
        /// AuthToken的过期时间，时间单位：毫秒
        /// </summary>
        [XmlAttribute("AuthTokenTimeOut")]
        public Double AuthTokenTimeOut { get; set; }

        /// <summary>
        /// shutdown过程的超时时间，时间单位：毫秒
        /// </summary>
        [XmlAttribute("ShutdownTimeOut")]
        public Double ShutdownTimeOut { get; set; }
    }

}
