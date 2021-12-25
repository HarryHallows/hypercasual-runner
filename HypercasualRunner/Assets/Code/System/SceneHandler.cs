using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private string persistentScene; //Manager Scene 
    [SerializeField] public string[] scenes; //Dynamic Scenes e.g. menus/levels 

    [SerializeField] private string sceneName;

    [SerializeField] private Scene activeScene;

    public int currentScene;

    [SerializeField] private GameManager gm; //Game Manager

    // Start is called before the first frame update
    private void Awake()
    {
        //Initially load in the managers scene
        SceneManager.LoadSceneAsync(persistentScene, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(scenes[0], LoadSceneMode.Additive);

        #region Legacy Code
        //CurrentScene _getFirstScene = FindObjectOfType<CurrentScene>();
        //Scene _tempScene = SceneManager.GetSceneByBuildIndex(_getFirstScene.currentScene);
        //SceneManager.SetActiveScene(_tempScene);
        #endregion

        currentScene = 0;
    }

    private void Start()
    {
        ActiveScene();
    }

    [ContextMenu("NextScene")]
    public void NextScene()
    {
        Debug.Log("Going to next scene");

        #region Legacy Code
        /*for (int i = 0; i <= scenes.Length; i++)
        {
            CurrentScene _sceneCheck = FindObjectOfType<CurrentScene>();

            activeScene = SceneManager.GetSceneByBuildIndex(_sceneCheck.currentScene);
            SceneManager.SetActiveScene(activeScene);

            scenes[i] = activeScene.name;
            sceneName = activeScene.name;

            SceneManager.UnloadSceneAsync(sceneName);
            SceneManager.LoadSceneAsync(scenes[i + 1], LoadSceneMode.Additive);
           
            gm = FindObjectOfType<GameManager>();
            gm.PauseGame(true);

            currentScene++;
            break;
        }*/

        /*for (int i = 0; i < scenes.Length;)
        {
            SceneManager.UnloadSceneAsync(currentScene);
            SceneManager.LoadSceneAsync(scenes[currentScene + 1], LoadSceneMode.Additive);

            gm = FindObjectOfType<GameManager>();
            gm.PauseGame(true);

            currentScene++;
            break;
        }*/

        /*
        currentScene = FindObjectOfType<CurrentScene>().currentScene;
        */
        #endregion

        currentScene = FindObjectOfType<CurrentScene>().currentScene;

        SceneManager.UnloadSceneAsync(activeScene);
        SceneManager.LoadSceneAsync(scenes[currentScene + 1], LoadSceneMode.Additive);

        gm = FindObjectOfType<GameManager>();
        gm.PauseGame(true);

        Debug.Log("Get scene name" + activeScene.name);
        return;
        
    }

    public void ActiveScene()
    {
        Scene scene = SceneManager.GetSceneAt(+3);
        sceneName = scene.name;
        Debug.Log("Get scene name " + scene.name);
        SceneManager.SetActiveScene(scene);
        activeScene = SceneManager.GetActiveScene();
        Debug.Log("Get active scene name " + activeScene.name);
    }

    [ContextMenu("PreviousScene")]
    public void LoadPreviousScene()
    {
        currentScene = FindObjectOfType<CurrentScene>().currentScene;

        SceneManager.UnloadSceneAsync(activeScene);
        SceneManager.LoadSceneAsync(scenes[currentScene - 1], LoadSceneMode.Additive);

        gm = FindObjectOfType<GameManager>();
        gm.PauseGame(true);

        Debug.Log("Get scene name" + activeScene.name);
        return;
    }
}
