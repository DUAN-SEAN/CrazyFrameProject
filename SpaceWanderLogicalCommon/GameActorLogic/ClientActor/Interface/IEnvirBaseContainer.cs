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
    public interface IEnvirBaseContainer : IBaseContainer
    {


    }


    public interface IEnvirBaseComponentContainer: IEnvirBaseContainer , IBaseComponentContainer
    {

    }
}
