using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRotations : MonoBehaviour
{

    private InputManager input;
    private PlayerController controller;

    [SerializeField] private float angle;

    // Start is called before the first frame update
    private void Awake()
    {
        input = InputManager.Instance;
        controller = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        VerticalRotation();
    }

    public void VerticalRotation()
    {

        //Debug.Log(transform.localEulerAngles.x + " camera current vertical");

        var angles = transform.localEulerAngles;
        angle = transform.localEulerAngles.x;

        if (angle > 160 )
        {
            //angles = 1
        }

        /*
        Vector3 _rotationAngle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, _desiredRotation * 30);
        Quaternion _rotation = Quaternion.Euler(_rotationAngle);
        transform.rotation = _rotation;  // =* Quaternion.Euler(0, _rotation.y, 0); NOTE: Possible solution to some input issues for rotation control
        */
    }
}
