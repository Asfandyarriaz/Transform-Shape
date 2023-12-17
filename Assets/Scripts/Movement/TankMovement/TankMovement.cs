using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
    //Flags
    [SerializeField] public bool forceEffect = false;
    [SerializeField] private float waitTimerForForceEffect;

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
                rb.velocity = Vector3.forward * vehicleProperties.speed;
            }
            else
            {
                rb.velocity = new Vector3(0, -vehicleProperties.speed, 0);
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
}
