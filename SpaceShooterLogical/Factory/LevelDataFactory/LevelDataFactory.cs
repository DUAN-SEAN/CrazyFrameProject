

using System;


/// <summary>
/// 将关卡中需要用到的对象通过其它工厂生成
/// 这里做指挥关卡生成用
/// </summary>
public class LevelDataFactory
{
    public static LevelDataFactory Instance
    {
        get
        {
            if (m_leveldatafactory == null) m_leveldatafactory = new LevelDataFactory();
            return m_leveldatafactory;
        }
    }

    private LevelDataFactory()
    {

    }

    public void LoadingResources()
    {
        var enviroemt = EnvironmentFactory.Instance;
        var ship = ShipFactory.Instance;
        var weapon = WeaponFactory.Instance;
    }


    /// <summary>
    /// 将资源加载到World世界中
    /// AI会由AIManager管理
    /// </summary>
    /// <param name="id"></param>
    public void LoadingLeveldataByID(int id)
    {
        LoadingResources();

         LevelData levelData = LevelDataSystem.Instance.GetLevelDataByID(id);
        foreach(var body in levelData.things)
        {
            LogUI.Log("leveldata count"+levelData.things.Count);
            if(body is AICarrierShipInBody)
            {
                LogUI.Log("Boss done");
                AICarrierShipInBody aIShipBase = body as AICarrierShipInBody;
                GamePlayerLogic.Instance.boss_ship = ShipFactory.Instance.LoadShipFromAssetBundle<AICarrierShipInBody>(ShipName.CarrierShip, aIShipBase.Label, aIShipBase.Min_posi, aIShipBase.Max_posi, aIShipBase.Forward) as ShipBase;

                continue;
            }
            if (body is AISmallShipInBody)
            {
                LogUI.Log("AI done");
                AISmallShipInBody aIShipBase = body as AISmallShipInBody;
                ShipFactory.Instance.LoadShipFromAssetBundle<AISmallShipInBody>(ShipName.SmallShip1, aIShipBase.Label, aIShipBase.Min_posi, aIShipBase.Max_posi, aIShipBase.Forward);
                continue;
            }
            if (body is PlayerInBody)
            {
                PlayerInBody body1 = body as PlayerInBody;
                ShipFactory.Instance.LoadShipFromAssetBundle<PlayerInBody>(ShipName.MainShip, body1.Label, body1.Min_posi, body1.Max_posi, body1.Forward);
                continue;
            }

            if(body is EnviromentInBody)
            {
                EnviromentInBody meteorite = body as EnviromentInBody;
                EnvironmentFactory.Instance.LoadEnvironmentFromAssetBundle<EnviromentInBody>(EnvironmentName.Meteorite, meteorite.Position, meteorite.radius,meteorite.Forward);
            }

        }

    }

    public void UnLoadLeveldataByID(int id)
    {
        EnvironmentFactory.Instance.Dispose();
        ShipFactory.Instance.Dispose();
        WeaponFactory.Instance.Dispose();
    }


    private static LevelDataFactory m_leveldatafactory;
}
