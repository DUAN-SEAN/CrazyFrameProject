using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    public interface IInvariantAttributeBase
    {

    }

    public interface IInvariantAttributeinternalBase : IInvariantAttributeBase
    {
        /// <summary>
        /// 获得最大速度
        /// </summary>
        int GetMaxVel();

        /// <summary>
        /// 获得当前转向速度
        /// </summary>
        int GetCurrentRotaVel();


    }
}