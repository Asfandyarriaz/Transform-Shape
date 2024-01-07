using System.Collections;
using UnityEngine;

public class TankMovement : MonoBehaviour, IInterfaceMovement
{
    /*[SerializeField] VehicleProperties vehicleProperties;

    [SerializeField] public bool allowMove;
    [SerializeField] float slowDown;

    [Header("Force")]
    [SerializeField] float forceBackwards;
    [SerializeField] float upwardsForce;
    Rigidbody rb;

    [Header("Force Offset")]
    [SerializeField] Vector3 upwardForceOffset;
    [Header("Flags")]

    [Header("Speed")]
    [SerializeField] private float speed;

    [Header("Speed Increment")]
    [SerializeField] private float incrementSpeedPercentage;
    //Flags
    [SerializeField] public bool forceEffect = false;
    [SerializeField] private float waitTimerForForceEffect;
    private bool runOnce = false;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Start)
        {
            runOnce = false;
        }
        if (state == GameManager.GameState.Play)
        {
            IncrementSpeed();
        }
        //Reset any flags on transform state 
        if (state == GameManager.GameState.Transform)
        {
            forceEffect = false;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Movement()
    {
        if (!forceEffect)
        {
            if (allowMove)
            {
                rb.velocity = Vector3.forward * speed;
            }
            else
            {
                rb.velocity = new Vector3(0, -speed, 0);
            }
        }
    }

    public void ForceBack(Vector3 position)
    {
        forceEffect = true;
        rb.AddForce(forceBackwards * Vector3.back, ForceMode.Impulse);
        rb.AddForce((transform.up + upwardForceOffset) * upwardsForce, ForceMode.Impulse);
        StartCoroutine(SetForceEffectToFalse());
    }

    IEnumerator SetForceEffectToFalse()
    {
        yield return new WaitForSeconds(waitTimerForForceEffect);
        forceEffect = false;
    }

    //5 % Increment with each level
    void IncrementSpeed()
    {
        
        if (vehicleProperties.currentUpgradeLevel >= 1 && runOnce != true)
        {
            speed = vehicleProperties.speed;
            incrementSpeedPercentage = incrementSpeedPercentage * vehicleProperties.currentUpgradeLevel -1;
            speed += Mathf.RoundToInt(vehicleProperties.speed * (incrementSpeedPercentage / 100));
            runOnce = true;
        }
    }*/
    [Header("Speed")]
    public float targetVelocity = 12f;  // Adjust the desired velocity
    public float forceMultiplier = 1f;  // Adjust the force multiplier

    [Header("Slope Rotation")]
    public float rotationSpeed = 45f;  // Adjust the rotation speed as needed
    public float maxSlopeAngle = 45f;  // Adjust the maximum slope angle to consider
    public LayerMask ground;
    public Vector3 offset;
    public float rayCastLength;


    [Header("Grounded")]
    public float rayCastGroundLength;
    Rigidbody rb;

    [Header("Air")]
    public float flySpeed;

    [Header("Force")]
    [SerializeField] float forceBackwards;
    [SerializeField] float upwardsForce;
    [SerializeField] Vector3 upwardForceOffset;

    [Header("Other Settings")]
    [SerializeField] VehicleProperties vehicleProperties;
    [SerializeField] private float waitTimerForForceEffect;

    //Flags
    [Header("Flags")]
    [SerializeField] public bool forceEffect = false;
    private bool runOnce = false;
    public bool allowMove;
    public bool allowRotate = true;

    //Variables
    private float tempForceMultiplier;
    private float tempTargertVelocity;

    private int slowSpeed = 4;
    private float slowDuration = 0.5f;
    private float resetDuration = 1.5f;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Start)
        {
            //runOnce = false;
        }
        if (state == GameManager.GameState.Play)
        {
            targetVelocity = vehicleProperties.speed;
            //IncrementSpeed();
        }
        //Reset any flags on transform state 
        if (state == GameManager.GameState.Transform)
        {
            forceEffect = false;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        tempForceMultiplier = forceMultiplier;
        tempTargertVelocity = targetVelocity;

    }
    public void Movement()
    {
        if (!forceEffect)
        {
            if (allowMove)
            {
                MoveForward();
                if (allowRotate)
                    RotateToSlope();
            }
        }
    }
    void MoveForward()
    {
        if (IsGrounded())
        {
            forceMultiplier = tempForceMultiplier;
            float acceleration = (targetVelocity - rb.velocity.magnitude) / Time.fixedDeltaTime;
            // Calculate force needed
            float force = rb.mass * acceleration;

            // Add force in the forward direction
            rb.AddForce(transform.forward * forceMultiplier * force);
        }
        else
        {
            forceMultiplier = 0;
            rb.AddForce(Vector3.forward * flySpeed);
        }
    }

    void RotateToSlope()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + offset, Vector3.down, out hit, rayCastLength, ground))
        {
            Debug.DrawRay(transform.position + offset, Vector3.down * rayCastLength, Color.yellow);
            // Check if the surface hit is a slope (based on the angle)
            if (Vector3.Angle(hit.normal, Vector3.up) > 0 && Vector3.Angle(hit.normal, Vector3.up) < maxSlopeAngle)
            {
                // Calculate the rotation based on the slope normal
                Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

                // Rotate the object smoothly
                transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, rayCastGroundLength))
        {
            Debug.DrawRay(transform.position, Vector3.down * rayCastGroundLength, Color.red);
            return true;
        }
        return false;
    }
    public void ForceBack(Vector3 position)
    {
        forceEffect = true;
        rb.AddForce(forceBackwards * Vector3.back, ForceMode.Impulse);
        rb.AddForce((transform.up + upwardForceOffset) * upwardsForce, ForceMode.Impulse);
        StartCoroutine(SetForceEffectToFalse());
    }

    IEnumerator SetForceEffectToFalse()
    {
        yield return new WaitForSeconds(waitTimerForForceEffect);
        forceEffect = false;
    }

    void ContraintXRotation()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
    }

    //Reduce speed by 4 times
    public IEnumerator SlowSpeedInWater()
    {
        float time = 0;
        float velocity = 0;
        while (time < slowDuration)
        {
            velocity = Mathf.Lerp(targetVelocity, slowSpeed, time / slowDuration);
            targetVelocity = velocity;
            time += Time.deltaTime;
            yield return null;
        }

    }
    public IEnumerator ResetSpeed()
    {
        float time = 0;
        float velocity = 0;
        while (time < resetDuration)
        {
            velocity = Mathf.Lerp(targetVelocity, tempTargertVelocity, time / resetDuration);
            targetVelocity = velocity;
            time += Time.deltaTime;
            yield return null;
        }
    }
    public void StopCar()
    {
        rb.velocity = new Vector3(0, 0, 0);
    }
}
