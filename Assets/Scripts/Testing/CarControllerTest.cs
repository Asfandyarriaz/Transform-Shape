using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class CarControllerTest : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float maxRotationAngle = 30f;
    public LayerMask groundMask;
    [SerializeField]
    private AnimationCurve aniCurve;
    [SerializeField]
    private float Timer;

    [SerializeField] float movementSpeed;
    [SerializeField] float coefficient;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        SurfaceAlignment();
    }
    private void FixedUpdate()
    {
        Movement();
    }
    void Movement()
    {
        Vector3 counterMovement = new Vector3(-rb.velocity.x,0,-rb.velocity.z);
        rb.AddForce(Vector3.forward * movementSpeed);
        rb.AddForce(counterMovement * coefficient);
    }

    void SurfaceAlignment()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit info = new RaycastHit();
        Quaternion rotationRef = Quaternion.Euler(0, 0, 0);

        if (Physics.Raycast(ray, out info, groundMask))
        {

            //  rotationRef = Quaternion.Lerp(transform.rotation , Quaternion.FromToRotation(Vector3.up, info.normal), aniCurve.Evaluate(Timer));
            //  transform.rotation = Quaternion.Euler(rotationRef.eulerAngles.x, transform.eulerAngles.y,rotationRef.eulerAngles.z);

            rotationRef = Quaternion.Lerp(transform.rotation,Quaternion.FromToRotation(Vector3.up,info.normal), aniCurve.Evaluate(Timer));
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, info.normal), aniCurve.Evaluate(Timer));
            transform.rotation = Quaternion.Euler(rotationRef.eulerAngles.x, transform.rotation.y, rotationRef.eulerAngles.z);

        }
    }


}


