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

        /// <summary>
        /// 设置武器弹药数量
        /// </summary>
        void SetBulletNum(int num);

        /// <summary>
        /// 获取武器类型
        /// </summary>
        /// <returns></returns>
        int GetWeaponType();

        /// <summary>
        /// 获取武器CD
        /// </summary>
        int GetWeaponCd();

        /// <summary>
        /// 设置武器CD
        /// </summary>
        void SetWeaponCd(int cd);

        /// <summary>
        /// 获取武器伤害
        /// </summary>
        int GetWeaponDamage();

        /// <summary>
        /// 设置武器伤害
        /// </summary>
        void SetWeaponDamage(int damage);

        ulong GetOwnerID();
        
        void SetOwnerID(ulong id);

    }

    /// <summary>
    /// 武器属性对内接口
    /// </summary>
    public interface IWeaponAttributeInternalBase : 
        IWeaponAttributeBase
    {
        /// <summary>
        /// 添加或减少弹药
        /// </summary>
        void AddBulletNum(int add);
        void ReduceBulletNum(int rdu);

        /// <summary>
        /// 武器在过了存活时间后自动销毁
        /// -1则武器无限存活
        /// </summary>
        long GetMaxLifeTime();

      


    }
}
