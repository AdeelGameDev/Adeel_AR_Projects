using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class FilterManagerUI : MonoBehaviour
{
    public static Action<int> OnFilterChange;

    [SerializeField] private Button leftFilterButton;
    [SerializeField] private Button rightFilterButton;
    [SerializeField] private TextMeshProUGUI filterNameText;

    private int currentFilterIndex; // Current face index

    private void Awake()
    {
        leftFilterButton.onClick.AddListener(() => ChangeFilter(-1));
        rightFilterButton.onClick.AddListener(() => ChangeFilter(1));

        FilterManager.OnFilterChanged += FilterChanged;
    }

    private void FilterChanged(string faceName, int faceIndex, int totalFaces)
    {
        filterNameText.text = faceName;
        currentFilterIndex = faceIndex;

        // Update the buttons' interactable state
        leftFilterButton.interactable = currentFilterIndex != 0;
        rightFilterButton.interactable = currentFilterIndex != totalFaces - 1;
    }

    private void ChangeFilter(int index)
    {
        OnFilterChange?.Invoke(index);
    }

    private void OnDestroy()
    {
        FilterManager.OnFilterChanged -= FilterChanged;
    }
}
