using System;
using UnityEngine;
using UnityEngine.Rendering;

public class FilterManager : MonoBehaviour
{
    public static Action<string, int, int> OnFilterChanged;

    [SerializeField] private int defaultFilterIndex;
    [SerializeField] private Volume volume;

    private string filtersFolderPath = "VolumeProfiles";
    private VolumeProfile[] filters;
    private int currentFilterIndex;

    private void Start()
    {
        // Load all VolumeProfile assets from the specified folder
        filters = Resources.LoadAll<VolumeProfile>(filtersFolderPath);

        FilterManagerUI.OnFilterChange += ChangeFilter;
        currentFilterIndex = defaultFilterIndex;
        UpdateFilter(currentFilterIndex);
    }

    public void ChangeFilter(int index)
    {
        currentFilterIndex += index;
        UpdateFilter(currentFilterIndex);
    }

    public void SetDefaultFilter()
    {
        UpdateFilter(defaultFilterIndex);
    }

    private void UpdateFilter(int faceIndex)
    {
        currentFilterIndex = Mathf.Clamp(faceIndex, 0, filters.Length - 1);

        volume.profile = filters[currentFilterIndex];

        OnFilterChanged?.Invoke(filters[currentFilterIndex].name, currentFilterIndex, filters.Length);
    }
}
