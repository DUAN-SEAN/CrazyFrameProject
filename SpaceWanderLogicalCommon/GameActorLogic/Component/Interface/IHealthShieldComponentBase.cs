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
        int GetShieldNum();

        /// <summary>
        /// 减少护盾恢复间隔
        /// </summary>
        void ReduceRecoveryInterval(int shieldrecovery);

        /// <summary>
        /// 增加护盾恢复值
        /// </summary>
        /// <param name="recoveryval"></param>
        void AddRecoveryVal(int recoveryval);

        /// <summary>
        /// 增加护盾上限
        /// </summary>
        void AddShieldMax(int shieldrecovery);


    }


    public interface IHealthShieldinternalBase : IHealthShieldBase
    {
        /// <summary>
        /// 失去血量
        /// 会先失去护盾
        /// </summary>
        void LossBlood(int loss);

        /// <summary>
        /// 加血 只加到血量
        /// </summary>
        void AddBlood(int get);
    }
}
