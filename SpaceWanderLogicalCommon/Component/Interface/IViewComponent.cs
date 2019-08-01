using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWanderLogicalCommon
{
    public interface IViewComponent
    {
        /// <summary>
        /// 驱动更新view层
        /// </summary>
        void Update();
    }

    public interface IInternalViewComponent:IViewComponent
    {

    }
}
