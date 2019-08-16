using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace GameServer
{
    public partial class GameServerPlayerContext
    {
        /// <summary>
        /// 从数据库获取当前飞船配置
        /// </summary>
        /// <returns></returns>
        public async Task<GameServerDBPlayerShip> GetCurrentShipInfoFormDB()
        {
            var ship =  await GetPlayerShipFromDBAsync(m_gameUserId);
            return ship;
        }
        /// <summary>
        /// 服务器现场内存保存一份
        /// 上传飞船配置到数据库
        /// </summary>
        /// <param name="shipInfo"></param>
        /// <returns></returns>
        public async Task<bool> UpLoadShipInfoToDB(GameServerDBPlayerShip shipInfo)
        {
            m_gameServerDBPlayer.playerShip = shipInfo;

            return await UpdatePlayerShipInfo(shipInfo);
        }

        #region DB

#pragma warning disable CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        private async Task<bool> UpdatePlayerShipInfo(GameServerDBPlayerShip shipInfo)
#pragma warning restore CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        {
            
            //获取要执行操作的数据库collection
            var dataBase = MongoDBHelper.GetDataBaseEntity(SampleGameServerDBItemDefine.DATABASE);

            var collection = dataBase.GetCollection<GameServerDBPlayer>(SampleGameServerDBItemDefine.COLLECTION_PLAYERS);//获取Players集合

            var filterBuilder = Builders<GameServerDBPlayer>.Filter;
            
            var filter = filterBuilder.Eq(SampleGameServerDBItemDefine.PLAYER_USERNAME, m_gameUserId);

            shipInfo.modifyTime = DateTime.Now;//获取当前时间

            var update = Builders<GameServerDBPlayer>.Update.Set(SampleGameServerDBItemDefine.PLAYER_SHIPINFO, shipInfo);

            var result = collection.UpdateOne(filter, update);
            if (result.ModifiedCount > 0)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// 根据昵称获取数据库player的实体
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private async Task<GameServerDBPlayerShip> GetPlayerShipFromDBAsync(string account)
        {
            //获取要执行操作的数据库collection
            var dataBase = MongoDBHelper.GetDataBaseEntity(SampleGameServerDBItemDefine.DATABASE);

            var collection = dataBase.GetCollection<GameServerDBPlayer>(SampleGameServerDBItemDefine.COLLECTION_PLAYERS);//获取Players集合

            var filterBuilder = Builders<GameServerDBPlayer>.Filter;

            var filter = filterBuilder.Eq(SampleGameServerDBItemDefine.PLAYER_USERNAME, account);

            var result = (await (collection.FindAsync(filter))).ToList();



            if (result != null && result.Count > 0)
            {
                return result[0].playerShip;
            }

            return null;
        }


        #endregion


    }
}
