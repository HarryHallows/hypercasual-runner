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
    [SerializeField] public GameObject instructionImage;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] public GameObject endLevelScreen;
    [SerializeField] private GameObject pauseButton;

    public GameObject scoreParent;

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

    private void Start()
    {
        if (scoreParent != null)
        {
            scoreParent.SetActive(false);
        }

        if (instructionImage != null)
        {
            instructionImage.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SceneCheck();
    }
 
    private void SceneCheck()
    {
        //Get current scene
        //Debug.Log("get current scene from game manager");
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
        int _levelScore = (int)_score;

        scoreText.text = _levelScore.ToString("F0");

        gm.finalScore = _levelScore;
        return _levelScore = score;
    }

    #region ButtonFunctions 
    public void MenuScene()
    {
        gm.LoadMenu();

        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
            pauseButton.SetActive(true);
            instructionImage.SetActive(true);
        }

        if (endLevelScreen != null)
        {
            pauseButton.SetActive(true);
            instructionImage.SetActive(true);
            score = 0;
            scoreParent.SetActive(false);
            endLevelScreen.SetActive(false);
        }
    }

    public void PlayGame()
    {
        sceneHandler.NextScene();

        if (endLevelScreen != null)
        {
            endLevelScreen.SetActive(false);
        }
    }

    public void ReplayLevel()
    {
        sceneHandler.PlayAgain();
        return;
    }

    public void PauseButton()
    {
        pauseButton.SetActive(false);
        pauseScreen.SetActive(true);
        gm.PauseGame(true);
    }

    public void ResumeButton()
    {
        pauseButton.SetActive(true);
        pauseScreen.SetActive(false);
        gm.PauseGame(false);
    }

    public void QuitGame()
    {
        gm.OnApplicationQuit();
    }
    #endregion

    #region Legacy Code NOTE: Not enough time to complete
    /*
    private IEnumerator SceneTransition(SceneLoad _sceneChoice)
    {
        float _transition = transitionScreen.GetComponent<CanvasGroup>().alpha;
        _transition += Time.unscaledTime;

        yield return new WaitUntil(() => _transition >= 1);

        //gm.ChooseSceneToLoad(_sceneChoice);
        _transition -= Time.unscaledTime;
    }
    */
    #endregion
}
