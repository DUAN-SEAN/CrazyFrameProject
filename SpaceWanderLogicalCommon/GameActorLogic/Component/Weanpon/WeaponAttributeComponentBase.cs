using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameActorLogic;

namespace GameActorLogic
{
    /// <summary>
    /// 基础武器属性组件
    /// </summary>
    public class WeaponAttributeComponentBase:
        IWeaponAttributeBase,
        IWeaponAttributeInternalBase
    {
        protected int bulletnum;

        protected int weanpontype;

        protected int maxbulletnum;

        protected long lifetime;

        public WeaponAttributeComponentBase()
        {
            bulletnum = 0;
            weanpontype = -1;
            maxbulletnum = 1;
            // 3s
            lifetime = 3000000;
        }

        public WeaponAttributeComponentBase(int bulletnum,int weanpontype,int maxbulletnum,long lifetime)
        {
            this.bulletnum = bulletnum;
            this.weanpontype = weanpontype;
            this.maxbulletnum = maxbulletnum;
            this.lifetime = lifetime;
        }
        #region IWeaponAttributeBase
        public int GetBulletNum()
        {
            return bulletnum;
        }

        public int GetWeaponType()
        {
            return weanpontype;
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



    }
}
