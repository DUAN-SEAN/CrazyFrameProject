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

        // 服务器现场配置
        [XmlElement("ServerContext")]
        public GameServerContext ServerContext { get; set; }

        [XmlArray("MatchConfig"), XmlArrayItem("GameMatch")]
        public GameMacthConfig[] GameMacthConfigs { get; set; }

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
    }

    /// <summary>
    /// 游戏匹配配置信息，实例代表一个匹配队列
    /// </summary>
    [Serializable]
    public class GameMacthConfig 
    {
        [XmlAttribute("Type")]
        public int Type { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("MemberCount")]
        public int MemberCount { get; set; }

    }
          

}
