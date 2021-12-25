using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    private bool flyingInput;

    //Floats 
    [Tooltip("Player collectable resource for flying")]
    public float fuel;
    private float finalFuel; // Score remainder

    private float _inputTime = .5f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSmoothingSpeed;

    [Header("Player Grounded")]

    [Tooltip("Cinemachine Virtual Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCam;

    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool grounded = true;
    public bool flying = false;

    [SerializeField] private CapsuleCollider groundedCollider;
    [SerializeField] private CapsuleCollider flyingCollider;

    [Tooltip("Useful for rough ground")]
    public float groundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float groundedRadius = 0.28f;

    private Vector3 spherePosition;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.Instance;
        gm = FindObjectOfType<GameManager>();
        cm = FindObjectOfType<CanvasManager>();

        rb = GetComponent<Rigidbody>();
        anim = transform.GetChild(1).GetComponent<Animator>();

        virtualCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
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
    }

    //Handles the rotations of the player
    private void HorizontalRotations(float _desiredRotation)
    {
        Vector3 _rotationAngle = new Vector3(0, _desiredRotation / 2, 0);
        Quaternion _rotation = Quaternion.Euler(_rotationAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rotation, Time.time * rotationSmoothingSpeed);
       
        #region Legacy Particle Curve Code
        /*if (fuel > 0 && !grounded)
        {
            for (int i = 0; i < jetfuel.Length; i++)
            {
                //ps = ParticleSystem
                ParticleSystem ps = jetfuel[i].GetComponent<ParticleSystem>();

                var forceOverLife = ps.forceOverLifetime;

                AnimationCurve curve = new AnimationCurve();

                curve.AddKey(0, _desiredRotation);
                forceOverLife.y = new ParticleSystem.MinMaxCurve(0.1f, .5f);
            }
        }*/
        #endregion
    }

    private void VerticalRotations(float _desiredRotation)
    {
        Vector3 _rotationAngle = new Vector3(_desiredRotation / 2, transform.eulerAngles.y, transform.eulerAngles.z);
        Quaternion _rotation = Quaternion.Euler(_rotationAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rotation, Time.time * rotationSmoothingSpeed);

        flyingInput = true;

        #region Legacy Particle Curve Code
        /*if (fuel > 0 && !grounded)
        {
            for (int i = 0; i < jetfuel.Length; i++)
            {
                ParticleSystem _particleSystem = jetfuel[i].GetComponent<ParticleSystem>();

                var forceOverLife = _particleSystem.forceOverLifetime;

                AnimationCurve curve = new AnimationCurve();

                curve.AddKey(0, _desiredRotation);
                forceOverLife.x = new ParticleSystem.MinMaxCurve(0.1f, .5f);
            }
        }*/
        #endregion
    }

    private void GroundedCheck()
    {
        //Debug.Log(grounded);

        // set sphere position, with offset
        spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        if (fuel > 0 && !grounded)
        {
            Fly(true);
            flying = true;
            rb.useGravity = false;

            _inputTime -= Time.deltaTime;

            if (!flyingInput)
            {
                //Clamp camera rotation to prevent camera julting downwards by accident
                Vector3 _rotationAngle = new Vector3(0, virtualCam.transform.eulerAngles.y, 0);
                Quaternion _rotation = Quaternion.Euler(_rotationAngle);
                virtualCam.transform.rotation = Quaternion.Lerp(virtualCam.transform.rotation, _rotation, Time.time * rotationSmoothingSpeed);
            }

            //Vertical input delay
            if (_inputTime < 0)
            {
                //Debug.Log("I should be accessing vertical input");

                groundedCollider.enabled = false;
                flyingCollider.enabled = true;
                rb.isKinematic = true;

               

                VerticalRotations(input.lastTouchPosition.y);

                fuel -= Time.deltaTime;

                for (int i = 0; i < jetfuel.Length; i++)
                {
                    if (jetfuel[i].activeSelf == false)
                    {
                        Debug.Log("Jet fuel on");
                        jetfuel[i].SetActive(true);

                        //dynamically adjusting the size of the jetpack flame based on current fuel usage
                        var _particleSystem = jetfuel[i].GetComponent<ParticleSystem>().main;
                        _particleSystem.startLifetime = fuel;

                    }
                }
            }
        }
        else if (fuel <= 0 && !grounded)
        {
            Debug.Log("should be falling");
            //Lose condition
            Fly(false);
            flying = false;

            rb.isKinematic = false;
            rb.useGravity = true;
            rb.mass = 5;

            moveSpeed = 0;

            for (int i = 0; i < jetfuel.Length; i++)
            {
                if (jetfuel[i].activeSelf == true)
                {
                    jetfuel[i].SetActive(false);
                }
            }
        }
        else if (fuel <= 0 && grounded || fuel > 0 && grounded)
        {
            //Grounded with or without fuel
            Fly(false);
            flying = false;
            _inputTime = .5f;

            //Debug.Log("Should be on the ground");

            rb.isKinematic = false;
            rb.mass = 1;
            groundedCollider.enabled = true;
            flyingCollider.enabled = false;

            moveSpeed = 5;

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

            flyingInput = false;
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
            gm.LoseGame();
            //Turn on lose/Reset Level screen
        }

        if (_col.CompareTag("FinishLine"))
        {
            for (int i = 0; i < podiums.Length; i++)
            {
                SettingActive(podiums[i], true);
                finalFuel = fuel;
            }

            _col.GetComponentInParent<Animator>().SetTrigger("FinishLine");
            SettingActive(_col.gameObject, false);
            moveSpeed = 10;
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
    //IF reached finish Podium/Rocket/Finish Line
    //Call Game Manager GameWin(*Check Final Score*)
    #endregion
}