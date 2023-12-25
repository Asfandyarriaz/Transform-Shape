using System.Collections;
using UnityEngine;

public class TankMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;

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
        speed = vehicleProperties.speed;
        if (vehicleProperties.currentUpgradeLevel >= 1 && runOnce != true)
        {     
            incrementSpeedPercentage = incrementSpeedPercentage * vehicleProperties.currentUpgradeLevel -1;
            speed += Mathf.RoundToInt(vehicleProperties.speed * (incrementSpeedPercentage / 100));
            runOnce = true;
        }
    }
}
