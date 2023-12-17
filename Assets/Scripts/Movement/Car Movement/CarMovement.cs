using System.Runtime.CompilerServices;
using UnityEngine;

public class CarMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;
    [SerializeField] public bool allowMove;

    Rigidbody rb;

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
        allowMove = true;
    }

    public void Movement()
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
