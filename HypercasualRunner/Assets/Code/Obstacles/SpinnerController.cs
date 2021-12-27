using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerController : MonoBehaviour
{
    public bool clockwise;
    private bool touchedPlayer = false;

    private float forceCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate(clockwise);

        if (touchedPlayer)
        {
            forceCoolDown -= Time.deltaTime;
        }

        if (forceCoolDown <= 0)
        {
            touchedPlayer = false;
            forceCoolDown = 1f;
        }
    }

    private void Rotate(bool _direction)
    {
        if (_direction)
        {
            transform.Rotate(0, 20 * Time.deltaTime, 0);
        }
        else
        {
            transform.Rotate(0, -20 * Time.deltaTime, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && forceCoolDown <= 0)
        {
            Debug.Log("Push player");
            touchedPlayer = true;
            collision.rigidbody.AddForce(Vector3.forward, ForceMode.Impulse);
        }
    }
}
