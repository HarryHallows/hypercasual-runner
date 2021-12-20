using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

[DefaultExecutionOrder(-1)]
public class InputManager : PersistentSingleton<InputManager>
{
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


    public void Awake()
    {
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
        for (int i = 0; i < Input.touchCount; i++)
        {
            touch = Input.GetTouch(i);

            if (Input.touchCount > 0)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    firstTouchPosition = touch.position;
                    lastTouchPosition = touch.position;
                }
                else if (Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    lastTouchPosition = touch.position;

                    //Debug.Log(lastTouchPosition);

                    //currentPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -10));

                    //horizontalTouchPos = Mathf.Clamp(currentPos.x, -2.5f, 2.5f);
                    //verticalTouchPos = Mathf.Clamp(currentPos.y, 3f, 5.5f);
                    //Debug.Log($"Y Touch Position :{verticalTouchPos}");
                }
                else if(touch.phase == TouchPhase.Ended)
                {
                    lastTouchPosition = touch.position;

                    if (Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > dragDistance || Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y) > dragDistance)
                    {
                        if (Mathf.Abs(lastTouchPosition.x - firstTouchPosition.x) > Mathf.Abs(lastTouchPosition.y - firstTouchPosition.y))
                        {
                            if (lastTouchPosition.x > firstTouchPosition.x)
                            {
                                //right swipe
                                Debug.Log("swiping right");
                            }
                            else
                            {
                                //Left swipe
                                Debug.Log("swiping left");
                            }
                        }
                        else
                        {
                            if (lastTouchPosition.y > firstTouchPosition.y)
                            {
                                //up swipe
                                Debug.Log("swiping up");
                            }
                            else
                            {
                                //down swipe
                                Debug.Log("swiping down");
                            }
                        }
                    }

                }
            }



        }
    }
  
}