using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Singletons")]
    [SerializeField] private InputManager input;


    [Header("Movement")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;

    [SerializeField] private GameObject playerModel;


    //Floats 
    [Tooltip("Player collectable resource for flying")]
    public float fuel;

    private float _inputTime = 0.5f;
    [SerializeField] private CapsuleCollider groundedCollider;
    [SerializeField] private CapsuleCollider flyingCollider;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSmoothingSpeed;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool grounded = true;
    public bool flying = false;

    [Tooltip("Useful for rough ground")]
    public float groundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float groundedRadius = 0.28f;

    [Tooltip("Checks for decents longer than a jump")]
    public float jumpTimer;
    [SerializeField] private float startJumpTimer;

    private Vector3 spherePosition;

   [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.Instance;

        rb = GetComponent<Rigidbody>();
        anim = transform.GetChild(1).GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Position the cube.
        MoveDirection(input.lastTouchPosition.x);
    }
   
    //Handles the movement of the player while also taking the input from the input manager
    public void MoveDirection(float _touchValue)
    {
        //Rotate object towards limits of the X Values 
        //expodentially rotate based on outer limits 

        HorizontalRotations(_touchValue);
        GroundedCheck();

        Vector3 movement = transform.rotation * Vector3.forward;
        transform.position += movement * moveSpeed * Time.deltaTime;       
        //rb.AddRelativeForce(Vector3.forward * moveSpeed * Time.fixedDeltaTime, ForceMode.Force);
    }

    //Handles the rotation of the player
    private void HorizontalRotations(float _desiredRotation)
    {
        Vector3 _rotationAngle = new Vector3(transform.eulerAngles.x, _desiredRotation, 0);
        Quaternion _rotation = Quaternion.Euler(_rotationAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rotation, Time.time * rotationSmoothingSpeed);
    }

    private void VerticalRotations(float _desiredRotation)
    {
        Vector3 _rotationAngle = new Vector3(_desiredRotation / 2, transform.eulerAngles.y, transform.eulerAngles.z);
        Quaternion _rotation = Quaternion.Euler(_rotationAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rotation, Time.time * rotationSmoothingSpeed);
    }

    private void GroundedCheck()
    {
        Debug.Log(grounded);

        // set sphere position, with offset
        spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        if (fuel > 0 && !grounded)
        {
            Fly(true);
            flying = true;
            rb.useGravity = false;

            _inputTime -= Time.deltaTime;

            Debug.Log(_inputTime);

            if (_inputTime < 0)
            {
                Debug.Log("I should be accessing vertical input");
                groundedCollider.enabled = false;
                flyingCollider.enabled = true;
                rb.isKinematic = true;

                VerticalRotations(input.lastTouchPosition.y);
            }
        }
        else if (fuel <= 0 && !grounded)
        {
            Debug.Log("should be falling");
            //Lose condition
            Fly(false);
            flying = false;
            rb.useGravity = true;
        }
        else if (fuel <= 0 && grounded || fuel > 0  && grounded)
        {
            //Grounded with or without fuel
            Fly(false);
            flying = false;
            _inputTime = 0.5f;

            Debug.Log("Should be on the ground");

            rb.isKinematic = false;
            groundedCollider.enabled = true;
            flyingCollider.enabled = false;

            if (!rb.useGravity)
            {
                rb.useGravity = true;
            }
        }

    }
    
    private void Fly(bool _fly)
    {
        anim.SetBool("flying", _fly);
    }

    private void OnDrawGizmos()
    {
        //Draw Ground Check
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spherePosition, groundedRadius);
    }

    #region logic pseudo code
    //Raycast check to see if the player is grounded ? true : false
    //IF !grounded then start using jetpack fuel
    //reduce fuel % per second
    //IF fuel % == 0 && !grounded
    //Call Game Manager GameLose()
    //ELSE
    //IF reached finish Podium/Rocket/Finish Line
    //Call Game Manager GameWin(*Check Final Score*)
    #endregion
}