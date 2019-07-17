using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif




public class ABInfo
{
    private int refCount;
    public string Name { get; }

    public int RefCount
    {
        get
        {
            return this.refCount;
        }
        set
        {
            //WLog.Debug($"{this.Name} refcount: {value}");
            this.refCount = value;
        }
    }

    public AssetBundle AssetBundle { get; }

    public ABInfo(string name, AssetBundle ab)
    {
        this.Name = name;
        this.AssetBundle = ab;
        this.RefCount = 1;
        LogUI.Log($"load assetbundle: {this.Name}");
    }


}

// 用于字符串转换，减少GC
public static class AssetBundleHelper
{
    // 缓存包依赖，不用每次计算
    public static Dictionary<string, string[]> DependenciesCache = new Dictionary<string, string[]>();

    public static Dictionary<string, string> BundleNameToLowerDict = new Dictionary<string, string>();

    public static string BundleNameToLower(this string value)
    {
        if (BundleNameToLowerDict.TryGetValue(value, out string result))
        {
            return result;
        }
        result = value.ToLower();
        BundleNameToLowerDict[value] = result;
        return result;
    }

    public static string[] GetDependencies(string assetBundleName)
    {
        string[] dependencies = new string[0];
        if (DependenciesCache.TryGetValue(assetBundleName, out dependencies))
        {
            return dependencies;
        }
        if (!Define.IsAsync)
        {
#if UNITY_EDITOR
            dependencies = AssetDatabase.GetAssetBundleDependencies(assetBundleName, true);
#endif
        }
        else
        {
            dependencies = ResourcesComponent.AssetBundleManifestObject.GetAllDependencies(assetBundleName);
        }
        DependenciesCache.Add(assetBundleName, dependencies);
        return dependencies;
    }

    public static string[] GetSortedDependencies(string assetBundleName)
    {
        //WLog.Info("get:" + assetBundleName);
        Dictionary<string, int> info = new Dictionary<string, int>();
        List<string> parents = new List<string>();
        CollectDependencies(parents, assetBundleName, info);
        string[] ss = info.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
        return ss;
    }

    public static void CollectDependencies(List<string> parents, string assetBundleName, Dictionary<string, int> info)
    {
        //WLog.Info("collect:" + assetBundleName);
        parents.Add(assetBundleName);
        string[] deps = GetDependencies(assetBundleName);
        foreach (string parent in parents)
        {
            if (!info.ContainsKey(parent))
            {
                info[parent] = 0;
            }
            info[parent] += deps.Length;
        }


        foreach (string dep in deps)
        {
            if (parents.Contains(dep))
            {
                throw new Exception($"包有循环依赖，请重新标记: {assetBundleName} {dep}");
            }
            CollectDependencies(parents, dep, info);
        }
        parents.RemoveAt(parents.Count - 1);
    }
}


public class ResourcesComponent : IDisposable
{
    public static AssetBundleManifest AssetBundleManifestObject { get; set; }

    private readonly Dictionary<string, Dictionary<string, UnityEngine.Object>> resourceCache = new Dictionary<string, Dictionary<string, UnityEngine.Object>>();

    private readonly Dictionary<string, ABInfo> bundles = new Dictionary<string, ABInfo>();

    private static ResourcesComponent m_resourcesComponent;

    public static ResourcesComponent Instance
    {
        get
        {
            if (m_resourcesComponent == null) m_resourcesComponent = new ResourcesComponent();
            return m_resourcesComponent;
        }
    }

    public void Dispose()
    {

        foreach (var abInfo in this.bundles)
        {
            abInfo.Value?.AssetBundle?.Unload(true);
        }

        this.bundles.Clear();
        this.resourceCache.Clear();
    }

    private ResourcesComponent()
    {
#if !UNITY_EDITOR
        LoadOneBundle("StreamingAssets");
        AssetBundleManifestObject = (AssetBundleManifest)GetAsset("StreamingAssets", "AssetBundleManifest");
#endif
        }



    public UnityEngine.Object GetAsset(string bundleName, string prefab)
    {

        if (!this.resourceCache.TryGetValue(bundleName.BundleNameToLower(), out Dictionary<string, UnityEngine.Object> dict))
        {
            throw new Exception($"not found asset: {bundleName} {prefab}");
        }

        if (!dict.TryGetValue(prefab, out UnityEngine.Object resource))
        {
            throw new Exception($"not found asset: {bundleName} {prefab}");
        }

        return resource;
    }

    public void UnloadBundle(string assetBundleName)
    {


        string[] dependencies = AssetBundleHelper.GetSortedDependencies(assetBundleName.BundleNameToLower());

        //WLog.Debug($"-----------dep unload {assetBundleName} dep: {dependencies.ToList().ListToString()}");
        foreach (string dependency in dependencies)
        {
            this.UnloadOneBundle(dependency);
        }
    }

    private void UnloadOneBundle(string assetBundleName)
    {
        assetBundleName = assetBundleName.ToLower();

        if (!this.bundles.TryGetValue(assetBundleName, out ABInfo abInfo))
        {
            throw new Exception($"not found assetBundle: {assetBundleName}");
        }

        //WLog.Debug($"---------- unload one bundle {assetBundleName} refcount: {abInfo.RefCount - 1}");

        --abInfo.RefCount;

        if (abInfo.RefCount > 0)
        {
            return;
        }


        this.bundles.Remove(assetBundleName);
        //abInfo.Dispose();
        //WLog.Debug($"cache count: {this.cacheDictionary.Count}");
    }

    /// <summary>
    /// 同步加载assetbundle
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
    public void LoadBundle(string assetBundleName)
    {
        assetBundleName = assetBundleName.BundleNameToLower();
        //WLog.Info(assetBundleName);
        string[] dependencies = AssetBundleHelper.GetSortedDependencies(assetBundleName);
        //WLog.Info("dependencies:"+dependencies.Length);
        foreach (string dependency in dependencies)
        {
            //WLog.Info(dependency);
            if (string.IsNullOrEmpty(dependency))
            {
                continue;
            }
            this.LoadOneBundle(dependency);
        }
    }

    public void AddResource(string bundleName, string assetName, UnityEngine.Object resource)
    {
        if (!this.resourceCache.TryGetValue(bundleName, out Dictionary<string, UnityEngine.Object> dict))
        {
            dict = new Dictionary<string, UnityEngine.Object>();
            this.resourceCache[bundleName] = dict;
        }

        dict[assetName] = resource;
    }

    public void LoadOneBundle(string assetBundleName)
    {
        //WLog.Debug($"---------------load one bundle {assetBundleName}");
        if (this.bundles.TryGetValue(assetBundleName.BundleNameToLower(), out ABInfo abInfo))
        {
            ++abInfo.RefCount;
            return;
        }

        if (!Define.IsAsync)
        {
            string[] realPath = null;
#if UNITY_EDITOR
            realPath = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
            //LogUI.Log(assetBundleName);
            //LogUI.Log(realPath.Count());
            foreach (string s in realPath)
            {
                //LogUI.Log(s);
                string assetName = Path.GetFileNameWithoutExtension(s);
                //LogUI.Log(assetName);
                UnityEngine.Object resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);
                AddResource(assetBundleName, assetName, resource);
            }

            this.bundles[assetBundleName] = new ABInfo(assetBundleName, null);
#endif
            return;
        }

        string p;

        p = Path.Combine(Application.streamingAssetsPath, assetBundleName);
#if UNITY_ANDROID
            p = Path.Combine(PathHelper.AppArdPath, assetBundleName);
#endif
        AssetBundle assetBundle = null;
        LogUI.Log("AB包路径" + p);
        //if (File.Exists(p))
        //{
        LogUI.Log("文件存在");
        assetBundle = AssetBundle.LoadFromFile(p);
        //}
        //else
        //{
        //	p = Path.Combine(PathHelper.AppResPath, assetBundleName);
        //	assetBundle = AssetBundle.LoadFromFile(p);
        //}

        if (assetBundle == null)
        {
            throw new Exception($"assets bundle not found: {assetBundleName}");
        }

        if (!assetBundle.isStreamedSceneAssetBundle)
        {
            // 异步load资源到内存cache住
            UnityEngine.Object[] assets = assetBundle.LoadAllAssets();
            foreach (UnityEngine.Object asset in assets)
            {
                AddResource(assetBundleName, asset.name, asset);
            }
        }

        this.bundles[assetBundleName] = new ABInfo(assetBundleName, assetBundle);
    }

    /// <summary>
    /// 异步加载assetbundle
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
    public async Task LoadBundleAsync(string assetBundleName)
    {
        assetBundleName = assetBundleName.ToLower();
        string[] dependencies = AssetBundleHelper.GetSortedDependencies(assetBundleName);
        // Log.Debug($"-----------dep load {assetBundleName} dep: {dependencies.ToList().ListToString()}");
        foreach (string dependency in dependencies)
        {
            if (string.IsNullOrEmpty(dependency))
            {
                continue;
            }
            await this.LoadOneBundleAsync(dependency);
        }
    }

    public async Task LoadOneBundleAsync(string assetBundleName)
    {
        if (this.bundles.TryGetValue(assetBundleName, out ABInfo abInfo))
        {
            ++abInfo.RefCount;
            return;
        }

        LogUI.Log($"---------------load one bundle {assetBundleName}");
        if (!Define.IsAsync)
        {
            string[] realPath = null;
#if UNITY_EDITOR
            realPath = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
            foreach (string s in realPath)
            {
                string assetName = Path.GetFileNameWithoutExtension(s);
                UnityEngine.Object resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);
                AddResource(assetBundleName, assetName, resource);
            }

            this.bundles[assetBundleName] = new ABInfo(assetBundleName, null);
#endif
            return;
        }

        string p = Path.Combine(Application.streamingAssetsPath, assetBundleName);
        AssetBundle assetBundle = null;
        //if (!File.Exists(p))
        //{
        //	p = Path.Combine(PathHelper.AppResPath, assetBundleName);
        //}

        using (AssetsBundleLoaderAsync assetsBundleLoaderAsync = new AssetsBundleLoaderAsync())
        {
            assetBundle = await assetsBundleLoaderAsync.LoadAsync(p);
        }

        if (assetBundle == null)
        {
            throw new Exception($"assets bundle not found: {assetBundleName}");
        }

        if (!assetBundle.isStreamedSceneAssetBundle)
        {
            // 异步load资源到内存cache住
            UnityEngine.Object[] assets;
            using (AssetsLoaderAsync assetsLoaderAsync = new AssetsLoaderAsync(assetBundle))
            {
                assets = await assetsLoaderAsync.LoadAllAssetsAsync();
            }
            foreach (UnityEngine.Object asset in assets)
            {
                AddResource(assetBundleName, asset.name, asset);
            }
        }

        this.bundles[assetBundleName] = new ABInfo(assetBundleName, assetBundle);
    }

    public string DebugString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (ABInfo abInfo in this.bundles.Values)
        {
            sb.Append($"{abInfo.Name}:{abInfo.RefCount}\n");
        }
        return sb.ToString();
    }
}



public static class Define
{
#if UNITY_EDITOR && !ASYNC
    public static bool IsAsync;
#else
    public static bool IsAsync = true;
#endif

#if UNITY_EDITOR
    public static bool IsEditorMode = true;
#else
    public static bool IsEditorMode = false;
#endif

#if DEVELOPMENT_BUILD
    public static bool IsDevelopmentBuild = true;
#else
    public static bool IsDevelopmentBuild;
#endif

#if ILRuntime
    public static bool IsILRuntime = true;
#else
    public static bool IsILRuntime;
#endif
}