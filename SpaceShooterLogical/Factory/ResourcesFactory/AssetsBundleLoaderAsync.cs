
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;



    public class AssetsBundleLoaderAsync : IDisposable
    {
        private AssetBundleCreateRequest request;

        private TaskCompletionSource<AssetBundle> tcs;

        public void Update()
        {
            if (!this.request.isDone)
            {
                return;
            }

            TaskCompletionSource<AssetBundle> t = tcs;
            t.SetResult(this.request.assetBundle);
        }

        

        public Task<AssetBundle> LoadAsync(string path)
        {
            this.tcs = new TaskCompletionSource<AssetBundle>();
            this.request = AssetBundle.LoadFromFileAsync(path);
            return this.tcs.Task;
        }

    public void Dispose()
    {

    }
}

