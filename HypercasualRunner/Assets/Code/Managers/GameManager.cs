using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneLoad
{
    LOADLOSE, LOADWIN, LOADMENU, LOADNEXT, LOADQUIT
}

public class GameManager : MonoBehaviour
{
    public SceneLoad sceneLoader;

    [SerializeField] public SceneHandler sceneHandler;
    [SerializeField] public InputManager input;

    public string scene;

    public bool paused;
    public bool gameStarted = false;

    private GameObject hud;

    private void Awake()
    {
        sceneHandler = FindObjectOfType<SceneHandler>();
        input = InputManager.Instance;

        CurrentScene();

        sceneHandler.enabled = true;
    }

    private void Update()
    {
        if (scene == "Menu" || scene == "Lose" || scene == "Win")
        {
            if (hud != null)
            {
                hud.SetActive(false);
            }
            else
            {
                hud = GameObject.FindGameObjectWithTag("HUD");
            }
        }
        else
        {
            if (hud != null)
            {
                hud.SetActive(true);
            }
            else
            {
                hud = GameObject.FindGameObjectWithTag("HUD");
            }
        }
    }

    private void WinGame()
    {
        //Load win Game
        CurrentScene();
        SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadSceneAsync("Win", LoadSceneMode.Additive);
    }

    public void CurrentScene()
    {
        scene = SceneManager.GetActiveScene().name;
        //Debug.Log(scene);
    }

    public void LoseGame()
    {
        //Load Lose Scene
        CurrentScene();
        SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadSceneAsync("Lose", LoadSceneMode.Additive);
    }

    public void LoadNextLevel()
    {
        CurrentScene();
        sceneHandler.NextScene();
        return;
    }

    //TODO:: REPLACE THIS WITH WORKING FUNCTION BUGGY
    public void ChooseSceneToLoad(SceneLoad _sceneChoice)
    {
        switch (sceneLoader)
        {
            case SceneLoad.LOADLOSE:
                LoseGame();
                break;

            case SceneLoad.LOADWIN:
                WinGame();
                break;
                
            case SceneLoad.LOADMENU:
                LoadMenu();
                break;
            
            case SceneLoad.LOADNEXT:
                Debug.Log("Loading next scene?");
                LoadNextLevel();
                break;

            case SceneLoad.LOADQUIT:
                OnApplicationQuit();
                break;
        }
    }

    public void LoadMenu()
    {
        CurrentScene();
        SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
    }

    public bool PauseGame(bool _paused)
    {
        Debug.Log("Is paused: " + _paused);

        if (_paused)
        {
            gameStarted = true;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            gameStarted = false;
        }

        return paused = _paused;
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

}


