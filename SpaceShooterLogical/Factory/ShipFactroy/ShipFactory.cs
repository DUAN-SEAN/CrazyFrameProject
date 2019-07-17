using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///生成船GameObjedt
/// 将Body与GameObject绑定
/// Body由Body工厂生成
/// </summary>
public class ShipFactory
{

    private ShipFactory()
    {
        LoadedShipDict = new Dictionary<string, GameObject>();

        //从资源中加载船的AB包
        ResourcesComponent.Instance.LoadBundle(AssetBundleName.ShipAssetBundle);
    }

   



    public void Dispose()
    {
        //从缓存中卸载指定的AB包
        ResourcesComponent.Instance.UnloadBundle(AssetBundleName.ShipAssetBundle);
    }


    public static ShipFactory Instance
    {
        get
        {
            if (m_shipfactory == null) m_shipfactory = new ShipFactory();
            return m_shipfactory; 
        }
    }

/// <summary>
/// 从AB包中加载GameObject资源
/// 用Body工厂生成Body
/// </summary>
    public T LoadShipFromAssetBundle<T>(string prefabname,Label label,Vector2 min ,Vector2 max,Vector2 forward)where T : ShipBase, new()
    {
        //确认工厂缓存
        if (!LoadedShipDict.TryGetValue(prefabname, out GameObject gameObject))
        {
            gameObject = ResourcesComponent.Instance.GetAsset(AssetBundleName.ShipAssetBundle, prefabname) as GameObject;
            gameObject.SetActive(false);
            LoadedShipDict[prefabname] = gameObject;
        }

    
        //使用BodyFactory生成船Body
        T body_ship = BodyFactory.Instance.LoadShipBodyByType<T>(label, min, max, forward);
        //body_ship.Enable = true;

        //生成GameObject并与Body绑定
        GameObject game = UnityEngine.Object.Instantiate(gameObject);
        game.SetActive(true);
        body_ship.shipinworld = game.GetComponent<ShipInWorld>();
        body_ship.shipinworld.m_ship = body_ship;

        return body_ship;
    }

    private Dictionary<string, GameObject> LoadedShipDict;

    private static ShipFactory m_shipfactory;
}

public static class ShipName
{

    public const string MainShip = "Players Variant";
    public const string MainShip2 = "SF_Battleship-G6_FBX";


    public const string BossBigShip1 = "Carrier";
    public const string SmallShip1 = "SF_Bomber-X4";
    public const string CarrierShip = "Carrier";
}