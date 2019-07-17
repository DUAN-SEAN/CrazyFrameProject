using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///生成环境GameObjedt
/// 将Body与GameObject绑定
/// Body由Body工厂生成
/// </summary>
public class EnvironmentFactory
{
    private EnvironmentFactory()
    {
        LoadedEnvironmentDict = new Dictionary<string, GameObject>();

        // 从文件中加载指定的AssetBudle包
        ResourcesComponent.Instance.LoadBundle(AssetBundleName.EnvironmentAssetBundle);
    }

    public void Dispose()
    {
        // 从缓存中卸载指定的AssetBundle包
        ResourcesComponent.Instance.UnloadBundle(AssetBundleName.EnvironmentAssetBundle);
    }


    public static EnvironmentFactory Instance
    {
        get
        {
            if (m_environmentfactory == null) m_environmentfactory = new EnvironmentFactory();
            return m_environmentfactory;
        }
    }


    public T LoadEnvironmentFromAssetBundle<T>(string prefabname, Vector2 position,float radius,Vector2 forward)where T : EnviromentInBody,new()
    {
        if (!LoadedEnvironmentDict.TryGetValue(prefabname, out GameObject gameObject))
        {
            gameObject = ResourcesComponent.Instance.GetAsset(AssetBundleName.EnvironmentAssetBundle, prefabname) as GameObject;
            gameObject.SetActive(false);
            LoadedEnvironmentDict[prefabname] = gameObject;
        }

        T body_environ = BodyFactory.Instance.LoadEnvironmentBodyByType<T>(position, radius, forward);

        GameObject game = UnityEngine.Object.Instantiate(gameObject);
        game.SetActive(true);
        LogUI.Log("enviroment init");
        body_environ.enviromentinworld = game.GetComponent<EnviromentInWorld>();
        LogUI.Log(body_environ.enviromentinworld);
        body_environ.enviromentinworld.m_body = body_environ;
        //body_environ.enviromentinworld.Destroy();

        return body_environ;
    }

    private Dictionary<string, GameObject> LoadedEnvironmentDict;

    private static EnvironmentFactory m_environmentfactory;
}

public static class EnvironmentName
{

    public const string Meteorite = "Asteroid";
}