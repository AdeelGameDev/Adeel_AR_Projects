using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DownloadingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI downloadingText;
    [SerializeField] private GameObject loader;

    private IAssetLoader assetLoader;

    private void Start()
    {
        // Ensure that the loader GameObject has a component implementing IAssetLoader
        assetLoader = loader.GetComponent<IAssetLoader>();
        if (assetLoader != null)
        {
            // Subscribe to the OnSingleAssetDownloaded event
            assetLoader.OnSingleAssetDownloaded += UpdateDownloadingUI;
            assetLoader.OnSingleAssetDownloading += AssetLoader_OnSingleAssetDownloading;
            assetLoader.OnSingleAssetSize += AssetLoader_OnSingleAssetSize;
        }
        else
        {
            Debug.LogError("Loader GameObject does not have a component implementing IAssetLoader.");
        }
    }

    private float assetSize;

    private void AssetLoader_OnSingleAssetSize(float size)
    {
        assetSize = size;
    }

    private void AssetLoader_OnSingleAssetDownloading(float size)
    {
        downloadingText.gameObject.SetActive(true);
        downloadingText.text = "Downloading..." + size + "MB/" + assetSize + "MB";

    }

    private void OnDestroy()
    {
        if (assetLoader != null)
        {
            // Unsubscribe from the OnSingleAssetDownloaded event
            assetLoader.OnSingleAssetDownloaded -= UpdateDownloadingUI;
            assetLoader.OnSingleAssetDownloading -= AssetLoader_OnSingleAssetDownloading;
        }
    }

    // Method to update the UI when an asset is downloaded
    private async void UpdateDownloadingUI()
    {
        downloadingText.text = "Asset Downloaded!";

        await Task.Delay(1000);



        downloadingText.gameObject.SetActive(false);
        // You can add additional UI updates or loader animation stops here if needed
    }
}
