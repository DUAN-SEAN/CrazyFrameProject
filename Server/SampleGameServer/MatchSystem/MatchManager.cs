using System;
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
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            

        }

        

        private MatchManager m_instance;
        public MatchManager Instance { get => m_instance; set => m_instance = value; }
    }
}
