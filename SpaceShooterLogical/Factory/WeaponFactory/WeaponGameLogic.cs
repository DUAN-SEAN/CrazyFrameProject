using System;
using System.Collections.Generic;

/// <summary>
/// 驱动所有的武器逻辑
/// </summary>
public class WeaponGameLogic : ITickable
{

    #region SystemMethod
    public static WeaponGameLogic Instance
    {
        get
        {
            if (m_weapongameSystem == null) m_weapongameSystem = new WeaponGameLogic();
            return m_weapongameSystem;
        }

    }
    private WeaponGameLogic()
    {
        moveWeaponsList = new List<ITickable>();
    }
    #endregion
    public void Tick()
    {
        for(int i = 0; i < moveWeaponsList.Count; i++)
        {
            moveWeaponsList[i].Tick();
        }

    }


    public List<ITickable> moveWeaponsList;

    protected static WeaponGameLogic m_weapongameSystem;

   
}
