using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerController : MonoBehaviour
{
    public bool clockwise;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate(clockwise);
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
}
