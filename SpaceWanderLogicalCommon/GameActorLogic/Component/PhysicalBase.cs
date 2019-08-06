using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{

    /// <summary>
    /// 物理信息组件
    /// 可能会与碰撞组件合并成一个类 然后继承两种接口
    /// </summary>
    public abstract class PhysicalBase:
        IPhysicalBase,
        IPhysicalinternalBase
    {

        #region IPhysicalBase
        public void GetPosition()
        {
        }

        public void GetForward()
        {
        }

        public double GetSpeed()
        {
            return 0;
        }


        #endregion

        #region IPhysicalinternalBase

        public void AddForce()
        {
        }

        public void AddForward()
        {
        }

        #endregion


    }
}
