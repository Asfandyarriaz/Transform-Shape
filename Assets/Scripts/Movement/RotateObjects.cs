using System.Collections;
using UnityEngine;
using static GameManager;

public class RotateTire : MonoBehaviour
{
    [SerializeField] GameObject[] objectToRotate;
    private Rigidbody rb;
    public float rotationSpeedMultiplier = 10f;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }
    private void GameManagerOnGameStateChanged(GameState state)
    {
        if(state == GameManager.GameState.Play)
        {
            StartCoroutine(StartRotate());
        }
        else
        {
            StopCoroutine(StartRotate());
        }
    }

    void Start()
    {
        // Get the Rigidbody component attached to the GameObject
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator StartRotate()
    {
        while (true)
        {
            if (rb != null)
            {
                // Calculate rotation speed based on velocity magnitude
                float rotationSpeed = rb.velocity.magnitude * rotationSpeedMultiplier;

                for (int i = 0; i < objectToRotate.Length; i++)
                {
                    // Rotate the tire around its local forward axis
                    objectToRotate[i].transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                Debug.LogError("Rigidbody component not found!");
            }
        }
    }
}