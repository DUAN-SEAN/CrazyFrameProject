using Crazy.NetSharp;
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
    /// 匹配管理器
    /// </summary>
    public class GameMatchManager
    {
        /// <summary>
        /// 保证只执行一次赋值
        /// </summary>
        static GameMatchManager()
        {
            m_instance = new GameMatchManager();
        }

        public GameMatchManager()
        {
            m_gameMatchPlayerCtxQueDic = new ConcurrentDictionary<int, GameMatchPlayerContextQueue>();
        }
        /// <summary>
        /// 根据配置文件初始化匹配管理器
        /// </summary>
        public void Initialize()
        {
            //获取游戏匹配的配置文件,获取所有的游戏匹配信息

            Configure.GameMacthConfig item = GameServer.Instance.m_gameServerGlobalConfig;



            
            
        }




        private readonly ConcurrentDictionary<int, GameMatchPlayerContextQueue> m_gameMatchPlayerCtxQueDic;//玩家匹配队列字典，服务器有若干个匹配队列，由配置文件进行配置

        private static GameMatchManager m_instance;
        public static GameMatchManager Instance { get => m_instance; set => m_instance = value; }
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

        public void AllocPlayerContext(IClientEventHandler playercontext)
        {
            var playerId = playercontext.GetInstanceId();





        }
        /// <summary>
        /// 一个玩家现场选择退出匹配队列 那么整个团队就退出
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


        public class MatchTeam {


            public readonly List<IClientEventHandler> Member = new List<IClientEventHandler>();

            

        }


    }





}
