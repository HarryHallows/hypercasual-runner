using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public CanvasManager cm; //Canvas manager
    [SerializeField] public SceneHandler sceneHandler;
    [SerializeField] public InputManager input;

    public string scene;

    public bool paused;
    public bool gameStarted = false;

    private GameObject hud;

    public int finalScore;

    private void Awake()
    {
        sceneHandler = FindObjectOfType<SceneHandler>();
        input = InputManager.Instance;

        //CurrentScene();

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
            if (cm == null)
            {
                cm = FindObjectOfType<CanvasManager>();
            }

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
        sceneHandler.ActiveScene();
        scene = sceneHandler.sceneName;
        //Debug.Log(scene);
    }

    public void LoseGame()
    {
        //Load Lose Scene
        CurrentScene();
        SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadSceneAsync("Lose", LoadSceneMode.Additive);
        return;
        //sceneHandler.ActiveScene();
    }

    public void LoadNextLevel()
    {
        CurrentScene();
        sceneHandler.NextScene();
        return;
    }

    public void LoadMenu()
    {
        CurrentScene();
        SceneManager.UnloadSceneAsync(scene);
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
    }

    public int CurrentLevel()
    {
        CurrentScene _currentScene = FindObjectOfType<CurrentScene>();
        return sceneHandler.previousLevel = _currentScene.previousLevel;
    }

    public bool PauseGame(bool _paused)
    {
        Debug.Log("Is paused: " + _paused);

        if (_paused)
        {
            input.firstTouch = false;
            gameStarted = true;

            if (cm != null)
            {
                cm.instructionImage.SetActive(false);
            }
            Time.timeScale = 0;
        }
        else
        {
            cm.instructionImage.SetActive(false);
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


