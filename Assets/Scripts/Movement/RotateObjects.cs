using UnityEngine;

public class RotateObjects : MonoBehaviour
{
    [SerializeField] GameObject[] objectToRotate;

    //public float rotationSpeedMultiplier = 10f;
    [SerializeField] float rotationSpeed;

    private void Update()
    {
        for (int i = 0; i < objectToRotate.Length; i++)
        {
            // Rotate the tyre around its local forward axis
            objectToRotate[i].transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        }
    }
}