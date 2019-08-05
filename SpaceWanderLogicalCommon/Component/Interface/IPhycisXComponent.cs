using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWanderLogicalCommon
{
    /// <summary>
    /// 对外接口
    /// </summary>
    public interface IPhycisXComponent
    {

    }

    public interface IInternalPhycisXComponent:IPhycisXComponent
    {

        /// <summary>
        /// 得到当前位置
        /// </summary>
        void GetCurrentPosition();

        void GetCurrentForward();
    }
}
