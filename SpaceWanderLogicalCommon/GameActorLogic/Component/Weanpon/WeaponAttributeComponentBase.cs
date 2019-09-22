using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crazy.Common;
using GameActorLogic;

namespace GameActorLogic
{
    /// <summary>
    /// 基础武器属性组件
    /// </summary>
    public class WeaponAttributeComponentBase:
        IWeaponAttributeBase,
        IWeaponAttributeInternalBase,
        ISkillAttributeComponent,
        ISkillAttributeInternalComponent
    {
        //子弹容量
        protected int bulletnum;

        protected int weanpontype;

        protected int maxbulletnum;

        protected long lifetime;

        protected int weaponcd;

        protected int weaponDamage;

        protected ulong OwnerActorId;
        public WeaponAttributeComponentBase()
        {
            bulletnum = 0;
            weanpontype = -1;
            maxbulletnum = 1;
            weaponDamage = 20;
            // 3s
            lifetime = 3000000;

            OwnerActorId = ulong.MaxValue;
        }

        public WeaponAttributeComponentBase(int bulletnum,int weanpontype,int maxbulletnum,long lifetime,int Damage,ulong OwnerActorId)
        {
            this.bulletnum = bulletnum;
            this.weanpontype = weanpontype;
            this.maxbulletnum = maxbulletnum;
            this.lifetime = lifetime;
            this.weaponDamage = Damage;
            this.OwnerActorId = OwnerActorId;
        }

        public WeaponAttributeComponentBase(WeaponAttributeComponentBase clone)
        {
            this.bulletnum = clone.bulletnum;
            this.weanpontype = clone.weanpontype;
            this.maxbulletnum = clone.maxbulletnum;
            this.lifetime = clone.lifetime;
            this.weaponcd = clone.weaponcd;
            this.weaponDamage = clone.weaponDamage;
            this.OwnerActorId = clone.OwnerActorId;
            //Log.Trace("WeaponAttributeComponent:weaponcd" + weaponcd);
        }

        #region IWeaponAttributeBase
        public int GetBulletNum()
        {
            return bulletnum;
        }

        public void SetBulletNum(int num)
        {
            bulletnum = num;
        }

        public int GetWeaponType()
        {
            return weanpontype;
        }

        public int GetWeaponCd()
        {
            return weaponcd;
        }

        public void SetWeaponCd(int cd)
        {
            weaponcd = cd;
        }

        public void SetWeaponDamage(int damage)
        {
            weaponDamage = damage;
        }

        public ulong GetOwnerID()
        {
            return OwnerActorId;
        }

        public void SetOwnerID(ulong id)
        {
            OwnerActorId = id;
        }

        public int GetWeaponDamage()
        {
            return weaponDamage;
        }

        #endregion

        #region IWeaponAttributeInternalBase

        public void AddBulletNum(int add)
        {

            bulletnum += add;
            if (bulletnum > maxbulletnum) bulletnum = maxbulletnum;
        }

        public void ReduceBulletNum(int rdu)
        {
            bulletnum -= rdu;
            if (bulletnum < 0) bulletnum = 0;
        }

        public long GetMaxLifeTime()
        {
            return lifetime;
        }

        #endregion


        public int GetSkillCapacity()
        {
            return GetBulletNum();
        }

        public void SetSkillCapacity(int cap)
        {
           SetBulletNum(cap);
        }

        public int GetSkillCd()
        {
            return GetWeaponCd();
        }

        public void SetSkillCd(int cd)
        {
            SetWeaponCd(cd);
        }

        public int GetMaxSkillCd()
        {
            return maxbulletnum;
        }

        public void SetMaxSkillCd(int cd)
        {
            maxbulletnum = cd;
        }
    }
}
