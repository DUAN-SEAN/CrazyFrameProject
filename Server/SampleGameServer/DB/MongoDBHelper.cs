using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Crazy.Common;
using MongoDB.Bson;
using System.Collections;

namespace SampleGameServer
{
    public sealed class MongoDBHelper
    {
        

        public static void CreateDBClient()
        {
            try
            {
                var dbConfig = GameServer.Instance.m_gameServerGlobalConfig.DBConfigInfo[0];

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
            BsonElement bsonElement = new BsonElement();
            
                
        }

    }
}
