using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 武器属性 对外接口
    /// </summary>
    public interface IWeaponAttributeBase
    {
        /// <summary>
        /// 获取武器弹药数量
        /// </summary>
        int GetBulletNum();
    }

    /// <summary>
    /// 武器属性对内接口
    /// </summary>
    public interface IWeanponAttributeinternalBase : 
        IWeaponAttributeBase
    {
        /// <summary>
        /// 添加或减少弹药
        /// </summary>
        int BulletNum { set; get; }

        /// <summary>
        /// 武器在过了存活时间后自动销毁
        /// -1则武器无限存活
        /// </summary>
        long GetMaxLifeTime();


    }
}
