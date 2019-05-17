using Crazy.Common;
using Crazy.NetSharp;
using Crazy.ServerBase;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer
{
    /// <summary>
    /// 匹配系统
    /// </summary>
    public class GameMatchSystem:BaseSystem
    {
        /// <summary>
        /// 保证只执行一次赋值
        /// </summary>
     

        public GameMatchSystem():base()
        {
            
        }

        #region BaseSystem
        public override void Start()
        {
            base.Start();//初始化本地消息队列


            
        }

        public override void Update()
        {
            base.Update();//基类的update 负责驱动本地消息

            foreach (var item in m_gameMatchPlayerCtxQueDic.Values)
            {
                item.MatchUpdate();// 每一个匹配队列进行刷新   队列元素已经上锁 放心使用
            }


        }

        public override void Dispose()
        {
            base.Dispose();//基类的Diopse 负责销毁本地消息队列
        }
        /// <summary>
        /// 以下要处理收到的消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public override Task OnMessage(ILocalMessage msg)
        {
            //写正常的逻辑代码，1  先



            return base.OnMessage(msg);
        }

        public override bool PostLocalMessage(ILocalMessage msg)
        {
            return base.PostLocalMessage(msg);
        }

        #endregion

        /// <summary>
        /// 根据配置文件初始化匹配管理器
        /// </summary>
        public bool Initialize(int serverId)
        {
            //初始化匹配队列字典
            m_gameMatchPlayerCtxQueDic = new ConcurrentDictionary<int, GameMatchPlayerContextQueue>();
            //初始化队伍字典
            m_teamDic = new ConcurrentDictionary<UInt64, MatchTeam>();
            //获取游戏匹配的配置文件,获取所有的游戏匹配信息
            m_gameMacthConfigs = GameServer.Instance.m_gameServerGlobalConfig.GameMacthConfigs;
            //根据配置文件初始化若干个匹配队列  存入字典中
            foreach (var config in m_gameMacthConfigs)
            {
                var queue = new GameMatchPlayerContextQueue(config.Id, config.Level, config.MemberCount);
                m_gameMatchPlayerCtxQueDic.TryAdd(config.Id, queue);
            }
            //初始化设置Id生成工具  ps:id由线程安全进行累加 
            m_roomIdFactory = new SessionIdFactory(serverId);


            return true;
        }



     
        /// <summary>
        /// 获取匹配队列的个数
        /// </summary>
        /// <returns></returns>
        public int CountMatchQueue()
        {
            return m_gameMacthConfigs.Length;
        }

        public int CountTeam()
        {
            return m_teamDic.Count;
        }


        /// <summary>
        /// 创建一个匹配队伍，由玩家发起的本地消息
        /// 创建的队伍并不直接放入匹配队列，而是通过匹配系统进行管理
        /// 每个MatchTeam 有若干个状态进行控制
        /// </summary>
        public void OnCreateMatchTeam()
        {
            //创建队伍，返回队伍id 
            UInt64 id = m_roomIdFactory.AllocateSessionId();
            MatchTeam matchTeam = new MatchTeam(id);//生成一个房间，并通知对应的玩家队伍生成好了
            //加入队伍字典中
            m_teamDic.TryAdd(id, matchTeam);

        }
        /// <summary>
        /// 玩家离开队伍
        /// </summary>
        public void OnLeaveMatchTeam(UInt64 roomId,UInt64 playerId)
        {
            //如果离开队伍后 房间人数为0 那么就执行清除任务 并设置房间的状态为Close
        }
        /// <summary>
        /// 队伍进入匹配队列
        /// 保证是队长发起 保证队伍人数大于0人
        /// </summary>
        public void OnJoinMatchQueue()
        {

        }
        /// <summary>
        /// 队伍离开匹配队列
        /// 任何队伍中的玩家都能发起队伍离开匹配队列
        /// </summary>
        public void OnLeaveMatchQueue()
        {

        }

        /// <summary>
        /// 用来生成唯一的房间Id 
        /// </summary>
        private SessionIdFactory m_roomIdFactory;

        /// <summary>
        /// 本地匹配配置
        /// </summary>
        private Configure.GameMacthConfig[] m_gameMacthConfigs;

        /// <summary>
        /// 玩家匹配队列字典，服务器有若干个匹配队列，由配置文件进行配置
        /// </summary>
        private  ConcurrentDictionary<int, GameMatchPlayerContextQueue> m_gameMatchPlayerCtxQueDic;

        /// <summary>
        /// 队伍保存字典 该队伍和关卡无关
        /// </summary>
        private ConcurrentDictionary<UInt64, MatchTeam> m_teamDic;


        
    }

    /// <summary>
    /// 游戏匹配的玩家队列
    /// </summary>
   
   
   




}
