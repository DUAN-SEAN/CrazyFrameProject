using Crazy.Common;
using Crazy.NetSharp;
using Crazy.ServerBase;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameServer
{
    /// <summary>
    /// 1 匹配系统
    /// 2 包含了队伍模块和匹配模块
    /// 3 保证system的运转是线程安全的
    /// 4 匹配系统面向匹配队伍不面向玩家，玩家在队伍里作为一个整体
    /// </summary>
    public class GameMatchSystem : BaseSystem
    {
        public GameMatchSystem() : base()
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
        public override void Update(int data1 = 0, long data2 = 0, object data3 = null)
        {
            base.Update(data1, data2, data3);
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

            switch (msg.MessageId)
            {
                case GameServerConstDefine.MatchSystemCreateMatchTeam:
                    CreateMatchTeamMessage createMatchTeamMessage = msg as CreateMatchTeamMessage;
                    OnCreateMatchTeam(createMatchTeamMessage.playerId);
                    break;
                case GameServerConstDefine.MatchSystemJoinMatchTeam:
                    JoinMatchTeamMessage joinMatchTeamMessage = msg as JoinMatchTeamMessage;
                    OnJoinMatchTeam(joinMatchTeamMessage.teamId, joinMatchTeamMessage.playerId);
                    break;
                case GameServerConstDefine.MatchSystemExitMatchTeam:
                    ExitMatchTeamMessage exitMatchTeamMessage = msg as ExitMatchTeamMessage;
                    OnExitMatchTeam(exitMatchTeamMessage.teamId, exitMatchTeamMessage.playerId);
                    break;
                case GameServerConstDefine.MatchSystemJoinMatchQueue:
                    JoinMatchQueueMessage joinMatchQueueMessage = msg as JoinMatchQueueMessage;
                    OnJoinMatchQueue(joinMatchQueueMessage.teamId, joinMatchQueueMessage.playerId, joinMatchQueueMessage.barrierId);
                    break;
                case GameServerConstDefine.MatchSystemExitMatchQueue:
                    ExitMatchQueueMessage exitMatchQueueMessage = msg as ExitMatchQueueMessage;
                    OnExitMatchQueue(exitMatchQueueMessage.teamId, exitMatchQueueMessage.playerId, exitMatchQueueMessage.barrierId);
                    break;
                case GameServerConstDefine.MatchQueueCompleteSingle://来自 MatcingSystemQueue 的消息，通知system匹配完成
                    MatchQueueCompleteSingleMessage matchQueueCompleteSingleMessage = msg as MatchQueueCompleteSingleMessage;
                    OnCompleteMatching(matchQueueCompleteSingleMessage.teamIds, matchQueueCompleteSingleMessage.barrierId);
                    break;
                default:
                    break;
            }


            return base.OnMessage(msg);
        }

        public override bool PostLocalMessage(ILocalMessage msg)
        {
            return base.PostLocalMessage(msg);
        }

        #endregion

        /// <summary>
        /// 根据配置文件初始化匹配管理器
        /// 该方法由Server 唯一调用
        /// </summary>
        public bool Initialize(int serverId)
        {
            //初始化匹配队列字典
            m_gameMatchPlayerCtxQueDic = new ConcurrentDictionary<int, GameMatchPlayerContextQueue>();
            //初始化队伍字典
            m_teamDic = new ConcurrentDictionary<UInt64, MatchTeam>();
            //获取关卡配置文件,获取所有的游戏匹配信息
            m_gameBarrierConfigs = GameServer.Instance.m_gameServerGlobalConfig.BarrierConfigs;
            //获取队伍配置文件
            m_gameMatchTeamConfig = GameServer.Instance.m_gameServerGlobalConfig.GameMatchTeam;
            //根据配置文件初始化若干个匹配队列  存入字典中
            foreach (var config in m_gameBarrierConfigs)
            {
                //配置关卡Id 关卡等级 关卡容纳人数
                var queue = new GameMatchPlayerContextQueue(config.Id, config.Level, config.MemberCount, this);
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
            return m_gameBarrierConfigs.Length;
        }
        /// <summary>
        /// 队伍总数
        /// </summary>
        /// <returns></returns>
        public int CountTeam()
        {
            return m_teamDic.Count;
        }
        /// <summary>
        /// 创建一个匹配队伍，由玩家发起的本地消息
        /// 创建的队伍并不直接放入匹配队列，而是通过匹配系统进行管理
        /// 每个MatchTeam 有若干个状态进行控制
        /// </summary>
        private void OnCreateMatchTeam(string playerId)
        {
            if (m_teamDic.Values.Count > m_gameMatchTeamConfig.MaxCount)
            {
                Log.Error("服务器队伍容量过载");
                return;
            }

            //创建队伍，返回队伍id 
            UInt64 id = m_roomIdFactory.AllocateSessionId();
            MatchTeam matchTeam = new MatchTeam(id, m_gameMatchTeamConfig.TeamCapacity);//生成一个房间，并通知对应的玩家队伍生成好了
            //加入队伍字典中
            m_teamDic.TryAdd(id, matchTeam);

            matchTeam.Add(playerId);


            //向玩家发送 创建并且加入到队伍的消息



        }
        /// <summary>
        /// 玩家离开队伍
        /// </summary>
        private void OnExitMatchTeam(UInt64 teamId, string playerId)
        {
            //如果离开队伍后 房间人数为0 那么就执行清除任务 并设置房间的状态为Close
            MatchTeam matchTeam = null;
            if (!m_teamDic.TryGetValue(teamId, out matchTeam))
            {
                return;
            }
            if (matchTeam.State != MatchTeam.MatchTeamState.OPEN)//队伍不开放 不能进入
            {
                return;
            }

            if (!matchTeam.Remove(playerId))//从队伍中移除
            {
                return;
            }
            if (matchTeam.CurrentCount <= 0)//如果这时候队伍的人数为空 则执行清除工作 并设置队伍的状态为关闭
            {
                //可以执行清除任务了
                if (m_teamDic.TryRemove(matchTeam.Id, out matchTeam))
                {
                    matchTeam.State = MatchTeam.MatchTeamState.CLOSE;
                }
            }

        }
        /// <summary>
        /// 玩家加入队伍
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="playerId"></param>
        private void OnJoinMatchTeam(UInt64 teamId, string playerId)
        {
            MatchTeam matchTeam = null;
            if (!m_teamDic.TryGetValue(teamId, out matchTeam))
            {
                return;
            }
            //队伍状态检测
            if (matchTeam.State != MatchTeam.MatchTeamState.OPEN) return;

            //检测队伍容量
            if (!matchTeam.IsFull()) return;

            matchTeam.Add(playerId);
        }

        /// <summary>
        /// 队伍进入匹配队列
        /// 保证是队长发起 保证队伍人数大于0人
        /// </summary>
        public void OnJoinMatchQueue(ulong teamId, string playerId, int barrierId)
        {
            //1 验证 队伍是否存在
            MatchTeam matchTeam = null;
            if (!m_teamDic.TryGetValue(teamId, out matchTeam))
            {
                return;
            }
            //2 验证队长是否合法
            if (matchTeam.GetCaptainId() != playerId)
            {
                return;
            }
            //3 验证team状态是否满足
            if (matchTeam.State != MatchTeam.MatchTeamState.OPEN)
            {
                return;
            }
            //4 验证是否有对应的关卡匹配队列
            GameMatchPlayerContextQueue gameMatchPlayerContextQueue;
            if (!m_gameMatchPlayerCtxQueDic.TryGetValue(barrierId, out gameMatchPlayerContextQueue))
            {
                return;
            }
            //5 向匹配队列加入该队伍  方法内修改matchTeam的状态
            gameMatchPlayerContextQueue.OnJoinMatchQueue(matchTeam);


        }
        /// <summary>
        /// 队伍离开匹配队列
        /// 任何队伍中的玩家都能发起队伍离开匹配队列
        /// </summary>
        public void OnExitMatchQueue(ulong teamId, string playerId, int barrierId)
        {
            //1 验证 队伍是否存在
            MatchTeam matchTeam = null;
            if (!m_teamDic.TryGetValue(teamId, out matchTeam))
            {
                return;
            }
            //2 验证队伍是否包含该玩家
            if (!matchTeam.IsContain(playerId))
            {
                return;
            }
            //3 检查队伍状态是否合法
            if (matchTeam.State != MatchTeam.MatchTeamState.Matching)
            {
                return;
            }

        }

        /// <summary>
        /// 匹配成功由MatchQueue触发事件
        /// 匹配系统由于充当队伍系统功能，所以目前匹配系统不删除匹配成功的队伍
        /// </summary>
        public void OnCompleteMatching(List<UInt64> teamIds, int barrierId)
        {
            //检查队伍状态是否合法  合法则修改队伍状态 并向GameServer 发送 启动物理模块开启战斗系统
            foreach (var id in teamIds)
            {
                var team = m_teamDic[id];
                if (team == null)
                {
                    Log.Error("找不到id对应的队伍 匹配出错");
                    return;
                }
                if (team.State != MatchTeam.MatchTeamState.Matching)
                {
                    Log.Error("队伍状态错误 匹配出错");
                    return;
                }
                //修改队伍状态 并通知GameServer 向战斗系统发送创建战斗模块的消息
                team.State = MatchTeam.MatchTeamState.INBATTLE;
                //TODO:向战斗系统发生生成消息 战斗系统内部会向队伍所有玩家发生 战斗生成消息
                //GameServer.Instance.PostMessageToSystem<BaseSystem>(null);

            }
        }

        /// <summary>
        /// 用来生成唯一的房间Id 
        /// </summary>
        private SessionIdFactory m_roomIdFactory;

        /// <summary>
        /// 关卡配置文件
        /// </summary>
        private Configure.GameBarrierConfig[] m_gameBarrierConfigs;


        /// <summary>
        /// 队伍配置文件
        /// </summary>
        private Configure.GameMatchTeamConfig m_gameMatchTeamConfig;

        /// <summary>
        /// 玩家匹配队列字典，服务器有若干个匹配队列，由配置文件进行配置
        /// </summary>
        private ConcurrentDictionary<int, GameMatchPlayerContextQueue> m_gameMatchPlayerCtxQueDic;

        /// <summary>
        /// 队伍保存字典 该队伍和关卡无关
        /// </summary>
        private ConcurrentDictionary<UInt64, MatchTeam> m_teamDic;



    }
}