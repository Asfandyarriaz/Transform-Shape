using System.Collections;
using UnityEngine;

public class GliderMovement : MonoBehaviour, IInterfaceMovement
{
    public float speed = 5f;
    public float jumpForce = 8f;
    public float gravity = -30f;

    private CharacterController characterController;
    private Vector3 velocity;

    [SerializeField] private float rayCastLengthGround;


    //Flags
    public bool allowMove;
    public bool allowJump;

    //Variables
    private float tempSpeed;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        tempSpeed = speed;
    }
    public void Movement()
    {
        HandleMovement();
        GroundedSpeed();
        ApplyGravity();
    }
    void HandleMovement()
    {
        Vector3 moveVelocity = Vector3.forward * speed;
        characterController.Move(moveVelocity * Time.deltaTime);
    }
    public void GroundedSpeed()
    {
        if (allowJump != true)
        {
            if (IsGrounded())
            {
                speed = 2.5f;
                velocity.y = -2f; // Reset vertical velocity when grounded

                if (Input.GetKeyDown(KeyCode.W))
                {
                    velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                }
            }
            else
            {
                speed = tempSpeed;
            }
        }
    }
    public IEnumerator Jump()
    {
        yield return null;
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);   
        yield return null;
        allowJump = false;
    }
    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    bool IsGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayCastLengthGround, Color.green);
        if (Physics.Raycast(transform.position, Vector3.down, rayCastLengthGround))
        {
            return true;
        }
        return false;
    }
}
