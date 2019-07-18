using SpaceShip.Base;
using System.Collections.Generic;

namespace SpaceShip.System
{
   

    /// <summary>
    /// 驱动所有的武器逻辑
    /// </summary>
    public class WeaponGameLogic : ITickable
    {

        #region SystemMethod
        
        public WeaponGameLogic()
        {
            moveWeaponsList = new List<ITickable>();
        }
        #endregion
        public void Tick()
        {
            for (int i = 0; i < moveWeaponsList.Count; i++)
            {
                moveWeaponsList[i].Tick();
            }

        }

        public void Dispose()
        {
            moveWeaponsList.Clear();
            moveWeaponsList = null;
        }
        public List<ITickable> moveWeaponsList;

       

    }
}