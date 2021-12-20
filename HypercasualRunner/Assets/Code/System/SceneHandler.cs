using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private string persistentScene; //Manager Scene 
    [SerializeField] private string[] scenes; //Dynamic Scenes e.g. menus/levels 

    // Start is called before the first frame update
    private void Awake()
    {
        //Initially load in the managers scene
        SceneManager.LoadSceneAsync(persistentScene, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(scenes[0], LoadSceneMode.Additive);
    }

    [ContextMenu("NextScene")]
    public void LoadNextScene()
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            SceneManager.UnloadSceneAsync(scenes[i]);
            SceneManager.LoadSceneAsync(scenes[i + 1], LoadSceneMode.Additive);
            break;
        }
    }

    [ContextMenu("PreviousScene")]
    public void LoadPreviousScene()
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            SceneManager.UnloadSceneAsync(scenes[i]);
            SceneManager.LoadSceneAsync(scenes[i - 1], LoadSceneMode.Additive);
            break;
        }
    }
}
