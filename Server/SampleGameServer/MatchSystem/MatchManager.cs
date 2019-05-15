using Crazy.NetSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            //获取游戏匹配的配置文件,获取

            
            
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
        public void AllocPlayerContext(IClientEventHandler playercontext)
        {
            var playerId = playercontext.GetInstanceId();





        }

        public void ReleasePlayerContext(ulong playerId)
        {
            IClientEventHandler playerContext;

            

            if (m_playerDic.TryRemove(playerId,out playerContext))
            {
                playerContext.OnMessage(null);//向玩家现场发送从队列字典中清除的消息
            }

            
        }


        private ConcurrentQueue<IClientEventHandler> m_playerQue;//玩家匹配队列
        private ConcurrentDictionary<ulong, IClientEventHandler> m_playerDic;//玩家字典

    }



}
