using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWanderLogicalCommon
{
    /// <summary>
    /// 对外接口 显示组件
    /// </summary>
    public interface IViewComponent
    {
        /// <summary>
        /// 驱动更新view层
        /// </summary>
        void Update();
    }

    /// <summary>
    /// 对内接口 显示组件
    /// 绑定GameObject，绑定各种状态的事件event
    /// </summary>
    public interface IInternalViewComponent:IViewComponent
    {

    }
}
