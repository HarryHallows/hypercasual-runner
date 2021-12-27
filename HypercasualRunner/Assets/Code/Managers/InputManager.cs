using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

[DefaultExecutionOrder(-1)]
public class InputManager : PersistentSingleton<InputManager>
{
    [SerializeField] private GameManager gm; // game manager
    private Touch touch;

    Ray ray;
    RaycastHit hit;

    [Tooltip("Touch values for X and Y")]
    public float horizontalTouchPos;
    public float verticalTouchPos;

    //actual values unclamped
    public Vector3 currentPos;

    private Vector3 firstTouchPosition;
    public Vector3 lastTouchPosition;

    private float dragDistance; // minimum distance for a swipe

    private float previousXTouchPosition = 0f;
    private float previousYTouchPosition = 0f;

    public float width;
    public float height;
    [SerializeField] private float inputTimer = 1;

    public bool firstTouch;

    public bool touchFinished;

    public void Awake()
    {
        gm = FindObjectOfType<GameManager>();

        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        dragDistance = height * 15 / 100;
    }

    void Update()
    {
        TouchCheck();
    }

    private void TouchCheck()
    {
        if (gm.gameStarted)
        {
            //Debug.Log("input timer?");
            inputTimer -= Time.unscaledDeltaTime;
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            touch = Input.GetTouch(i);

            if (Input.touchCount > 0)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    //touch.position = currentPos;
                    firstTouchPosition = touch.position;
                    lastTouchPosition = touch.position;

                    touchFinished = false;

                    if (firstTouch == false && gm.gameStarted)
                    {
                        firstTouch = true;
                        gm.PauseGame(false);
                    }
                }
                else if (Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    lastTouchPosition = touch.position;

                    //Debug.Log(lastTouchPosition);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    lastTouchPosition = touch.position;
                    currentPos = lastTouchPosition;

                    touchFinished = true;

                    if (Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > dragDistance || Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y) > dragDistance)
                    {
                        if (Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y))
                        {
                            if (lastTouchPosition.x > firstTouchPosition.x)
                            {
                                //right swipe
                                // Debug.Log("swiping right");
                            }
                            else
                            {
                                //Left swipe
                                // Debug.Log("swiping left");
                            }
                        }
                        else
                        {
                            if (lastTouchPosition.y > firstTouchPosition.y)
                            {
                                //up swipe
                                // Debug.Log("swiping up");
                            }
                            else
                            {
                                //down swipe
                                // Debug.Log("swiping down");
                            }
                        }
                    }

                }
            }
        }
    }

}