using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using Crazy.NetSharp;
using Crazy.ServerBase;
using MongoDB.Driver;

namespace GameServer
{
    /// <summary>
    /// 玩家现场
    /// </summary>
    public sealed class GameServerPlayerContext:PlayerContextBase
    {
        public GameServerPlayerContext()
        {
            m_csm = CreatePlayerContextStateMachine();
            m_csm.SetStateWithoutCheck(PlayerContextStateMachine.StateIdle);
        }
        /// <summary>
        /// 创建玩家现场状态机
        /// </summary>
        /// <returns></returns>
        private PlayerContextStateMachine CreatePlayerContextStateMachine()
        {
            return new PlayerContextStateMachine();
        }
        /// <summary>
        /// 该事件基类现场没有实现任何逻辑
        /// 只需要添加最新的逻辑即可
        /// </summary>
        public override void OnConnected()
        {
            m_csm.SetStateCheck(PlayerContextStateMachine.EventOnConnected);
        }
        /// <summary>
        /// 重写 但要保留基现场的逻辑
        /// </summary>
        /// <param name="msg">本地消息类型</param>
        /// <returns></returns>
        public override Task OnMessage(ILocalMessage msg)

        {
            switch (msg.MessageId)
            {
                case GameServerConstDefine.LocalMsgGameServerContextTransformOK:
                    return OnLMsgOnContextTransformOk((LocalMessageContextTransformOk)msg);

                case GameServerConstDefine.LocalMsgShutdownContext:
                    return OnLMsgShutdownContext((LocalMessageShutdownContext)msg);
                default:
                    return base.OnMessage(msg);
                   
            }


            return Task.CompletedTask;
        }
        #region GameServerPlayerContext基础设施
        /// <summary>
        /// 验证信息 登陆
        /// 这是我们规定的硬登陆 无论服务器有无应用层的现场关闭否，都将覆盖之
        /// </summary>
        /// <returns></returns>
        public async Task OnAuthByLogin(C2S_LoginMessage message, Action<S2C_LoginMessage> reply)
        {
            string account = message.Account;
            string password = message.Password;
            S2C_LoginMessage response = null;

            //设置现场状态
            if(m_csm.SetStateCheck(PlayerContextStateMachine.EventOnAuthLoginReq) == -1)
            {
                Log.Info("OnAuthByLogin::PlayerContextStateMachine switch Fail to LoginReq");
                return;
            }
            // 进行数据库验证
            var authDB = await ValidateAuthLogin(account, password);
            
            //验证失败则通知客户端
            if (!authDB)
            {
                //设置现场状态
                if (m_csm.SetStateCheck(PlayerContextStateMachine.EventOnAuthLoginFail) == -1)
                {
                    Log.Info("OnAuthByLogin::PlayerContextStateMachine switch Fail to LoginFail");
                    return;
                }
                Log.Info(account + " 验证失败");
                response = new S2C_LoginMessage { PlayerGameId = null, State = authDB ? S2C_LoginMessage.Types.State.Ok : S2C_LoginMessage.Types.State.Fail };
                reply(response);
                return;
            }
            //获取数据库player的实体
            m_gameServerDBPlayer = await GetPlayerFromDBAsync(account);
            if (m_gameServerDBPlayer == null)
            {
                Log.Info("gameServerDBPlayer is NULL");
                return;
            }
            //DB的唯一标识符代表着这个玩家应用层的Id
            m_gameUserId = m_gameServerDBPlayer._id.ToString();

            //将玩家现场注册到应用层上
            var brpc = GameServer.Instance.PlayerCtxManager.RegisterPlayerContextByString(m_gameUserId, this);
            //表示已有旧的玩家现场在服务器中 需要对旧玩家现场的状态进行迁移和销毁
            if (!brpc)
            {
                var oldCtx = GameServer.Instance.PlayerCtxManager.FindPlayerContextByString(m_gameUserId);
                if (oldCtx != null)
                {
                    //TODO:通知旧现场去关闭 逻辑先留着不写
                    oldCtx.PostLocalMessage(new LocalMessageShutdownContext {  m_shutdownRightNow= true});
                }
            }
            //修改玩家现场状态机
            if(m_csm.SetStateCheck(PlayerContextStateMachine.EventOnAuthLoginOK)== -1)
            {
                Log.Info("OnAuthByLogin::PlayerContextStateMachine switch Fail To LoginOk");
                return;
            }
            //由于目前是单服 所以直接将玩家现场状态设置成验证成功状态
            if (m_csm.SetStateCheck(PlayerContextStateMachine.EventOnSessionLoginOK) == -1)
            {
                Log.Info("OnAuthByLogin::PlayerContextStateMachine switch Fail To SessuibLoginOk");
                return;
            }
            //向客户端发送登陆验证成功
            response = new S2C_LoginMessage { PlayerGameId = this.m_gameUserId, State = authDB ? S2C_LoginMessage.Types.State.Ok : S2C_LoginMessage.Types.State.Fail };
            reply(response);
            //TODO:通知应用层登录流程完成
            await OnLoginOk();
            return ;
        }
        /// <summary>
        /// 当登录成功时触发
        /// </summary>
        /// <returns></returns>
        private async Task OnLoginOk()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 软登陆，由于玩家掉线延迟导致断网，选择重连进行验证时候的登陆
        /// 如果存在旧现场需要恢复，则进行恢复现场上下文
        /// </summary>
        /// <returns></returns>
        public async Task OnAuthReCByLogin(C2S_ReConnectByLogin message)
        {
            string account = message.Account;
            string password = message.Password;
            S2C_LoginMessage response = null;

            //设置现场状态
            if (m_csm.SetStateCheck(PlayerContextStateMachine.EventOnSessionLoginReq) == -1)
            {
                Log.Info("OnAuthByLogin::PlayerContextStateMachine switch Fail to SessionLoginReq");
                return;
            }
            // 进行数据库验证
            var authDB = await ValidateAuthLogin(account, password);
            if (!authDB)
            {
                Log.Info("OnAuthReCByLogin::FAIL TO authDB");
                //验证Session失败先不理会
                goto RETURN;
            }
            //获取数据库player的实体
            m_gameServerDBPlayer = await GetPlayerFromDBAsync(account);
            if (m_gameServerDBPlayer == null)
            {
                Log.Info("gameServerDBPlayer is NULL");
                goto RETURN;
            }
            //DB的唯一标识符代表着这个玩家应用层的Id
            m_gameUserId = m_gameServerDBPlayer._id.ToString();

            //开始验证是否已经存在userid对应的玩家现场
            GameServerPlayerContext oldCtx = (GameServerPlayerContext)GameServer.
                Instance.PlayerCtxManager.FindPlayerContextByString(m_gameUserId);
            //如果存在 则判定为需要重连 需要恢复现场状态
            //这里可以判断设备Id 判断是否需要重连 或者是挤掉原来老的现场设备
            //PS：以后Session验证要更加完全 不能只验证账号密码
            if (oldCtx != null)
            {

                bool ret = await ContextTransform(oldCtx);
                if (!ret)
                {
                    
                    
                }
                // 在现场转移之后不能执行任何代码了，立刻退出
                return;
            }
            //当前玩家验证成功且不存在断线则设置正确的玩家状态
            RETURN:
            if (authDB)
            {
                m_csm.SetStateCheck(PlayerContextStateMachine.EventOnSessionLoginOK);
            }
            //TODO:添加发给客户端的验证成功且不重连的消息
            //Send(null); 
            return ;
        }
        private async Task<bool> ContextTransform(GameServerPlayerContext target)
        {
            await target.EnterLock();//要操作目标现场需要先把目标现场上锁


            if (!target.CanAcceptContextTransform())
            {
                Log.Info("GameServerPlayerContext::ContextTransform can not AcceptContextTransform. target sessionId = "+ target.SessionId+" state = "+  target.m_csm.State);
                // 释放访问权限
                target.LeaveLock();
                return false;
            }
            bool resetSucess = false;
            if (!m_client.Closed && !m_client.Disconnected)
            {
                // 此处需要先处理掉前面一个现场的连接，因为close后不会再收到任何回调，所以不用调用m_client.ResetCientEventHandler(null);
                target.m_client.Close();

                target.m_client = m_client;
                m_client.ResetCientEventHandler(target);
                m_client = null;

                resetSucess = true;
            }

            // 释放访问权限
            target.LeaveLock();

            Log.Info("GameServerBasePlayerContext::ContextTransform resetSucess="+ resetSucess);

            // 向目标现场发送msg，来执行后续操作
            if (resetSucess)
            {
                target.PostLocalMessage(new LocalMessageContextTransformOk());
            }

            // 由于已经没有client对象所以本现场不易久留
            Release();

            return resetSucess;
        }
        /// <summary>
        /// 现场当前是否允许接受现场转移
        /// </summary>
        /// <returns></returns>
        private bool CanAcceptContextTransform()
        {
            // 去除断线的判断逻辑，只通过状态机来检查状态
            //if (!IsDisconnected())
            //{
            //    return false;
            //}
            if (m_csm.EventCheck(PlayerContextStateMachine.EventOnConextTransformOK) == false)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 根据昵称获取数据库player的实体
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private async Task<GameServerDBPlayer> GetPlayerFromDBAsync(string account)
        {
            //获取要执行操作的数据库collection
            var dataBase = MongoDBHelper.GetDataBaseEntity(SampleGameServerDBItemDefine.DATABASE);

            var collection = dataBase.GetCollection<GameServerDBPlayer>(SampleGameServerDBItemDefine.COLLECTION_PLAYERS);//获取Players集合

            var filterBuilder = Builders<GameServerDBPlayer>.Filter;

            var filter = filterBuilder.Eq(SampleGameServerDBItemDefine.PLAYER_USERNAME, account);

            var result = (await(collection.FindAsync(filter))).ToList();

            if (result != null && result.Count > 0)
            {
                return result[0];
            }

            return null;
        }

        /// <summary>
        /// 数据库验证登陆信息的有效性
        /// 目前通过昵称和密码进行登陆
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<bool> ValidateAuthLogin(string account,string password)
        {

            //获取要执行操作的数据库collection
            var dataBase = MongoDBHelper.GetDataBaseEntity(SampleGameServerDBItemDefine.DATABASE);

            var collection = dataBase.GetCollection<GameServerDBPlayer>(SampleGameServerDBItemDefine.COLLECTION_PLAYERS);//获取Players集合

            var filterBuilder = Builders<GameServerDBPlayer>.Filter;

            var filter = filterBuilder.Eq(SampleGameServerDBItemDefine.PLAYER_USERNAME, account) & filterBuilder.Eq(SampleGameServerDBItemDefine.PLAYER_PASSWORD, password);

            var result = (await(collection.FindAsync(filter))).ToList();

            if (result != null && result.Count > 0)
            {             
                return true;
            }
            return false;
        }
        /// <summary>
        /// 该方法由Base回调，将实行本层在TimerManger注册的TimerNode
        /// 例如 每隔500毫秒一次心跳包，5秒一次玩家现场状态检查等
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected override Task OnPlayerContextTimer(PlayerTimerMessage msg)
        {

            return Task.CompletedTask;
        }
        /// <summary>
        /// 当断线重连恢复完成
        /// </summary>
        /// <param name="msg">恢复当前玩家现场本地消息</param>
        /// <returns></returns>
        private Task OnLMsgOnContextTransformOk(LocalMessageContextTransformOk msg)
        {
            Boolean bRet;
         

            if (msg == null)
            {
                Log.Error("GameServerPlayerContext::OnLMsgOnContextTransformOk, but lmsg is null");
                return Task.CompletedTask;
            }

            // 先测试一下状态
            //if (_csm.SetStateCheck(GamePlayerContextBaseStateMachine.EVENT_ONCONTEXTTRANSFORMOK, -1, true) == -1)
            //{
            //    this.LogError("GameServerBasePlayerContext::OnLMsgOnContextTransformOk SetStateCheck EVENT_ONCONTEXTTRANSFORMOK failed");
            //    // 关键流程错误，状态不对不确认是否需要向客户端发送 sessionloginack，所以直接关闭
            //    ShutdownContext(true);
            //    return Task.CompletedTask;
            //}

            Log.Debug("GameServerPlayerContext::OnLMsgOnContextTransformOk, lmsg.MessageId="+ msg.MessageId.ToString());

            // 发送LoginBySessionTokenAck，这里其实是发送给新联上的客户端对象的
            //Send() 
          

            // 直到最后才能确认已经成功完成sessiontokenlogin
            m_csm.SetStateCheck(PlayerContextStateMachine.EventOnConextTransformOK);

            // 当从断线重联恢复
            OnResumeFromWaitForReconnectStart();

            return Task.CompletedTask;
        }
        /// <summary>
        /// 当从断线重联恢复
        /// </summary>
        private void OnResumeFromWaitForReconnectStart()
        {
            // 重置状态时间
            m_disconnectedTime = DateTime.MaxValue;
            m_shutdownTime = DateTime.MaxValue;
            m_disconnetedWaitTimeOutTime = DateTime.MaxValue;
            //TODO:通知 Server 玩家线程需要恢复，要求各个系统得到感知

            m_resumingFromWaitForReconnect = true;
        }

        /// <summary>
        /// 断线重联完成
        /// </summary>
        private void OnResumeFromWaitForReconnectEnd()
        {
            //现场恢复完成后要标记重连完成记号
            m_resumingFromWaitForReconnect = false;
        }
        /// <summary>
        /// 关闭当前玩家现场事件
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private Task OnLMsgShutdownContext(LocalMessageShutdownContext msg)
        {

            if (msg == null)
            {
                Log.Error("GameServerPlayerContext::OnLMsgShutdownContext, but lmsg is null");
                return Task.CompletedTask;
            }
            Log.Debug("GameServerPlayerContext::OnLMsgShutdownContext, lmsg.MessageId="+msg.MessageId.ToString());

            //TODO:通知客户端断开连接
            //Send();
            
            return ShutdownContext(msg.m_shutdownRightNow);
        }

        /// <summary>
        /// 关闭现场
        /// 内部将切断与客户端的连接Client
        /// 如果设置立即切断 则不等待通信层Client通知
        /// </summary>
        /// <param name="shutdownRightNow">是否立即退出</param>
        /// <returns></returns>
        public Task ShutdownContext(bool shutdownRightNow = false)
        {
            Boolean needDisconnect = false;
            Boolean needCloseTickTimer = false;
            Boolean needCloseClient = false;
            Boolean needRelease = false;
            // 设置shutdown开始时间
            if (m_shutdownTime == DateTime.MaxValue)
            {
                m_shutdownTime = DateTime.Now;
            }
            // 计算接下来处理标记的逻辑
            CalcShutdownPlayerCtxNextActionFlagByState(m_csm, ref needDisconnect, ref needCloseTickTimer, ref needCloseClient, ref needRelease);
            // 如果需要断开连接，则先发起断开连接等待OnDisconnected中再回到shutdown
            if (needDisconnect && m_client != null)
            {
                m_client.Disconnect();

                // 当不需要立刻关闭的时候等待Ondisconnect再次驱动shutdown
                if (!shutdownRightNow)
                {
                    return Task.CompletedTask;
                }
            }

            // 停止timer
            if (needCloseTickTimer)
            {
                if (m_playerCtxTimerId != Int64.MinValue)
                {
                    GameServer.Instance.TimerManager.UnsetPlayerContextTimer(m_playerCtxTimerId);
                }
                if (m_heartBeatTimerId != Int64.MinValue)
                {
                    GameServer.Instance.TimerManager.UnsetPlayerContextTimer(m_heartBeatTimerId);
                }
            }

            // 关闭客户端对象
            if (needCloseClient && m_client != null)
            {
                m_client.Close();
            }

            // 释放玩家现场
            if (needRelease)
            {
                Release();//CtxManager Free Context
            }


            return Task.CompletedTask;

        }
        /// <summary>
        /// 由通信层Client调用 用于通知玩家现场 我要和客户端断开连接
        /// 简单的说由Client感知客户端掉线情况
        /// 后期会加上心跳包去激发Client的检测功能
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnected()
        {
            // 由于基类调用了 ServerBase.Instance.PlayerCtxManager.FreePlayerContext(this); 
            // 所以这里要实现延时删除必须覆盖基类实现,在Shutdown中 最终释放现场


            // 设置状态，等待timer来进行删除
            if (m_csm.SetStateCheck(PlayerContextStateMachine.EventOnDisconnected) == -1)
            {
                Log.Info("GameServerBasePlayerContext::OnDisconnected SetStateCheck EVENT_ONDISCONNECTED failed");
                return Task.CompletedTask;
            }
            // 如果已经在shutdown过程中 
            if (m_shutdownTime != DateTime.MaxValue)
            {
                ShutdownContext(true);
                return Task.CompletedTask;
            }
            // 如果不处于断线等待状态也要关闭现场 也就是已经在断线状态了 也要关闭
            if (m_csm.State == PlayerContextStateMachine.StateDisconnected)
            {
                ShutdownContext();
                return Task.CompletedTask;
            }
            return Task.CompletedTask ;
        }
        /// <summary>
        /// 根据当前的状态机状态，得到在关闭ctx时后续处理标记
        /// </summary>
        /// <param name="csm"></param>
        /// <param name="needDisconnect"></param>
        /// <param name="needCloseTickTimer"></param>
        /// <param name="needCloseClient"></param>
        /// <param name="needRelease"></param>
        private void CalcShutdownPlayerCtxNextActionFlagByState(StateMachine csm, ref bool needDisconnect, ref bool needCloseTickTimer, ref bool needCloseClient, ref bool needRelease)
        {
            switch (m_csm.State)
            {
                case PlayerContextStateMachine.StateIdle:
                    needRelease = true;
                    break;
                case PlayerContextStateMachine.StateConnected:
                    needDisconnect = true;
                    needCloseTickTimer = true;
                    needCloseClient = true;
                    needRelease = true;
                    break;
                case PlayerContextStateMachine.StateAuthLoginOK:
                    needDisconnect = true;
                    needCloseTickTimer = true;
                    needCloseClient = true;
                    needRelease = true;
                    break;
                case PlayerContextStateMachine.StateSessionLoginOK:
                    needDisconnect = true;
                    needCloseTickTimer = true;
                    needCloseClient = true;
                    needRelease = true;
                    break;
                case PlayerContextStateMachine.StateDisconnecting:
                    needCloseTickTimer = true;
                    needCloseClient = true;
                    needRelease = true;
                    break;
                case PlayerContextStateMachine.StateDisconnected:
                    needCloseTickTimer = true;
                    needCloseClient = true;
                    needRelease = true;
                    break;
                case PlayerContextStateMachine.StateDisconnectedWaitForReconnect:
                    needCloseTickTimer = true;
                    needCloseClient = true;
                    needRelease = true;
                    break;

                case PlayerContextStateMachine.StateEnteredGame:
                    needDisconnect = true;
                    needCloseTickTimer = true;
                    needCloseClient = true;
                    needRelease = true;
                    break;
                default:
                    break;
            }
        }
        #endregion


        /// <summary>
        /// 数据库代表玩家的实体player
        /// </summary>
        private GameServerDBPlayer m_gameServerDBPlayer;
        /// <summary>
        /// 玩家现场状态机
        /// </summary>
        private PlayerContextStateMachine m_csm;
        /// <summary>
        /// 现场关闭用时
        /// </summary>
        private DateTime m_shutdownTime = DateTime.MaxValue;
        /// <summary>
        /// 记录断线的时间
        /// </summary>
        protected DateTime m_disconnectedTime = DateTime.MaxValue;
        /// <summary>
        /// 记录的断线超时时间
        /// </summary>
        protected DateTime m_disconnetedWaitTimeOutTime = DateTime.MaxValue;
        /// <summary>
        /// 是否正在断线重联的恢复中
        /// </summary>
        protected bool m_resumingFromWaitForReconnect;

        /// <summary>
        /// 用来tick玩家现场的timerid
        /// </summary>
        private Int64 m_playerCtxTimerId = Int64.MinValue;
        private const Int32 PLAYERTIMER_INT32CODE_TICK = 1;

        private Int64 m_heartBeatTimerId = Int64.MinValue;
        private const Int32 HEARBEATTIMER_INT32CODE_TICK = 2;
    }
}
