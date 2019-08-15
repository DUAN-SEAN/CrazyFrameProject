using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Configure;

namespace GameActorLogic
{
    /// <summary>
    /// 配置文件组件
    /// </summary>
    public interface IConfigComponentBase
    {
        void InitializeConfig(GameShipConfig[] ships, GameSkillConfig skill,GameBarrierConfig[] barrier);
    }


    public interface IConfigComponentInternalBase : IConfigComponentBase
    {
        /// <summary>
        /// 尝试获取一个配置表中的Actor
        /// </summary>
        bool TryGetActor(Int32 key, out ActorBase actor);

        /// <summary>
        /// 
        /// </summary>
        bool GetActorClone(Int32 key, out ActorBase actor);
    }
}
