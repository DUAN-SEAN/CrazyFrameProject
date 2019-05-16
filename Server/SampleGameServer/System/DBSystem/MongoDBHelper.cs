using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Crazy.Common;
using MongoDB.Bson;
using System.Collections;
using GameServer.Configure;

namespace GameServer
{
    public sealed class MongoDBHelper
    {
        static MongoDBHelper()
        {
            _dbConfigDic = new Dictionary<string, DBConfigInfo>();
            foreach (var elem in GameServer.Instance.m_gameServerGlobalConfig.DBConfigInfos)
            {
                _dbConfigDic[elem.DataBase] = elem;
            }
            _dbEntityDic = new Dictionary<string, IMongoDatabase>();
        }
        /// <summary>
        /// 根据数据库名获得数据库实体
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static IMongoDatabase GetDataBaseEntity(string dbName)
        {
            IMongoDatabase mongoDatabase = null;
            if(_dbEntityDic.TryGetValue(dbName,out mongoDatabase))
            {
                return mongoDatabase;
            }
            DBConfigInfo dBConfigInfo = null;
            // 之前没有记录，说明是第一次构造这个MongoDatabase
            if (!_dbConfigDic.TryGetValue(dbName, out dBConfigInfo))
            {
                Log.Fatal("DBConfig is Empty");
                return null;
            }
            string connection = $"mongodb://{dBConfigInfo.ConnectHost}:{dBConfigInfo.Port}/{dBConfigInfo.DataBase}";//暂时不需要验证
            var client = new MongoClient(connection);
            mongoDatabase = client.GetDatabase(dbName);
            _dbEntityDic[dbName] = mongoDatabase;

            return mongoDatabase;


        }
        public static void CreateDBClient()
        {
            try
            {
                var dbConfig = GameServer.Instance.m_gameServerGlobalConfig.DBConfigInfos[0];

                // 连接到单实例MongoDB服务
                // 这种连接方式不会自动检测连接的节点是主节点还是复制集的成员节点
                //var client = new MongoClient();

                // 或者使用string连接本地服务器,localhost=127.0.0.1,连接到单实例
                string connection = $"mongodb://{dbConfig.ConnectHost}:{dbConfig.Port}/{dbConfig.DataBase}";
                string connection1 = $"mongodb://root:123456@{dbConfig.ConnectHost}:{dbConfig.Port}/admin";
                Log.Info(connection);
                var client = new MongoClient(connection);



                var dataBase = client.GetDatabase(dbConfig.DataBase);

                var collection = dataBase.GetCollection<BsonDocument>("Players");

                {
                    //插入
                    var document = new BsonDocument
                {
                    { "username", "MongoDB3" },
                    { "password", "Database" },
                    { "level", 1 },

                };
                    collection.InsertOne(document);
                    Log.Info("插入成功：" + document.ToJson());

                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            
        }

        public static void Test()
        {
            var gameContext = GameServer.Instance.PlayerCtxManager.AllocPlayerContext() as GameServerContext;

            //1
            //new RegsiterVerifyContextAsyncAction(gameContext, gameContext.ContextId.ToString(), "duanrui", "123456").Start();
            //2
            //new LoginVerifyContextAsyncAction(gameContext, "duanrui", "123456").Start();
            //3
            //var data = new GameServerDBarrierRecord { battleId = 1, number = 1, grade = 100, costTime = 600000,endTime = DateTime.Now, startTime = DateTime.Now,
            //    players = new List<GameServerDBarrierRecordPlayer>() };
            //data.players.Add(  new GameServerDBarrierRecordPlayer {userName = "zhuying" });
            //new UpLoadBarrierRecordAsyncAction(data,gameContext).Start();
            //4
            new GetAllBarrierRecordAsyncAction(gameContext, null).Start();


        }

        /// <summary>
        /// 配置文件读到的数据库配置
        /// </summary>
        private static Dictionary<String, DBConfigInfo> _dbConfigDic ;

        /// <summary>
        /// 由配置文件配置数据生成的数据库entry，保存在这里，后面再使用的时候直接从这里取，提升效率
        /// </summary>
        private static Dictionary<String, IMongoDatabase> _dbEntityDic;

    }
}
