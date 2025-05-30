using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragonManagerUI : MonoBehaviour
{
    public static event Action<int> OnDragonChangeAction;
    public static event Action<int> OnDragonSkinChangeAction;

    [SerializeField] private Button leftDragonButton;
    [SerializeField] private Button rightDragonButton;

    [SerializeField] private Button leftSkinButton;
    [SerializeField] private Button rightSkinButton;

    [SerializeField] private TextMeshProUGUI dragonNameText;
    [SerializeField] private TextMeshProUGUI skinNameText;

    private int totalDragons; // Total number of dragons
    private int currentDragonIndex; // Current dragon index

    private int totalSkins; // Total number of skins
    private int currentSkinIndex; // Current skin index

    private void Awake()
    {
        leftDragonButton.onClick.AddListener(() => ChangeDragon(-1));
        rightDragonButton.onClick.AddListener(() => ChangeDragon(1));

        leftSkinButton.onClick.AddListener(() => ChangeSkin(-1));
        rightSkinButton.onClick.AddListener(() => ChangeSkin(1));

        DragonsManager.OnSkinChangedAction += DragonsManager_OnSkinChangedAction;
        DragonsManager.OnDragonChangedAction += DragonsManager_OnDragonChangedAction;
    }

    private void Start()
    {
        EnableDisableDragonButtons(false);
        EnableDisableSkinButtons(false);
    }

    private void DragonsManager_OnDragonChangedAction(string dragonName, int dragonIndex, int totalDragons)
    {
        dragonNameText.text = dragonName;
        currentDragonIndex = dragonIndex;
        this.totalDragons = totalDragons;

        // Update the dragon buttons' interactable state
        leftDragonButton.interactable = currentDragonIndex != 0;
        rightDragonButton.interactable = currentDragonIndex != totalDragons - 1;
    }

    private void OnDestroy()
    {
        DragonsManager.OnSkinChangedAction -= DragonsManager_OnSkinChangedAction;
        DragonsManager.OnDragonChangedAction -= DragonsManager_OnDragonChangedAction;
    }

    private void DragonsManager_OnSkinChangedAction(string skinName, int skinIndex, int totalSkins)
    {
        skinNameText.text = skinName;
        currentSkinIndex = skinIndex;
        this.totalSkins = totalSkins;


        // Update the skin buttons' interactable state
        leftSkinButton.interactable = currentSkinIndex != 0;
        rightSkinButton.interactable = currentSkinIndex != totalSkins - 1;
    }

    private void ChangeDragon(int change)
    {
        EnableDisableDragonButtons(false);
        EnableDisableSkinButtons(false);

        OnDragonChangeAction?.Invoke(change);
    }

    private void ChangeSkin(int change)
    {
        EnableDisableSkinButtons(false);
        OnDragonSkinChangeAction?.Invoke(change);
    }

    private void EnableDisableDragonButtons(bool value)
    {
        leftDragonButton.interactable = value;
        rightDragonButton.interactable = value;
    }

    private void EnableDisableSkinButtons(bool value)
    {
        leftSkinButton.interactable = value;
        rightSkinButton.interactable = value;
    }
}
