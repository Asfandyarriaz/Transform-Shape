using UnityEngine;

public class SlopeMovementScript : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float downwardsVelocity = -5f;
    [SerializeField] private float steepAngle = 30f * Mathf.Deg2Rad; // 30 degrees in radians
    private Vector3 NoVertical;
    public Transform groundingRay;
    public Camera cam;
    public LayerMask ground;

    private void Awake()
    {
        NoVertical = new Vector3(1, 0, 1);
    }

    private void Start()
    {
        //cam = transform.Find("../Camera/Camera").GetComponent<Camera>();
        //groundingRay = transform.Find("GroundingRay");
    }

    /*private Vector3 GetAxisDirection()
    {
        return ((cam.transform.right * Input.GetAxis("Horizontal") +
        cam.transform.forward * Input.GetAxis("Vertical")) + NoVertical).normalized;
    }
*/
    private void Update()
    {
        Vector3 direction = Vector3.back * speed;

        RaycastHit hit;
        if (Physics.Raycast(groundingRay.position, Vector3.down, out hit, 1f,ground) &&
            Vector3.Angle(hit.normal, Vector3.up) < steepAngle)
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);

            Vector3 normal = hit.normal;
            Vector3 cross = Vector3.Cross(transform.up, normal);
            if (cross.magnitude > 0.0001f)
                transform.rotation = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;

            direction = Quaternion.AngleAxis(Vector3.Angle(transform.up, normal), transform.up) * direction;
        }
        else
        {
            transform.Translate(new Vector3(0, downwardsVelocity, 0) * Time.deltaTime);

            Vector3 normal = Vector3.up;
            Vector3 cross = Vector3.Cross(transform.up, normal);
            if (cross.magnitude > 0.0001f)
                transform.rotation = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;
        }

        transform.Translate(Vector3.back * Time.deltaTime);
        if (direction.magnitude > 0) transform.LookAt(transform.position - direction, Vector3.up);
    }
}