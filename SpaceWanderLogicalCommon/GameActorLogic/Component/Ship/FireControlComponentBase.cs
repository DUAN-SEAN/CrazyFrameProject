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
    public class FireControlComponentBase:
        IFireControlBase,
        IFireControlInternalBase
    {
        /// <summary>
        /// 准备好生成的武器
        /// </summary>
        protected List<IWeaponBaseContainer> weapons;

        protected List<IWeaponBaseContainer> weaponInitList;

        protected ILevelActorComponentBaseContainer level;

        protected IShipComponentBaseContainer container;



        public FireControlComponentBase(IShipComponentBaseContainer container, ILevelActorComponentBaseContainer create)
        {
            weapons = new List<IWeaponBaseContainer>();
            this.container = container;
            this.level = create;
        }

        public FireControlComponentBase(IShipComponentBaseContainer container, ILevelActorComponentBaseContainer create,List<Int32> weapons)
        {
            this.container = container;
            this.weapons = new List<IWeaponBaseContainer>();
            foreach (var weapon in weapons)
            {
               level.GetConfigComponentInternalBase().GetActorClone(weapon, out var actor);
               if (actor is IWeaponBaseContainer weaponBase)
                   this.weapons.Add(weaponBase);
            }
            
        }

        protected void WaitWeaponDestroy(IWeaponBaseContainer weapon)
        {
            weaponInitList.Remove(weapon);
        }

        #region IFireControlBase

        public void InitializeFireControl(List<Int32> containers)
        {
            weapons = new List<IWeaponBaseContainer>();
            foreach (var weapon in containers)
            {
                level.GetConfigComponentInternalBase().GetActorClone(weapon, out var actor);
                if (actor is IWeaponBaseContainer weaponBase)
                    this.weapons.Add(weaponBase);
            }
        }

        public void SendButtonState(ulong actorid, int skilltype, int skillcontrol)
        {
            
        }

        
        

        public void Fire(int i)
        {
            for (var j = 0; j < weapons.Count; j++)
            {
                if (weapons[j].GetWeaponType() == i && weapons[j] is IWeaponBaseContainer actor)
                {
                    var weapon = actor.Clone() as IWeaponBaseContainer;
                    weaponInitList.Add(weapon);
                    weapon.OnDestroyWeapon += WaitWeaponDestroy;
                    level.GetEnvirinfointernalBase().AddActor(weapon as ActorBase);
                    weapon.Start();
                    OnFire?.Invoke(weapon);
                }
            }
        }

        public void End(int i)
        {

            for (var j = 0; j < weaponInitList.Count; j++)
            {
                if (weaponInitList[j].GetWeaponType() == i)
                {
                    weaponInitList[j].End();
                    OnEnd?.Invoke(weaponInitList[j]);
                }
            }
        }

        public void Destroy(int i)
        {
            List<IWeaponBaseContainer> weaponList = new List<IWeaponBaseContainer>();
            for (var j = 0; j < weaponInitList.Count; j++)
            {
                if (weaponInitList[j].GetWeaponType() == i)
                {
                    weaponList.Add(weaponInitList[j]);
                }
            }
            //从集合中删除
            foreach (var weaponBaseContainer in weaponInitList)
            {
                weaponList.Remove(weaponBaseContainer);
                OnDestroy?.Invoke(weaponBaseContainer);
                weaponBaseContainer.Destroy();
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
