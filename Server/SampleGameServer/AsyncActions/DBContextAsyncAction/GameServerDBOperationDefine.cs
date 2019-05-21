using Crazy.Common;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    /// <summary>
    /// 注册验证
    /// </summary>
    public class RegsiterVerifyContextAsyncAction : SampleGameServerContextAsyncAction
    {
        public RegsiterVerifyContextAsyncAction(GameServerPlayerContext context,  string username, string password, Action<S2C_RegisterMessage> reply , bool needResult = false, bool needSeq = false) : base(context, context.ContextId.ToString(), needResult, needSeq)
        {
            m_username = username;
            m_password = password;
            m_state = false;
            m_errorInfo = "";
            this.m_reply = reply;
        }

        public override async Task ExecuteAsync()
        {
            try
            {
                //获取要执行操作的数据库collection
                var dataBase = MongoDBHelper.GetDataBaseEntity(SampleGameServerDBItemDefine.DATABASE);

                var collection = dataBase.GetCollection<GameServerDBPlayer>(SampleGameServerDBItemDefine.COLLECTION_PLAYERS);//获取Players集合

                var filterBuilder = Builders<GameServerDBPlayer>.Filter;

                var filter = filterBuilder.Eq(SampleGameServerDBItemDefine.PLAYER_USERNAME, m_username);// & filterBuilder.Eq("password", m_password);


                var result = (await (collection.FindAsync(filter))).ToList();

                if (result != null && result.Count > 0)
                {

                    m_state = false;
                    m_errorInfo += "已经存在用户了\n";
  
                }
                else
                {
                    var player = new GameServerDBPlayer();

                    player.userName = m_username;

                    player.passWord = m_password;


                    player.createTime = DateTime.UtcNow;

                    await collection.InsertOneAsync(player);//插入
                    m_errorInfo += "插入成功\n";
                    m_state = true;
                }

               
                
            } catch (Exception e)
            {
                m_errorInfo += e.ToString();
                m_state = false;
            }
            finally
            {
                Log.Info(m_errorInfo);
            }


        }
        /// <summary>
        /// 在玩家现场中OnMessage中回调
        /// </summary>
        public override void OnResult()
        {
            Log.Info("注册回调执行成功");
            S2C_RegisterMessage response = new S2C_RegisterMessage { State = m_state? S2C_RegisterMessage.Types.State.Ok : S2C_RegisterMessage.Types.State.Fail };
            m_reply(response);

            base.OnResult();
        }
        private bool m_state;
        private string m_errorInfo;
        private string m_username;
        private string m_password;
        private Action<S2C_RegisterMessage> m_reply;
    }


    /// <summary>
    /// 登陆验证
    /// 5.21 测试成功 要设置允许现场进行回调
    /// </summary>
    public class LoginVerifyContextAsyncAction : SampleGameServerContextAsyncAction
    {
        public LoginVerifyContextAsyncAction(GameServerPlayerContext gameServerContext, string account, string password, Action<S2C_LoginMessage> reply, bool needResult = false, bool needSeq = false) : base(gameServerContext, gameServerContext.ContextId.ToString(), needResult, needSeq)
        {
            this.m_gameServerContext = gameServerContext;
            this.m_username = account;
            this.m_password = password;
            this.m_reply = reply;
            this.m_state = false;
        }
        /// <summary>
        /// 在这里写具体的登陆逻辑
        /// </summary>
        /// <returns></returns>
        public override async Task ExecuteAsync()
        {
            try
            {
                //获取要执行操作的数据库collection
                var dataBase = MongoDBHelper.GetDataBaseEntity(SampleGameServerDBItemDefine.DATABASE);

                var collection = dataBase.GetCollection<GameServerDBPlayer>(SampleGameServerDBItemDefine.COLLECTION_PLAYERS);//获取Players集合

                var filterBuilder = Builders<GameServerDBPlayer>.Filter;

                var filter = filterBuilder.Eq(SampleGameServerDBItemDefine.PLAYER_USERNAME, m_username) & filterBuilder.Eq(SampleGameServerDBItemDefine.PLAYER_PASSWORD, m_password);

                var result = (await (collection.FindAsync(filter))).ToList();

                if (result != null && result.Count > 0)
                {
                    m_state = true;
                    Log.Info("{m_username}:{m_password} 登陆成功！");
                    return;

                }


            }
            catch (Exception e)
            {
                Log.Error($"登陆异常! \n{e}");
            }


            return;
        }
        public override void OnResult()
        {
            
            
            m_response = new S2C_LoginMessage { PlayerId = m_gameServerContext.ContextId, State = m_state? S2C_LoginMessage.Types.State.Ok:S2C_LoginMessage.Types.State.Fail};
            Log.Info("Login 回调:"+m_response.ToJson());
            m_reply(m_response);
            //gameServerContext.Send();
        }
        // RPC消息的任务完成句柄
        

        private GameServerPlayerContext m_gameServerContext;

        private string m_username;

        private string m_password;

        private bool m_state;

        private Action<S2C_LoginMessage> m_reply;

        private S2C_LoginMessage m_response;
    }
    /// <summary>
    /// 上传一场战斗数据
    /// </summary>
    public class UpLoadBarrierRecordAsyncAction : SampleGameServerContextAsyncAction
    {
        public UpLoadBarrierRecordAsyncAction(GameServerDBarrierRecord data, GameServerPlayerContext gameServerContext, bool needResult = false, bool needSeq = false) : base(gameServerContext, null, needResult, needSeq)
        {
            m_data = data;
        }


        private GameServerDBarrierRecord m_data;

        public override async Task ExecuteAsync()
        {
            //获取要执行操作的数据库collection
            var dataBase = MongoDBHelper.GetDataBaseEntity(SampleGameServerDBItemDefine.DATABASE);

            var collection = dataBase.GetCollection<GameServerDBarrierRecord>(SampleGameServerDBItemDefine.COLLECTION_BARRIERRECORD);//获取Players集合

            await collection.InsertOneAsync(m_data);



            Log.Info("插入战绩成功");
            return;
        }

        public override void OnResult()
        {


            base.OnResult();
        }
    }

    //获取所有战斗数据
    public class GetAllBarrierRecordAsyncAction : SampleGameServerContextAsyncAction
    {
        public GetAllBarrierRecordAsyncAction(GameServerPlayerContext context, string targetQueueUserId, bool needResult = false, bool needSeq = false) : base(context, targetQueueUserId, needResult, needSeq)
        {

        }

        public override Task ExecuteAsync()
        {
            var dataBase = MongoDBHelper.GetDataBaseEntity(SampleGameServerDBItemDefine.DATABASE);

            var collection = dataBase.GetCollection<GameServerDBarrierRecord>(SampleGameServerDBItemDefine.COLLECTION_BARRIERRECORD);//获取Players集合

            var datas = collection.Find(new BsonDocument()).ToList();

            foreach (var item in datas)
            {
                Log.Info(item.Tojson());

            }

            return base.ExecuteAsync();
        }

        public override void OnResult()
        {
            base.OnResult();
        }
    }
    

}
