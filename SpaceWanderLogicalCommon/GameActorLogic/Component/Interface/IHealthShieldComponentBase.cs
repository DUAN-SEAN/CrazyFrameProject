using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 血量及护盾接口
    /// </summary>
    public interface IHealthShieldBase
    {
        /// <summary>
        /// 获得当前血量
        /// </summary>
        int GetHP();

        /// <summary>
        /// 获得当前护盾值
        /// </summary>
        int GetShieldBase();

        /// <summary>
        /// 失去血量
        /// </summary>
        void LossBlood(int loss);

        /// <summary>
        /// 得到血量
        /// </summary>
        void GetBlood(int get);
    }


    public interface IHealthShieldinternalBase : IHealthShieldBase
    {

    }
}
