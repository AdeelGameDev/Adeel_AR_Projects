using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FaceModifierUI : MonoBehaviour
{
    public static Action<int> OnFaceChangeAction;

    [SerializeField] private GameObject DownloadintTExt;

    [SerializeField] private Button leftFaceButton;
    [SerializeField] private Button rightFaceButton;
    [SerializeField] private TextMeshProUGUI faceNameText;

    private int totalFaces; // Total number of faces
    private int currentFaceIndex; // Current face index

    private void Awake()
    {
        leftFaceButton.onClick.AddListener(() => ChangeFace(-1));
        rightFaceButton.onClick.AddListener(() => ChangeFace(1));

        FaceModifier.OnFaceChanged += FaceChanged;
    }

    private void FaceChanged(string faceName, int faceIndex, int totalFaces)
    {
        faceNameText.text = faceName;
        currentFaceIndex = faceIndex;
        this.totalFaces = totalFaces;

        // Update the buttons' interactable state
        leftFaceButton.interactable = currentFaceIndex != 0;
        rightFaceButton.interactable = currentFaceIndex != totalFaces - 1;

        DownloadintTExt.SetActive(false);

    }

    private void ChangeFace(int index)
    {
        leftFaceButton.interactable = false;
        rightFaceButton.interactable = false;
        DownloadintTExt.SetActive(true);

        OnFaceChangeAction?.Invoke(index);
    }

    private void OnDestroy()
    {
        FaceModifier.OnFaceChanged -= FaceChanged;
    }
}
