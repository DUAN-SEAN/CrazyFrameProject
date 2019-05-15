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
    public class MatchManager
    {
        /// <summary>
        /// 根据配置文件初始化匹配管理器
        /// </summary>
        public void Initialize()
        {
            

        }


       


        private MatchManager m_instance;
        public MatchManager Instance { get => m_instance; set => m_instance = value; }
    }


    public class MatchPlayerContextQueue
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
