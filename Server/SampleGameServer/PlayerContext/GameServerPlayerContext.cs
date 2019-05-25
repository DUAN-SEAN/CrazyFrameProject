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
        /// 直接重写
        /// </summary>
        /// <param name="msg">本地消息类型</param>
        /// <returns></returns>
        public override Task OnMessage(ILocalMessage msg)
        {
            switch (msg.MessageId)
            {
                
                default:
                    return base.OnMessage(msg);
                   
            }


            return Task.CompletedTask;
        }
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

            // 进行数据库验证
            var authDB = await ValidateAuthLogin(account, password);
            
            //验证失败则通知客户端
            if (!authDB)
            {
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
                    //通知旧现场去关闭 逻辑先留着不写
                }
            }
            //修改玩家现场状态机




            //向客户端发送登陆验证成功
            response = new S2C_LoginMessage { PlayerGameId = this.m_gameUserId, State = authDB ? S2C_LoginMessage.Types.State.Ok : S2C_LoginMessage.Types.State.Fail };

            reply(response);

            return ;
        }
        /// <summary>
        /// 软登陆，由于玩家掉线延迟导致断网，选择重连进行验证时候的登陆
        /// 如果存在旧现场需要恢复，则进行恢复现场上下文
        /// </summary>
        /// <returns></returns>
        public Task OnAuthBySession()
        {


            return Task.CompletedTask;
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
                Release();
            }


            return Task.CompletedTask;

        }
        public override Task OnDisconnected()
        {

            return base.OnDisconnected();
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
        private GameServerDBPlayer m_gameServerDBPlayer;
        /// <summary>
        /// 玩家现场状态机
        /// </summary>
        private PlayerContextStateMachine m_csm;
        /// <summary>
        /// 现场关闭用时
        /// </summary>
        protected DateTime m_shutdownTime = DateTime.MaxValue;

        /// <summary>
        /// 用来tick玩家现场的timerid
        /// </summary>
        protected Int64 m_playerCtxTimerId = Int64.MinValue;
        protected const Int32 PLAYERTIMER_INT32CODE_TICK = 1;

        private Int64 m_heartBeatTimerId = Int64.MinValue;
        protected const Int32 HEARBEATTIMER_INT32CODE_TICK = 2;
    }
}
