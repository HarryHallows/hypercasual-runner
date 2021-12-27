using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Singletons")]
    [SerializeField] private InputManager input;
    [SerializeField] private GameManager gm; // Game manager
    [SerializeField] private CanvasManager cm; // Canvas Manager
    [SerializeField] private JetPack jetpack;

    [Header("Movement")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;

    [SerializeField] private GameObject[] jetfuel;
    [SerializeField] private GameObject[] podiums;

    private bool finishLine;

    //Object Detection
    [SerializeField] private CapsuleCollider groundedCollider;
    [SerializeField] private CapsuleCollider flyingCollider;

    [SerializeField] private bool colliding;
    [SerializeField] private Vector3 detectionSize;
    [SerializeField] private Vector3 detectionPosition;
    [SerializeField] private LayerMask objectLayers;

    //Floats 
    [Tooltip("Player collectable resource for flying")]
    public float fuel;
    private float finalFuel; // Score remainder

    private float inputTime = .5f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float startMoveSpeed;
    [SerializeField] private float rotationSmoothingSpeed;

    [Header("Player Grounded")]
    [Tooltip("Cinemachine Virtual Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCam;

    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool grounded = true;
    public bool obstacleCollision = false;
    public bool flying = false;

    [Tooltip("Useful for rough ground")]
    public float groundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float groundedRadius = 0.28f;

    private Vector3 spherePosition;
    [SerializeField] private Vector3 bodyPosition;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask groundLayers;

    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.Instance;
        gm = FindObjectOfType<GameManager>();

        rb = GetComponent<Rigidbody>();
        anim = transform.GetChild(0).GetComponent<Animator>();

        virtualCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        finishLine = false;

        startMoveSpeed = 6;
        moveSpeed = startMoveSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!gm.paused)
        {
            // Position the cube.
            MoveDirection(input.lastTouchPosition.x);
            jetpack.GetComponent<JetPack>().Scale(fuel);
            CollisionCheck();

            if (gm.cm != null)
            {
                //Setting Game manager reference of canvas manager to players
                cm = gm.cm;
            }

            if (finishLine && fuel <= 0)
            {
                if (cm != null)
                {
                    cm.endLevelScreen.SetActive(true);
                    gm.PauseGame(true);
                }
            }
        }
    }

    //Handles the movement of the player while also taking the input from the input manager
    public void MoveDirection(float _touchValue)
    {
        //Rotate object towards limits of the X Values 
        //expodentially rotate based on outer limits 

        HorizontalRotations(_touchValue);
        GroundedCheck();

        Vector3 movement = transform.rotation * Vector3.forward;
        rb.MovePosition(transform.position += movement * moveSpeed * Time.deltaTime);
    }

    //Handles the rotations of the player
    private void HorizontalRotations(float _desiredRotation)
    {
        Vector3 _rotationAngle = new Vector3(0, _desiredRotation / 2, 0);
        Quaternion _rotation = Quaternion.Euler(_rotationAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rotation, Time.time * rotationSmoothingSpeed);
        //transform.rotation = _rotation;
    }

    private void VerticalRotations(float _desiredRotation)
    {
        Vector3 _rotationAngle = new Vector3(_desiredRotation / 2, transform.eulerAngles.y, transform.eulerAngles.z);

        //Clamping the rotation on the X to prevent camera flipping
        Vector3 _clampedAngle = new Vector3(Mathf.Clamp(_rotationAngle.x, 245, 475), transform.eulerAngles.y, transform.eulerAngles.z);
        Quaternion _rotation = Quaternion.Euler(_clampedAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rotation, Time.time * rotationSmoothingSpeed);
        //transform.rotation = _rotation;
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);

        if (fuel > 0 && !grounded)
        {
            Flying();
        }
        else if (fuel <= 0 && !grounded)
        {
            Falling();
        }
        else if (fuel <= 0 && grounded || fuel > 0 && grounded)
        {
            Running();
        }
    }

    private void CollisionCheck()
    {
        colliding = Physics.CheckBox(bodyPosition, detectionSize, Quaternion.identity, objectLayers);

        if (!colliding)
        {
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
        else
        {
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.None;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }

    private void Flying()
    {
        FlyAnimation(true);
        flying = true;
        rb.useGravity = false;
        rb.mass = 1;

        moveSpeed = startMoveSpeed * 2;

        groundedCollider.enabled = false;
        flyingCollider.enabled = true;

        VerticalRotations(input.lastTouchPosition.y);

        fuel -= Time.deltaTime;

        for (int i = 0; i < jetfuel.Length; i++)
        {
            if (jetfuel[i].activeSelf == false)
            {
                //Debug.Log("Jet fuel on");
                jetfuel[i].SetActive(true);

                //dynamically adjusting the size of the jetpack flame based on current fuel usage
                var _particleSystem = jetfuel[i].GetComponent<ParticleSystem>().main;
                _particleSystem.startLifetime = fuel;

                _particleSystem.startSize = (fuel / 100);
            }
        }
        
    }

    private void Falling()
    {
        Debug.Log("should be falling");
        //Lose condition
        FlyAnimation(false);
        flying = false;

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.mass = 5;

        moveSpeed -= Time.deltaTime;

        transform.position -= new Vector3(0, 100 * Time.deltaTime, 0);

        if (moveSpeed <= 0)
        {
            moveSpeed = 0;
        }

        for (int i = 0; i < jetfuel.Length; i++)
        {
            if (jetfuel[i].activeSelf == true)
            {
                jetfuel[i].SetActive(false);
            }
        }
    }

    private void Running()
    {
        //Grounded with or without fuel
        FlyAnimation(false);
        flying = false;

        if (transform.rotation.x == 0)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX;
        }
        else
        {
            Debug.Log(transform.rotation.x);
            Vector3 _correctAngle = new Vector3(0, transform.eulerAngles.x, 0);
            transform.rotation = Quaternion.Euler(_correctAngle);        
        }

        //Debug.Log("Should be on the ground");
        rb.mass = 1;
        groundedCollider.enabled = true;
        flyingCollider.enabled = false;

        moveSpeed = startMoveSpeed;

        for (int i = 0; i < jetfuel.Length; i++)
        {
            if (jetfuel[i].activeSelf == true)
            {
                jetfuel[i].SetActive(false);
            }
        }

        if (!rb.useGravity)
        {
            rb.useGravity = true;
        }
    }

    private void FlyAnimation(bool _fly)
    {
        anim.SetBool("flying", _fly);
    }

    private void OnDrawGizmos()
    {
        //Draw Ground Check
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spherePosition, groundedRadius);

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(bodyPosition, detectionSize);
    }

    //setting on and off objects 
    private void SettingActive(GameObject _object, bool _choice)
    {
        if (_choice == false)
        {
            _object.SetActive(false);

        }
        else
        {
            _object.SetActive(true);
        }
    }

    //Checking for collisions 
    private void OnTriggerEnter(Collider _col)
    {
        if (_col.CompareTag("Fuel"))
        {
            //Debug.Log("collided with Fuel");
            _col.gameObject.SetActive(false);

            fuel += 2;
        }

        if (_col.CompareTag("DeadBox"))
        {            
            //Turn on lose/Reset Level screen
            gm.CurrentLevel();
            Debug.Log("Should be dead");

            if (!finishLine)
            {
                gm.LoseGame();
                SettingActive(_col.gameObject, false);
                return;
            }
            return;
        }

        if (_col.CompareTag("FinishLine"))
        {
            for (int i = 0; i < podiums.Length; i++)
            {
                SettingActive(podiums[i], true);
                finalFuel = fuel;
            }

            cm.score = 0;
            finishLine = true;
            _col.GetComponentInParent<Animator>().SetTrigger("FinishLine");
            SettingActive(_col.gameObject, false);
            cm.scoreParent.SetActive(true);
            moveSpeed = 20;
        }

        if (_col.CompareTag("X1Podium")) 
        {
            //score *= 2;
            finalFuel *= 2;
            cm.ScoreCheck(finalFuel);
            Debug.Log(finalFuel);
            SettingActive(_col.gameObject, false);
        }
        else if (_col.CompareTag("X2Podium"))
        {
            finalFuel *= 4;
            cm.ScoreCheck(finalFuel);
            Debug.Log(finalFuel);
            SettingActive(_col.gameObject, false);

        }
        else if(_col.CompareTag("X3Podium"))
        {
            finalFuel *= 8;
            cm.ScoreCheck(finalFuel);
            Debug.Log(finalFuel);
            SettingActive(_col.gameObject, false);

        }
        else if(_col.CompareTag("X4Podium"))
        {
            finalFuel *= 10;
            cm.ScoreCheck(finalFuel);
            Debug.Log(finalFuel);
            SettingActive(_col.gameObject, false);

        }
        else if(_col.CompareTag("X5Podium"))
        {
            finalFuel *= 20;
            cm.ScoreCheck(finalFuel);
            Debug.Log(finalFuel);
            SettingActive(_col.gameObject, false);
        }
    }

    #region logic pseudo code
    //Raycast check to see if the player is grounded ? true : false
    //IF !grounded then start using jetpack fuel
    //reduce fuel % per second
    //IF fuel % == 0 && !grounded
    //Call Game Manager GameLose()
    //ELSE
    //IF reached finish Finish Line pop up podiums
    //Call Game Manager GameWin(*Check Final Score*)
    #endregion
}