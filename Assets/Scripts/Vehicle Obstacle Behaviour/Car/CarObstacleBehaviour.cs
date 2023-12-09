using UnityEngine;

public class CarObstacleBehaviour : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
