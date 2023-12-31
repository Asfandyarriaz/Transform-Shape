using UnityEngine;
using static GameManager;
/// <summary>
/// I know Tyre, typo chill 
/// </summary>
public class RotateObjects : MonoBehaviour
{
    [SerializeField] GameObject[] objectToRotate;

    //public float rotationSpeedMultiplier = 10f;
    [SerializeField] float rotationSpeed;

    void Start()
    {

    }

    private void Update()
    {

            // Calculate rotation frequency based on velocity magnitude
            //float rotationSpeed = rb.velocity.magnitude * rotationSpeedMultiplier;

            for (int i = 0; i < objectToRotate.Length; i++)
            {
                // Rotate the tire around its local forward axis
                objectToRotate[i].transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
            }

    }
}