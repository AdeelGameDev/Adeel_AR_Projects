using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum CustomScene
    {
        Basic_Face_Filter,
        LoadingScene,
        Basic_Interactive_Scene,
        AR_Template_Scene,
        AR_Portal,
        ARMarker
    }


    /*    public static int GetEnumCount()
        {
            int enumCount = Enum.GetValues(typeof(CustomScene)).Length;
            return enumCount;
        }
    */

    public static void Load(int targetScene)
    {
        PlayerPrefs.SetInt("SceneToLoad", targetScene);

        SceneManager.LoadScene(CustomScene.LoadingScene.ToString(), LoadSceneMode.Single);
    }



    public static void LoaderCallBack()
    {
        Debug.Log(PlayerPrefs.GetString("SceneToLoad"));
        SceneManager.LoadScene(PlayerPrefs.GetInt("SceneToLoad"));
    }
}
