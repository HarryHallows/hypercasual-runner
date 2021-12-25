using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    private GameManager gm; //Game Manager
    private SceneHandler sceneHandler;

    private InputManager input;
    [SerializeField] private GameObject instructionImage;

    //[SerializeField] private Animator transitionAnim;
    [SerializeField] private Image transitionScreen;
    [SerializeField] private TextMeshProUGUI scoreText;

    public int score;

    // Start is called before the first frame update
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        input = InputManager.Instance;
        sceneHandler = FindObjectOfType<SceneHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        SceneCheck();
    }
 
    private void SceneCheck()
    {
        //Get current scene
        Debug.Log("get current scene from game manager");
        gm.CurrentScene();

        if (gm.gameStarted == false && gm.scene != "Menu" && instructionImage != null)
        {
            //Debug.Log(input.firstTouch + " first touch?");

            if (input.firstTouch == true)
            {
                //Debug.Log("turn on image");
                if (instructionImage.activeSelf)
                {
                    instructionImage.SetActive(true);
                }
            }
            else
            {
                //Debug.Log("turn off image");
                if (instructionImage.activeSelf)
                {
                    instructionImage.SetActive(false);
                }
            }
        }
    }

    public int ScoreCheck(float _score)
    {
        int _finalScore = (int)_score;

        scoreText.text = _finalScore.ToString("F0");
        return _finalScore = score;
    }

    #region ButtonFunctions 
    public void LoseScene()
    {
        StartCoroutine(SceneTransition(SceneLoad.LOADLOSE));
    }

    public void MenuScene()
    {
        StartCoroutine(SceneTransition(SceneLoad.LOADMENU));
    }

    public void PlayGame()
    {
        //StartCoroutine(SceneTransition(SceneLoad.LOADNEXT));
        sceneHandler.NextScene();
    }

    public void QuitGame()
    {
        //StartCoroutine(SceneTransition(SceneLoad.LOADNEXT));
        gm.OnApplicationQuit();
    }
    #endregion

    private IEnumerator SceneTransition(SceneLoad _sceneChoice)
    {
        float _transition = transitionScreen.GetComponent<CanvasGroup>().alpha;
        _transition += Time.unscaledTime;

        yield return new WaitUntil(() => _transition >= 1);

        gm.ChooseSceneToLoad(_sceneChoice);
        _transition -= Time.unscaledTime;
    }

   
}
