using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangerUI : MonoBehaviour
{
    [SerializeField] private Button leftSceneButton;
    [SerializeField] private Button rightSceneButton;

    [SerializeField] private TextMeshProUGUI sceneNameText;

    private static int currentSceneIndex;

    private void Awake()
    {
        leftSceneButton.onClick.AddListener(() => ChangeScene(-1));
        rightSceneButton.onClick.AddListener(() => ChangeScene(1));
    }

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        currentSceneIndex = currentScene.buildIndex;
        sceneNameText.text = currentScene.name;

        // Disable the left button if it's the first scene
        leftSceneButton.interactable = currentSceneIndex != 0;

        // Disable the right button if it's the last scene
        rightSceneButton.interactable = currentSceneIndex != SceneManager.sceneCountInBuildSettings- 2;
    }

    private void ChangeScene(int index)
    {
        currentSceneIndex += index;
        currentSceneIndex = Mathf.Clamp(currentSceneIndex, 0, SceneManager.sceneCountInBuildSettings -2);

        if (currentSceneIndex == SceneManager.GetActiveScene().buildIndex)
        {
            return;
        }

        Loader.Load(currentSceneIndex);

        /*// Update the buttons' interactable state after changing the scene
        leftSceneButton.interactable = currentSceneIndex != 0;
        rightSceneButton.interactable = currentSceneIndex != SceneManager.sceneCount;*/
    }
}
