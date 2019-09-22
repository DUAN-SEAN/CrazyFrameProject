using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// AI飞机 对外接口
    /// </summary>
    public interface IAIShipBaseContainer : IShipBaseContainer,
        IAIBase
    {
    }


    /// <summary>
    /// AI飞机 对内接口/
    /// </summary>
    public interface IAIShipComponentBaseContainer : 
        IAIShipBaseContainer
        ,IShipComponentBaseContainer
    {

        



    }
}
