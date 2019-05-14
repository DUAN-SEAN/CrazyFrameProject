using Crazy.Common;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGameServer
{
    /// <summary>
    /// 注册验证
    /// </summary>
    public class RegsiterVerifyContextAsyncAction : SampleGameServerContextAsyncAction
    {
        public RegsiterVerifyContextAsyncAction(GameServerContext context, string targetQueueUserId, string username, string password, bool needResult = false, bool needSeq = false) : base(context, targetQueueUserId, needResult, needSeq)
        {
            m_username = username;
            m_password = password;
            m_state = false;
            m_errorInfo = "";

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
                    return;

                }
                var player = new GameServerDBPlayer();

                player.userName = m_username;

                player.passWord = m_password;


                player.createTime = DateTime.UtcNow;

                await collection.InsertOneAsync(player);//插入
                m_errorInfo += "插入成功\n";
                m_state = true;
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
            if (m_state)
                Log.Info("插入数据成功");
            base.OnResult();
        }
        private bool m_state;
        private string m_errorInfo;
        private string m_username;
        private string m_password;
    }


    /// <summary>
    /// 登陆验证
    /// </summary>
    public class LoginVerifyContextAsyncAction : SampleGameServerContextAsyncAction
    {
        public LoginVerifyContextAsyncAction(GameServerContext gameServerContext, string account, string password, bool needResult = false, bool needSeq = false) : base(gameServerContext, gameServerContext.ContextId.ToString(), false, false)
        {
            this.m_username = account;
            this.m_password = password;
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

                    Log.Info($"{m_username}:{m_password} 登陆成功！");
                    return;

                }


            }
            catch (Exception e)
            {

            }


            return;
        }
        public override void OnResult()
        {
            completionSource.SetResult(true);
            //gameServerContext.Send();
        }
        // RPC消息的任务完成句柄
        private TaskCompletionSource<bool> completionSource;

        private GameServerContext gameServerContext;

        private string m_username;

        private string m_password;
    }
    /// <summary>
    /// 上传一场战斗数据
    /// </summary>
    public class UpLoadBarrierRecordAsyncAction : SampleGameServerContextAsyncAction
    {
        public UpLoadBarrierRecordAsyncAction(GameServerDBarrierRecord data, GameServerContext gameServerContext, bool needResult = false, bool needSeq = false) : base(gameServerContext, null, needResult, needSeq)
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


    public class GetAllBarrierRecordAsyncAction : SampleGameServerContextAsyncAction
    {
        public GetAllBarrierRecordAsyncAction(GameServerContext context, string targetQueueUserId, bool needResult = false, bool needSeq = false) : base(context, targetQueueUserId, needResult, needSeq)
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
