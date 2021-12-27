using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private string persistentScene; //Manager Scene 
    [SerializeField] public string[] scenes; //Dynamic Scenes e.g. menus/levels 

    [SerializeField] public string sceneName;

    private Scene scene;
    private Scene activeScene;

    public int currentScene;
    public int previousLevel;

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

        UnloadCurrentScene();
        SceneManager.LoadSceneAsync(scenes[currentScene + 1], LoadSceneMode.Additive);

        ManagerPause();

        Debug.Log("Get scene name" + activeScene.name);
        return;
        
    }

    public void ActiveScene()
    {
        scene = SceneManager.GetSceneAt(3);
        sceneName = scene.name;
        SetActiveScene();
    }

    private void SetActiveScene()
    {
        if (scene.isLoaded)
        {
            SceneManager.SetActiveScene(scene);
            activeScene = SceneManager.GetActiveScene();
        }
        return;
    }

    private void UnloadCurrentScene()
    {
        SceneManager.UnloadSceneAsync(activeScene);
    }

    public void ReturnMenu()
    {
        ActiveScene();
        UnloadCurrentScene();
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
    }

    public void PlayAgain()
    {
        ActiveScene();
        UnloadCurrentScene();
        ManagerPause();
        SceneManager.LoadSceneAsync(scenes[previousLevel], LoadSceneMode.Additive);
    }

    [ContextMenu("PreviousScene")]
    public void LoadPreviousScene()
    {
        currentScene = FindObjectOfType<CurrentScene>().currentScene;

        UnloadCurrentScene();
        SceneManager.LoadSceneAsync(scenes[currentScene - 1], LoadSceneMode.Additive);

        ManagerPause();

        Debug.Log("Get scene name" + activeScene.name);
        return;
    }

    private void ManagerPause()
    {
        gm = FindObjectOfType<GameManager>();
        gm.PauseGame(true);
    }
}
