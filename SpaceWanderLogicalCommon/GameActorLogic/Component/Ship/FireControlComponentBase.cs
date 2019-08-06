using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameActorLogic
{
    /// <summary>
    /// 基础火控组件
    /// 每个飞船的火控组件中的武器应该不同
    /// </summary>
    public abstract class FireControlComponentBase:
        IFireControlBase,
        IFireControlInternalBase
    {
        protected List<IWeaponBaseContainer> weapons;

        protected FireControlComponentBase()
        {
            weapons = new List<IWeaponBaseContainer>();
        }

        protected FireControlComponentBase(List<IWeaponBaseContainer> weapons)
        {
            this.weapons = weapons;
        }


        #region IFireControlBase
        public void Fire(int i)
        {
            for (var j = 0; j < weapons.Count; j++)
            {
                if (weapons[j].GetWeanponType() == i)
                    OnFire?.Invoke(weapons[j]);
            }

            
        }

        public void End(int i)
        {

            for (var j = 0; j < weapons.Count; j++)
            {
                if (weapons[j].GetWeanponType() == i)
                    OnEnd?.Invoke(weapons[j]);
            }
        }

        public void Destroy(int i)
        {

            for (var j = 0; j < weapons.Count; j++)
            {
                if (weapons[j].GetWeanponType() == i)
                    OnDestroy?.Invoke(weapons[j]);
            }
        }
        #endregion

        #region IFireControlinternalBase

        public event Action<IWeaponBaseContainer> OnFire;
        public event Action<IWeaponBaseContainer> OnEnd;
        public event Action<IWeaponBaseContainer> OnDestroy;


        #endregion





    }
}
