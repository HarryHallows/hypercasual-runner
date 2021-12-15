using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class InputManager : PersistentSingleton<InputManager>
{
    //[SerializeField] private PlayerController player;

    private RaycastHit2D hit;

    public Vector3 touchPos;
    private Vector2 startPos = Vector2.zero;

    public float width;
    public float height;

    public void Awake()
    {
        //player = GameObject.FindObjectOfType<PlayerController>();

        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
    }

    void Update()
    {
        TouchCheck(); 
    }

    private void TouchCheck()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                startPos = touch.position;
            }
            else if (Input.GetTouch(i).phase == TouchPhase.Moved)
            {
                touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, -10f));
                Debug.Log($"TouchPos :{touchPos}");
            }
            else
            {
                startPos = Vector2.zero;
            }
        }
    }
}
