using UnityEngine;

public class SlopeMovement : MonoBehaviour
{
    private CharacterController characterController;
    public float speed = 5f;
    public float rotationSpeed = 10f;
    public float slopeForce = 5f;
    public float slopeRayLength = 0.1f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Perform a raycast downward to detect the slope
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeRayLength))
            {
                // Calculate the slope direction and adjust the rotation
                Vector3 slopeDirection = Vector3.Cross(Vector3.Cross(hit.normal, Vector3.up), hit.normal).normalized;
                Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, slopeDirection) * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation, Time.deltaTime * rotationSpeed);

                // Move along the slope
                float slopeModifier = Mathf.Clamp01(hit.normal.y);
                Vector3 slopeMoveDirection = new Vector3(hit.normal.x, -hit.normal.y, hit.normal.z).normalized;
                characterController.Move(slopeMoveDirection * slopeForce * slopeModifier * Time.deltaTime);
            }
            else
            {
                // If not on a slope, move normally
                characterController.Move(moveDirection * speed * Time.deltaTime);
            }
        }
    }
}