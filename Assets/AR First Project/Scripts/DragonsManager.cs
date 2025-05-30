using System;
using System.Threading.Tasks;
using UnityEngine;

public class DragonsManager : MonoBehaviour
{
    public static event Action<string, int, int> OnDragonChangedAction;
    public static event Action<string, int, int> OnSkinChangedAction;

    [SerializeField] private Transform dragonSpawnPoint;
    [SerializeField] private DragonSOLoader loader;
    [SerializeField] private int defaultDragonIndex = 0;
    [SerializeField] private int defaultDragonSkinIndex = 0;
    [SerializeField] private GameObject defaultDragonObject;

    [SerializeField] private Shader shader;

    private int currentDragonIndex;
    private int currentDragonSkinIndex;
    [SerializeField] private Dragon currentDragon;
    [SerializeField] private DragonSO currentDragonSO;

    private void Start()
    {
        EventsSubscribtion();

        currentDragonIndex = defaultDragonIndex;
        currentDragonSkinIndex = defaultDragonSkinIndex;
    }


    private void OnDestroy()
    {
        EventsUnSubscribtion();
    }


    private async Task InstantiateDragon(int index)
    {
        if (currentDragon != null)
        {
            Destroy(currentDragon.gameObject);
        }

        var dragonSO = await loader.GetCurrentAsset(index);
        currentDragonSO = dragonSO;
        currentDragon = Instantiate(dragonSO.dragonObject, dragonSpawnPoint.position, dragonSpawnPoint.rotation, transform).GetComponent<Dragon>();
        currentDragon.SetDragonSO(dragonSO);

        OnDragonChangedAction?.Invoke(dragonSO.dragonName, currentDragonIndex, DragonSOLoader.totalAssetCount);
    }

    private async Task UpdateSkin(int index)
    {

        currentDragon.UpdateSkin(currentDragonSO.skins[index]);
        OnSkinChangedAction?.Invoke(currentDragonSO.skins[index].name, currentDragonSkinIndex, currentDragonSO.skins.Length);

        await Task.Yield();
    }


    #region Events
    private async void DragonSOLoader_OnDragonsLoaded()
    {
        await InstantiateDragon(currentDragonIndex);
        await UpdateSkin(currentDragonSkinIndex);
    }

    private async void ChangeDragon(int change)
    {
        currentDragonIndex += change;
        currentDragonIndex = Mathf.Clamp(currentDragonIndex, 0, DragonSOLoader.totalAssetCount - 1);

        await InstantiateDragon(currentDragonIndex);
        await UpdateSkin(currentDragonSkinIndex);
    }

    private async void ChangeSkin(int change)
    {
        currentDragonSkinIndex += change;

        currentDragonSkinIndex = Mathf.Clamp(currentDragonSkinIndex, 0, currentDragonSO.skins.Length - 1);

        await UpdateSkin(currentDragonSkinIndex);
    }


    private void EventsSubscribtion()
    {
        DragonManagerUI.OnDragonChangeAction += ChangeDragon;
        DragonManagerUI.OnDragonSkinChangeAction += ChangeSkin;
        DragonSOLoader.OnDragonsLoaded += DragonSOLoader_OnDragonsLoaded;
    }

    private void EventsUnSubscribtion()
    {
        DragonManagerUI.OnDragonChangeAction -= ChangeDragon;
        DragonManagerUI.OnDragonSkinChangeAction -= ChangeSkin;
        DragonSOLoader.OnDragonsLoaded -= DragonSOLoader_OnDragonsLoaded;
    }
    #endregion
}
