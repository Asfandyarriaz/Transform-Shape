using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using static GameManager;
/// <summary>
/// I know Tyre, typo chill 
/// </summary>
public class RotateTire : MonoBehaviour
{
    [SerializeField] GameObject[] objectToRotate;
    private Rigidbody rb;
    //public float rotationSpeedMultiplier = 10f;
    [SerializeField] float rotationSpeed;

    void Start()
    {
        // Get the Rigidbody component attached to the GameObject
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb != null)
        {
            // Calculate rotation frequency based on velocity magnitude
            //float rotationSpeed = rb.velocity.magnitude * rotationSpeedMultiplier;

            for (int i = 0; i < objectToRotate.Length; i++)
            {
                // Rotate the tire around its local forward axis
                objectToRotate[i].transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            Debug.LogError("Rigidbody component not found!");
        }
    }
}