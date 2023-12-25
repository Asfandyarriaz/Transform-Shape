using UnityEngine;

public class TankObstacleBehaviour : MonoBehaviour
{
    [SerializeField] TankMovement tankMovementScript;
    [SerializeField] float rayCastLength;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector3 rayCastOffset;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            tankMovementScript.ForceBack(collision.transform.position);
            collision.gameObject.SetActive(false);
            //Play Audio
            AudioManager.Instance.PlaySFX(AudioManager.Instance.objectBreakSound);
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            tankMovementScript.allowMove = false;
        }
        else
        {
            tankMovementScript.allowMove = true;
        }

        //Win Check
        if (collision.gameObject.CompareTag("Win"))
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Cash);
        }
    }
    private void Update()
    {
        if (IsGrounded())
        {
            tankMovementScript.allowMove = true;
        }
        else
        {
            tankMovementScript.allowMove = false;
        }
    }
    bool IsGrounded()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (transform.position + rayCastOffset);

        Debug.DrawRay(rayCastOrigin, Vector3.down * rayCastLength, Color.green);
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, rayCastLength, groundLayer))
        {
            return true;
        }
        return false;
    }
}
