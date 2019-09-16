using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 环境用精简Actor
    /// </summary>
    public interface IEnvirBaseContainer : IBaseContainer,
        IAIBase
    {


    }


    public interface IEnvirBaseComponentContainer: IEnvirBaseContainer , IBaseComponentContainer
    {
        /// <summary>
        /// 获得ai组件对内接口
        /// </summary>
        IAIInternalBase GetAIinternalBase();
    }
}
