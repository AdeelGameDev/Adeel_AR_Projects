
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class DragonSOLoader : RemoteAssetLoader<DragonSO>
{
    public static event Action OnDragonsLoaded;


    protected override async void Start()
    {
        base.Start();

        Debug.Log("start");
        await Task.Delay(5000);


    }

    protected override async void OnResourceLocationsLoaded(AsyncOperationHandle<IList<IResourceLocation>> handle)
    {
        base.OnResourceLocationsLoaded(handle);

        OnDragonsLoaded?.Invoke();
    }
}
