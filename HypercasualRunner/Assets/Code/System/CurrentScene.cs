using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentScene : MonoBehaviour
{
    public int currentScene;

    public int SceneCount()
    {
        return currentScene++;
    }
}
