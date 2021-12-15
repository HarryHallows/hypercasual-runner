using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Singletons")]
    [SerializeField] private InputManager input;


    [Header("Movement")]
    //Components
    [SerializeField] private Rigidbody rb; 

    //Vectors 
    private Vector3 velocity = Vector3.forward;

    //Floats 
    public float fuel;
    [SerializeField] private float moveSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.Instance;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Position the cube.
        MoveDirection(-input.touchPos.x);
    }

    private void FixedUpdate()
    {
        ForwardMovement();
    }

    private void ForwardMovement()
    {
        //Move Forward Constantly
        rb.velocity = transform.forward * moveSpeed;
    }

    //Handles the movement of the player while also taking the input from the input manager
    public void MoveDirection(float _xPosition)
    {     
        Debug.Log(_xPosition);
        
        //limiting how far the player can move target position
        if (transform.position.x >= 2.5f)
        {
            transform.position = new Vector2(2.5f, transform.position.y);
        }
        else if(transform.position.x <= -2.5f)
        {
            transform.position = new Vector2(-2.5f, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(_xPosition, transform.position.y);
        }

        //Rotate object towards limits of the X Values 
        //expodentially rotate based on outer limits 
        Rotations(_xPosition);
    }

    //Handles the rotation of the player
    private void Rotations(float _xRotation)
    {

        Vector3 rotationAngle = new Vector3(transform.eulerAngles.x, _xRotation * 30, transform.eulerAngles.z);
        Quaternion rotation = Quaternion.Euler(rotationAngle);
        transform.rotation = rotation;

        //Raycast check to see if the player is grounded ? true : false
        //IF !grounded then start using jetpack fuel
            //reduce fuel % per second
            //IF fuel % == 0 && !grounded
                //Call Game Manager GameLose()
            //ELSE
                //IF reached finish Podium/Rocket/Finish Line
                    //Call Game Manager GameWin(*Check Final Score*)
                    //

    }
}
