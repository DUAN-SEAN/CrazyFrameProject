
using System;
using System.Threading.Tasks;
using UnityEngine;




    public class AssetsLoaderAsync : IDisposable
    {
        private AssetBundle assetBundle;

        private AssetBundleRequest request;

        private TaskCompletionSource<bool> tcs;

        public AssetsLoaderAsync(AssetBundle ab)
        {
            this.assetBundle = ab;
        }

        public void Update()
        {
            if (!this.request.isDone)
            {
                return;
            }

            TaskCompletionSource<bool> t = tcs;
            t.SetResult(true);
        }

       
        public async Task<UnityEngine.Object[]> LoadAllAssetsAsync()
        {
            await InnerLoadAllAssetsAsync();
            return this.request.allAssets;
        }

        private Task<bool> InnerLoadAllAssetsAsync()
        {
            this.tcs = new TaskCompletionSource<bool>();
            this.request = assetBundle.LoadAllAssetsAsync();
            return this.tcs.Task;
        }

    public void Dispose()
    {
        assetBundle = null;
        request = null;
    }
}

