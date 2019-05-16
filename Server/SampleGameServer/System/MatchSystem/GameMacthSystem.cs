using Crazy.NetSharp;
using Crazy.ServerBase;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleGameServer
{
    /// <summary>
    /// 匹配系统
    /// </summary>
    public class GameMatchSystem:BaseSystem
    {
        /// <summary>
        /// 保证只执行一次赋值
        /// </summary>
        static GameMatchSystem()
        {
            m_instance = new GameMatchSystem();
        }

        public GameMatchSystem()
        {
            
        }

        #region BaseSystem
        public override void Start()
        {
            base.Start();


            m_gameMatchPlayerCtxQueDic = new ConcurrentDictionary<int, GameMatchPlayerContextQueue>();

        }

        public override void Update()
        {
            base.Update();//基类的update 负责驱动本地消息
        }

        public override void Dispose()
        {
            base.Dispose();//基类的Diopse 负责销毁本地消息队列
        }

        public override Task OnMessage(ILocalMessage msg)
        {
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
        public void Initialize()
        {
            //获取游戏匹配的配置文件,获取所有的游戏匹配信息
            m_gameMacthConfigs = GameServer.Instance.m_gameServerGlobalConfig.GameMacthConfigs;
            //根据配置文件初始化若干个匹配队列

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
        /// </summary>
        public void OnCreateMatchTeam()
        {

        }

      


        /// <summary>
        /// 本地匹配配置
        /// </summary>
        private Configure.GameMacthConfig[] m_gameMacthConfigs;

        /// <summary>
        /// 玩家匹配队列字典，服务器有若干个匹配队列，由配置文件进行配置
        /// </summary>
        private  ConcurrentDictionary<int, GameMatchPlayerContextQueue> m_gameMatchPlayerCtxQueDic;

        /// <summary>
        /// 队伍保存字典
        /// </summary>
        private readonly ConcurrentDictionary<int, MatchTeam> m_teamDic = new ConcurrentDictionary<int, MatchTeam>();


        #region 单例
        private static GameMatchSystem m_instance;
        public static GameMatchSystem Instance { get => m_instance; set => m_instance = value; }
        #endregion
    }

    /// <summary>
    /// 游戏匹配的玩家队列
    /// </summary>
    public class GameMatchPlayerContextQueue
    {
        /// <summary>
        /// 初始化匹配队列
        /// </summary>
        /// <param name="maxCount"></param>
        public GameMatchPlayerContextQueue(int maxCount)
        {
            m_maxMemberCount = maxCount;
        }
        /// <summary>
        /// 匹配算法 操作m_matchTeamQue
        /// </summary>
        public void MatchUpdate()
        {
            
        }


        /// <summary>
        /// 一个玩家现场选择退出匹配队列 那么整个团队就退出
        /// 
        /// </summary>
        /// <param name="playerId"></param>
        public void OnExitMatchQueue(ulong playerId)
        {
            OnEnterLock();





            LeaveLock();
        }
        public void ReleasePlayerContext(ulong playerId)
        {
            IClientEventHandler playerContext;

            

            if (m_playerDic.TryRemove(playerId,out playerContext))
            {
                playerContext.OnMessage(null);//向玩家现场发送从队列字典中清除的消息
            }

            
        }

        private void OnEnterLock()
        {
            m_queueLock.Wait();

        }

        private void LeaveLock()
        {
            m_queueLock.Release();
        }


        private List<MatchTeam> m_matchTeamQue;//玩家匹配队列

        /// <summary>
        /// 队列锁 每次只能有一个线程去处理
        /// 这是一个混合锁，在playerContext中使用该锁锁住玩家现场
        /// 该锁将在内核模式自旋 while；然后在一定的阈值过后进行内核模式ThreadSleep
        /// </summary>
        protected SemaphoreSlim m_queueLock = new SemaphoreSlim(1);

        /// <summary>
        /// 该匹配队列所拥有的所有玩家现场
        /// </summary>
        private ConcurrentDictionary<ulong, IClientEventHandler> m_playerDic;

        /// <summary>
        /// 队列中支持最多的队伍人数
        /// </summary>
        private readonly int m_maxMemberCount;


       


    }
    public class MatchTeam
    {

        public enum MatchTeamState
        {
            OPEN,//开放房间
            CLOSE,//关闭房间
            INBATTLE//在战斗
        }

        /// <summary>
        /// 房间玩家列表
        /// </summary>
        public readonly List<IClientEventHandler> Member = new List<IClientEventHandler>();

        public MatchTeamState State { get; set; }

    }




}
