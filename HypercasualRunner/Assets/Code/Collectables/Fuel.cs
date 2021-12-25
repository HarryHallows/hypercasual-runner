using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] private GameManager gm; // Game Manager
    [SerializeField] private bool changeDirection = false;
    [SerializeField] private bool positionRecorded = false;
    [SerializeField] private Vector3 currentPosition;
    [SerializeField] private float distance;

    private float moveSpeed;

    // Start is called before the first frame update
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        moveSpeed = Random.Range(20, 100);
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
    }

    //Responsible for all the code based animations
    private void Animate()
    {
        //Debug.Log(gm.paused);
        if (!gm.paused && !positionRecorded)
        {
            currentPosition = transform.position;
            positionRecorded = true;
        }

        if (positionRecorded)
        {
            StartCoroutine(Movement());
        }

        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(0, moveSpeed * Time.deltaTime, 0);
    }

    //Coroutine to move the collectable object up and down
    private IEnumerator Movement()
    {
        distance = Vector3.Distance(currentPosition, transform.position);
        //Debug.Log(distance);

        if (distance <= 0.1f)
        {
            changeDirection = true;
            yield return null;
        }

        if (distance >= 0.5f)
        {
            changeDirection = false;
            yield return null;
        }

        if (changeDirection)
        {
            transform.position += new Vector3(0, 1 * Time.deltaTime / 2, 0);
        }
        else
        {
            transform.position -= new Vector3(0, 1 * Time.deltaTime / 2, 0);
        }

    }
}
