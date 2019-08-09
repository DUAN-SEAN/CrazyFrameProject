using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyEngine.Base;
using CrazyEngine.Core;

namespace GameActorLogic
{
    /// <summary>
    /// actor环境组件
    /// 包括物理引擎相关操作
    /// 对外接口
    /// </summary>
    public interface IEnvirinfoBase
    {
        void AddToEngine(ObjBase obj);

        void RemoveFromEngine(ObjBase obj);


    }

    /// <summary>
    /// 对内接口
    /// </summary>
    public interface IEnvirinfointernalBase : IEnvirinfoBase
    {
        /// <summary>
        /// 获取物理引擎
        /// </summary>
        /// <returns></returns>
        Engine GetEngine();


    }


}
