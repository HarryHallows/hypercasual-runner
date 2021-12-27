using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentScene : MonoBehaviour
{
    [SerializeField] private SceneHandler sceneHandler;
    [SerializeField] private GameManager gm; // Game Manager
    public int currentScene;
    public int previousLevel;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        sceneHandler = FindObjectOfType<SceneHandler>();
        if (gm != null)
        {
            gm.enabled = true;
            sceneHandler.ActiveScene();
            gm.CurrentScene();
        }
    }
}
