using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceModifier : MonoBehaviour
{
    public static Action<string, int, int> OnFaceChanged;


    [SerializeField] private int defaultFaceIndex;
    [SerializeField] private XROrigin xrOrigin;

    private int currentFaceIndex;

    private void Awake()
    {
        xrOrigin = FindObjectOfType<XROrigin>();
    }

    [SerializeField] GameObjectLoader gameObjectLoader;

    private void Start()
    {
        FaceModifierUI.OnFaceChangeAction += ChangeFace;
        currentFaceIndex = defaultFaceIndex;

        SetDefaultFace();
    }

    public void ChangeFace(int index)
    {
        currentFaceIndex += index;
        UpdateFace(currentFaceIndex);
    }

    public void SetDefaultFace()
    {
        UpdateFace(defaultFaceIndex);
    }


    private Dictionary<string, ARFace> facePool = new Dictionary<string, ARFace>();
    private async void UpdateFace(int faceIndex)
    {
        currentFaceIndex = Mathf.Clamp(faceIndex, 0, GameObjectLoader.totalAssetCount - 1);


        GameObject face1;

        if (faceIndex < 0)
        {
            face1 = await gameObjectLoader.ShowPreviousAsset(currentFaceIndex);

        }
        else
        {
            face1 = await gameObjectLoader.GetNextAsset(currentFaceIndex);
        }

        Debug.Log(face1.name);

        if (xrOrigin != null && xrOrigin.TryGetComponent(out ARFaceManager currentFaceManager))
        {
            if (currentFaceManager.facePrefab.name == face1.name)
                return;

            if (FindObjectOfType<ARFace>() != null)
            {
                // Instead of destroying the ARFace, deactivate it and add it to the pool
                var face = FindObjectOfType<ARFace>();
                face.gameObject.SetActive(false);
                facePool[face.gameObject.name] = face;
            }

            Destroy(currentFaceManager);
        }

        await Task.Yield();

        if (xrOrigin != null)
        {
            ARFaceManager faceManager = xrOrigin.AddComponent<ARFaceManager>();

            // Check if the face is in the pool
            if (facePool.ContainsKey(face1.name))
            {
                // If it is, reactivate it and remove it from the pool
                var face = facePool[face1.name];
                face.gameObject.SetActive(true);
                facePool.Remove(face.gameObject.name);
            }
            else
            {
                // If it's not in the pool, create a new one
                faceManager.facePrefab = face1;
            }

            OnFaceChanged?.Invoke(face1.name, currentFaceIndex, GameObjectLoader.totalAssetCount);
        }
    }

}
