using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWanderLogicalCommon
{
    public  interface IMoveComponent
    {
        /// <summary>
        /// 移动
        /// </summary>
        void Move();

        void Reset();
    }
    public interface IInternalMoveComponent:IMoveComponent
    {
       
    }
}
