using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //clamp X rotation
        Vector3 desiredRotation = new Vector3(0, transform.eulerAngles.y, 0);
        Quaternion rotation = Quaternion.Euler(desiredRotation);
        //transform.rotation = Quaternion.LookRotation(rotation);
    }
}
