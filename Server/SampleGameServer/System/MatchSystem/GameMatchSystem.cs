using Crazy.Common;
using Crazy.NetSharp;
using Crazy.ServerBase;
using MongoDB.Bson;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameServer.Battle;

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
            //Task.Delay()
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
                    OnExitMatchQueue(exitMatchQueueMessage.teamId, exitMatchQueueMessage.playerId);
                    break;
                case GameServerConstDefine.MatchQueueCompleteSingle://来自 MatcingSystemQueue 的消息，通知system匹配完成
                    MatchQueueCompleteSingleMessage matchQueueCompleteSingleMessage = msg as MatchQueueCompleteSingleMessage;
                    OnCompleteMatching(matchQueueCompleteSingleMessage.teamIds, matchQueueCompleteSingleMessage.barrierId);
                    break;
                case GameServerConstDefine.MatchSysteamMatchTeamUpdateInfo:
                    MatchTeamUpdateInfoMessage matchTeamUpdateInfoMessage = msg as MatchTeamUpdateInfoMessage;
                    OnUpdateMatchTeam(matchTeamUpdateInfoMessage.teamId);
                    break;
                case GameServerConstDefine.MatchSystemPlayerShutdown:
                    ToMatchPlayerShutdownMessage toMatchPlayerShutdownMessage = msg as ToMatchPlayerShutdownMessage;
                    OnShutdownPlayer(toMatchPlayerShutdownMessage);
                    break;
                case GameServerConstDefine.UpdateOnlinePlayerList://更新客户端在线玩家状态
                    UpdateOnlinePlayerMessage updateOnlinePlayerMessage = msg as UpdateOnlinePlayerMessage;
                    OnUpdateOnlinePlayerRpc(updateOnlinePlayerMessage);
                    break;
                default:
                    break;
            }


            return base.OnMessage(msg);
        }
        /// <summary>
        /// TODO:关闭一个玩家现场在匹配系统的存在
        /// 安全操作，尽管退出
        /// </summary>
        /// <param name="toMatchPlayerShutdownMessage"></param>
        private void OnShutdownPlayer(ToMatchPlayerShutdownMessage message)
        {
            Log.Info("收到了玩家现场发来的关闭消息");
            var playerId = message.playerId;
            var teamId = FindMatchTeamById(playerId);
            MatchTeam matchTeam = null;
            if (teamId == default)
            {
                return;
            }
            if(!m_teamDic.TryGetValue(teamId,out matchTeam))
            {
                return;
            }

            lock (matchTeam)
            {
               
                if (matchTeam.State == MatchTeam.MatchTeamState.INBATTLE)
                {

                    Log.Info("玩家掉线，重连超时，退出队伍  TeamId" + matchTeam.Id + "Team State = " + matchTeam.State);
                    OnExitMatchTeam(teamId, playerId);
                    return;
                }
                //匹配队列正在匹配，需要将队伍从匹配队列中删除
                if (matchTeam.State == MatchTeam.MatchTeamState.Matching)
                {
                    Log.Info("玩家掉线，重连超时，退出队伍  TeamId" + matchTeam.Id + "Team State = " + matchTeam.State);
                    
                    OnExitMatchTeam(teamId, playerId);
                    return;
                }
                //最简单的退出 直接交给退出队伍执行
                if (matchTeam.State == MatchTeam.MatchTeamState.OPEN)
                {
                    Log.Info("玩家掉线，重连超时，退出队伍  TeamId" + matchTeam.Id + "Team State = " + matchTeam.State);
                    OnExitMatchTeam(teamId, playerId);
                    return;
                }
            }
           
        }
        /// <summary>
        /// 玩家重连成功 取得战斗系统玩家实体控制权
        /// </summary>
        /// <param name="playerId"></param>
        public void OnReConnect(ulong battleId,string playerId)
        {

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
            S2C_CreateMatchTeamComplete.Types.State state = S2C_CreateMatchTeamComplete.Types.State.Complete;
            ulong teamId = default;
            if (m_teamDic.Values.Count > m_gameMatchTeamConfig.MaxCount)
            {
                Log.Error("服务器队伍容量过载");
                state = S2C_CreateMatchTeamComplete.Types.State.SystemError;
                goto Result;
            }
            if ((teamId = FindMatchTeamById(playerId))!=default)
            {
                Log.Info("创建队伍失败，因为玩家已经在队伍中了");
                state = S2C_CreateMatchTeamComplete.Types.State.HaveTeam;
                goto Result;
            }
            //创建队伍，返回队伍id 
            UInt64 id = m_roomIdFactory.AllocateSessionId();
            MatchTeam matchTeam = new MatchTeam(id, m_gameMatchTeamConfig.TeamCapacity);//生成一个房间，并通知对应的玩家队伍生成好了
            teamId = matchTeam.Id;
            //加入队伍字典中
            m_teamDic.TryAdd(id, matchTeam);
            matchTeam.Add(playerId);

            Result:
            //向玩家发送 创建并且加入到队伍的消息
            PostLocalMessageToCtx(new SystemSendNetMessage { Message = new S2C_CreateMatchTeamComplete { State = state,MatchTeamId = teamId }, PlayerId = playerId }, playerId);
            Log.Info($"创建房间Id = {teamId} 执行完毕 state = "+state.ToString());

        }
        /// <summary>
        /// 玩家离开队伍
        /// </summary>
        private void OnExitMatchTeam(UInt64 teamId, string playerId)
        {
            //如果离开队伍后 房间人数为0 那么就执行清除任务 并设置房间的状态为Close
            S2CM_ExitMatchTeamComplete message = new S2CM_ExitMatchTeamComplete {State = S2CM_ExitMatchTeamComplete.Types.State.Ok  };
            message.LaunchPlayerId = playerId;
            message.MatchTeamId = teamId;
            MatchTeam matchTeam = null;
            ulong realTeamId = FindMatchTeamById(playerId);//首先不信任客户端 在服务器内找到玩家
            if(realTeamId == default)//找不到说明是个野消息
            {
                message.State = S2CM_ExitMatchTeamComplete.Types.State.Fail;
                goto Result;
            }
            else
            {
                if (realTeamId == teamId)//如果队伍一样 说明是真实消息
                {
                    if (!m_teamDic.TryGetValue(teamId, out matchTeam))//没有队伍
                    {
                        message.State = S2CM_ExitMatchTeamComplete.Types.State.Fail;
                        goto Result;
                    }
                }
                else//否则 就是虚假消息，目前决策是这个用户是个脏用户，也需要被清除，直接从找到的真是队伍中删除
                {
                    if (!m_teamDic.TryGetValue(realTeamId, out matchTeam))//没有队伍
                    {
                        message.State = S2CM_ExitMatchTeamComplete.Types.State.Fail;
                        goto Result;
                    }
                }
            }
            
            if (matchTeam.State == MatchTeam.MatchTeamState.CLOSE )//队伍不开放暂时不能退出，脏的也不让退
            {
                
                message.State = S2CM_ExitMatchTeamComplete.Types.State.Fail;
                Log.Info("由于队伍已经处于关闭所以退出失败 = "+playerId);
                goto Result;
            }

            OnExitMatchQueue(matchTeam.Id, playerId);
            if (!matchTeam.Remove(playerId))//从队伍中移除
            {
                message.State = S2CM_ExitMatchTeamComplete.Types.State.Fail;
                goto Result;
            }
            message.LaunchPlayerId = playerId;
            message.MatchTeamId = matchTeam.Id;
            //TODO:还要向战斗系统发送玩家退出的请求


            Result:
            PostLocalMessageToCtx(new SystemSendNetMessage { Message = message }, playerId);//向发起者发送
            if (matchTeam != null)
            {
                PostLocalMessageToCtx(new SystemSendNetMessage { Message = message }, matchTeam.GetMembers());
                if (matchTeam.CurrentCount <= 0)//如果这时候队伍的人数为空 则执行清除工作 并设置队伍的状态为关闭
                {
                    //可以执行清除任务了
                    if (m_teamDic.TryRemove(matchTeam.Id, out matchTeam))
                    {
                        matchTeam.State = MatchTeam.MatchTeamState.CLOSE;
                    }
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
            S2CM_JoinMatchTeamComplete message = new S2CM_JoinMatchTeamComplete();
            message.State = S2CM_JoinMatchTeamComplete.Types.State.Complete;
            message.LaunchPlayerId = playerId;
            
            ulong realTeamId = 0;
            if ((realTeamId = FindMatchTeamById(playerId)) != default)
            {
                Log.Error("加入队伍失败，因为玩家已经在队伍中了");
                message.State = S2CM_JoinMatchTeamComplete.Types.State.HaveTeam;
                message.MatchTeamId = realTeamId;
                goto Result;
            }
            
            if (!m_teamDic.TryGetValue(teamId, out matchTeam))
            {
                message.State = S2CM_JoinMatchTeamComplete.Types.State.SystemError;
                goto Result;
            }
            //队伍状态检测
            if (matchTeam.State != MatchTeam.MatchTeamState.OPEN)
            {
                message.State = S2CM_JoinMatchTeamComplete.Types.State.SystemError;
                goto Result;
            }

            //检测队伍容量
            if (!matchTeam.IsFull()) {
                message.State = S2CM_JoinMatchTeamComplete.Types.State.SystemError;
                goto Result;
            }
            matchTeam.Add(playerId);//向队伍内添加玩家
            message.LaunchPlayerId = playerId;
            message.MatchTeamId = matchTeam.Id;
            Result:
            if (matchTeam == null)
            {
                PostLocalMessageToCtx(new SystemSendNetMessage { Message = message }, playerId);

            }
            else
            {
                PostLocalMessageToCtx(new SystemSendNetMessage { Message = message }, matchTeam?.GetMembers());
                if (matchTeam.CurrentCount <= 0)//如果这时候队伍的人数为空 则执行清除工作 并设置队伍的状态为关闭
                {
                    //可以执行清除任务了
                    if (m_teamDic.TryRemove(matchTeam.Id, out matchTeam))
                    {
                        matchTeam.State = MatchTeam.MatchTeamState.CLOSE;
                    }
                }
            }


        }

        /// <summary>
        /// 队伍进入匹配队列
        /// 保证是队长发起 保证队伍人数大于0人
        /// </summary>
        public void OnJoinMatchQueue(ulong teamId, string playerId, int barrierId)
        {
            S2CM_JoinMatchQueueComplete message = new S2CM_JoinMatchQueueComplete();
            message.State = S2CM_JoinMatchQueueComplete.Types.State.Fail;
            //1 验证 队伍是否存在
            MatchTeam matchTeam = null;
            GameMatchPlayerContextQueue gameMatchPlayerContextQueue = null;
            if (!m_teamDic.TryGetValue(teamId, out matchTeam))
            {
                goto Result;
            }
            //2 验证队长是否合法
            if (matchTeam.GetCaptainId() != playerId)
            {
                goto Result;
            }
            //3 验证team状态是否满足
            if (matchTeam.State != MatchTeam.MatchTeamState.OPEN)
            {
                goto Result;
            }
            //4 验证是否有对应的关卡匹配队列
            
            if (!m_gameMatchPlayerCtxQueDic.TryGetValue(barrierId, out gameMatchPlayerContextQueue))
            {
                goto Result;
            }
            //5 向匹配队列加入该队伍  方法内修改matchTeam的状态
            gameMatchPlayerContextQueue.OnJoinMatchQueue(matchTeam);
            message.State = S2CM_JoinMatchQueueComplete.Types.State.Ok;
            message.BarrierId = barrierId;
            message.MatchTeamId = teamId;
            Result:
            if(message.State == S2CM_JoinMatchQueueComplete.Types.State.Ok)
            {
                PostLocalMessageToCtx(new SystemSendNetMessage { Message = message }, matchTeam.GetMembers());
            }
            else
            {
                PostLocalMessageToCtx(new SystemSendNetMessage { Message = message }, playerId);
            }
            





        }
        /// <summary>
        /// 队伍离开匹配队列
        /// 任何队伍中的玩家都能发起队伍离开匹配队列
        /// </summary>
        public void OnExitMatchQueue(ulong teamId, string playerId)
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
            //4 获取匹配队列
            var matchQueue = FindMatchTeamQueue(matchTeam);
            if (matchQueue == null) return;
            //5 从匹配队列中删除
            matchQueue.OnExitMatchQueue(matchTeam);
            //6 向队伍内人广播退出匹配队列
            if(matchTeam.State == MatchTeam.MatchTeamState.OPEN)
            {
                PostLocalMessageToCtx(new SystemSendNetMessage { Message = new S2CM_ExitMatchQueue { LaunchPlayerId = playerId, MatchTeamId = teamId, State = S2CM_ExitMatchQueue.Types.State.Client } }, matchTeam?.GetMembers());
            }

        }

        /// <summary>
        /// 匹配成功由MatchQueue触发事件
        /// 匹配系统由于充当队伍系统功能，所以目前匹配系统不删除匹配成功的队伍
        /// </summary>
        public void OnCompleteMatching(List<UInt64> teamIds, int barrierId)
        {
            //检查队伍状态是否合法  合法则修改队伍状态 并向GameServer 发送 启动物理模块开启战斗系统
            bool flag = true;
            foreach (var id in teamIds)
            {
                var team = m_teamDic[id];
                if (team == null)
                {
                    flag = false;
                    Log.Error("找不到id对应的队伍 匹配出错");
                }
                if (team.State != MatchTeam.MatchTeamState.Matching)
                {
                    flag = false;
                    Log.Error("队伍状态错误 匹配出错");
                }
            }
            if (!flag)
            {
                Log.Debug("创建关卡前检查合法队伍时，存在队伍不合法");
                return;

            }
            Log.Info("战斗匹配系统 匹配一个战斗成功 BarrierId = "+barrierId);
            //TODO:向战斗系统发生生成消息 战斗系统内部会向队伍所有玩家发生 战斗生成消息
            CreateBattleBarrierMessage createBattleBarrierMessage = new CreateBattleBarrierMessage();
            foreach(var id in teamIds)
            {
                var team = m_teamDic[id];
                createBattleBarrierMessage.Players.AddRange(team.GetMembers());
                //修改队伍状态 并通知GameServer 向战斗系统发送创建战斗模块的消息
                team.State = MatchTeam.MatchTeamState.INBATTLE;
            }
           
            createBattleBarrierMessage.BarrierId = barrierId;
            GameServer.Instance.PostMessageToSystem<BattleSystem>(createBattleBarrierMessage);
        }
       
        /// <summary>
        /// 刷新队伍信息并且广播给队伍内玩家
        /// </summary>
        private void OnUpdateMatchTeam(ulong teamId)
        {
            S2C_UpdateMatchTeamInfo message = null;
            MatchTeam matchTeam = null;
            if (!m_teamDic.TryGetValue(teamId, out matchTeam))
            {

                return;
            }
            //队伍状态检测
            if (matchTeam.State != MatchTeam.MatchTeamState.OPEN)
            {
                
                goto Result;
            }
            message = new S2C_UpdateMatchTeamInfo();
            message.TeamInfo = new MatchTeamInfo();
            message.TeamInfo.MatchTeamId = teamId;
            message.MatchTeamId = teamId;
            foreach (var item in matchTeam.GetMembers())
            {
                message.TeamInfo.PlayerIds.Add(item);
            }

            Result:
            if (matchTeam != null && message != null)
            {
                PostLocalMessageToCtx(new SystemSendNetMessage { Message = message }, matchTeam?.GetMembers());
            }

        }
        /// <summary>
        /// 接收到用户的RPC请求 获取一次在线玩家列表，并显示在线玩家的状态
        /// </summary>
        /// <param name="message"></param>
        public void OnUpdateOnlinePlayerRpc(UpdateOnlinePlayerMessage message)
        {
            S2C_UpdateOnlinePlayerList response = new S2C_UpdateOnlinePlayerList();
            foreach (var item in message.players)
            {
                S2C_UpdateOnlinePlayerList.Types.OnlinePlayerInfo info = new S2C_UpdateOnlinePlayerList.Types.OnlinePlayerInfo();
                info.PlayerId = item;
                //检查玩家所在地
                info.State = GetPlayerState(item);
                response.OnlinePlayers.Add(info);
            }
            Log.Info(response.ToJson());
            message.reply(response);


            
        }
        /// <summary>
        /// 查找队伍的匹配队列
        /// </summary>
        /// <param name="matchTeam"></param>
        /// <returns></returns>
        private GameMatchPlayerContextQueue FindMatchTeamQueue(MatchTeam matchTeam)
        {
            foreach (var item in m_gameMatchPlayerCtxQueDic.Values)
            {
                if(item.IsContain(matchTeam)) return item;
            }
            return null;
        }

        /// <summary>
        /// 在查找玩家所在队伍
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        private ulong FindMatchTeamById(string playerId)
        {
            foreach(var team in m_teamDic.Values)
            {
                if (team.IsContain(playerId)) return team.Id;
            }
            return default;
        }
        /// <summary>
        /// 获取玩家的状态
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>在队伍里0,1,2,3。不在队伍里4</returns>
        private int GetPlayerState(string playerId)
        {
            foreach(var team in m_teamDic.Values)
            {
                if (team.IsContain(playerId))
                {
                    switch (team.State)
                    {
                        case MatchTeam.MatchTeamState.OPEN:
                            return 1;
                        case MatchTeam.MatchTeamState.INBATTLE:
                            return 2;
                        case MatchTeam.MatchTeamState.Matching:
                            return 3;
                        default:
                            break;
                    }
                }
            }
            return 0;//没有在队伍中 可以进行邀请
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