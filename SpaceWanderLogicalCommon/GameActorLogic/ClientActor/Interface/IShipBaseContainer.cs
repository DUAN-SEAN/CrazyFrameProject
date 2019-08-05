using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 船Actor对外接口
    /// </summary>
    public interface IShipBaseContainer : IBaseContainer,
        IFireControlBase,
        IHealthShieldBase
    {

    }


    /// <summary>
    /// 船对内接口
    /// </summary>
    public interface IShipComponentBaseContainer : 
        IBaseComponentContainer
        ,IShipBaseContainer
    {
        IFireControlinternalBase GetFireControlinternalBase(); 

        IHealthShieldinternalBase GetHealthShieldinternalBase();
    }
}
