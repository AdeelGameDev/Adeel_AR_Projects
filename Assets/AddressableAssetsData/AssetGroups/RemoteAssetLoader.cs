using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public interface IAssetLoader
{
    event Action<float> OnSingleAssetDownloading; // Pass downloaded size in MB
    event Action<float> OnSingleAssetSize; // Pass asset size in MB
    event Action OnSingleAssetDownloaded;
}

public class RemoteAssetLoader<T> : MonoBehaviour, IAssetLoader where T : UnityEngine.Object
{
    [SerializeField] private string label;
    private List<string> assetKeys = new List<string>();

    [SerializeField] private T defaultAsset;

    public static int totalAssetCount;

    public event Action<float> OnSingleAssetDownloading; // Pass downloaded size in MB
    public event Action<float> OnSingleAssetSize; // Pass asset size in MB
    public event Action OnSingleAssetDownloaded;

    protected virtual void Start()
    {
        LoadAssetKeys();
    }

    private void LoadAssetKeys()
    {
        Addressables.LoadResourceLocationsAsync(label).Completed += OnResourceLocationsLoaded;
    }

    protected virtual void OnResourceLocationsLoaded(AsyncOperationHandle<IList<UnityEngine.ResourceManagement.ResourceLocations.IResourceLocation>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var location in handle.Result)
            {
                assetKeys.Add(location.PrimaryKey);
            }

            Debug.Log("Asset keys loaded: " + assetKeys.Count);
            totalAssetCount = assetKeys.Count;
        }
        else
        {
            Debug.LogError("Failed to load asset keys.");
        }
    }

    public async Task<T> GetNextAsset(int currentIndex)
    {
        // Check if assetKeys has any elements to avoid DivideByZeroException
        if (assetKeys == null || assetKeys.Count == 0)
        {
            throw new InvalidOperationException("The asset list is empty. No assets to load.");
        }

        currentIndex = (currentIndex + 1) % assetKeys.Count;
        T loaded = await DownloadAssetAsync(assetKeys[currentIndex]);
        return loaded;
    }


    public async Task<T> GetCurrentAsset(int currentIndex)
    {
        if (currentIndex >= 0 && currentIndex < assetKeys.Count)
        {
            T loaded = await DownloadAssetAsync(assetKeys[currentIndex]);
            return loaded;
        }
        else
        {
            Debug.LogError("Invalid currentIndex: " + currentIndex);
            return defaultAsset;
        }
    }

    public async Task<T> ShowPreviousAsset(int currentIndex)
    {
        currentIndex = (currentIndex - 1 + assetKeys.Count) % assetKeys.Count;
        T loaded = await DownloadAssetAsync(assetKeys[currentIndex]);
        return loaded;
    }

    private async Task<T> DownloadAssetAsync(string key)
    {
        // Check if the asset is already downloaded
        AsyncOperationHandle<long> sizeHandle = Addressables.GetDownloadSizeAsync(key);
        await sizeHandle.Task;

        if (sizeHandle.Status == AsyncOperationStatus.Succeeded)
        {
            long assetSizeBytes = sizeHandle.Result;
            float assetSizeMB = (float)Math.Round(assetSizeBytes / (1024f * 1024f), 2);

            // Invoke event to notify asset size
            OnSingleAssetSize?.Invoke(assetSizeMB);

            if (assetSizeBytes > 0)
            {
                // The asset is not downloaded, start the download and track progress
                OnSingleAssetDownloading?.Invoke(0f); // Initial progress

                AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(key, true);
                long downloadedSizeBytes = 0;

                while (!downloadHandle.IsDone)
                {
                    long currentDownloadedSizeBytes = (long)(assetSizeBytes * downloadHandle.PercentComplete);
                    if (currentDownloadedSizeBytes > downloadedSizeBytes)
                    {
                        downloadedSizeBytes = currentDownloadedSizeBytes;
                        float downloadedSizeMB = (float)Math.Round(downloadedSizeBytes / (1024f * 1024f), 2);
                        OnSingleAssetDownloading?.Invoke(downloadedSizeMB);
                    }
                    await Task.Yield();
                }
            }
        }

        // Load the asset
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            T asset = handle.Result;

            // Invoke the downloaded event only if the asset was actually downloaded
            if (sizeHandle.Result > 0)
            {
                OnSingleAssetDownloaded?.Invoke();
            }

            return asset;
        }
        else
        {
            Debug.LogError("Failed to download asset: " + key);
            return defaultAsset;
        }
    }
}
