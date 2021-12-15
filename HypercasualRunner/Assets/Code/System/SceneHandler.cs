using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private Object persistentScene; //Manager Scene 
    [SerializeField] private Object[] scenes; //Dynamic Scenes e.g. menus/levels 

    // Start is called before the first frame update
    private void Awake()
    {
        //Initially load in the managers scene
        SceneManager.LoadSceneAsync(persistentScene.name, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(scenes[0].name, LoadSceneMode.Additive);
    }

    [ContextMenu("NextScene")]
    public void LoadNextScene()
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            SceneManager.UnloadSceneAsync(scenes[i].name);
            SceneManager.LoadSceneAsync(scenes[i + 1].name, LoadSceneMode.Additive);
            break;
        }
    }

    [ContextMenu("PreviousScene")]
    public void LoadPreviousScene()
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            SceneManager.UnloadSceneAsync(scenes[i].name);
            SceneManager.LoadSceneAsync(scenes[i - 1].name, LoadSceneMode.Additive);
            break;
        }
    }
}
