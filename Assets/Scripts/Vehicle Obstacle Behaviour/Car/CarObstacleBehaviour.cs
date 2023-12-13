using UnityEngine;

public class CarObstacleBehaviour : MonoBehaviour
{
    [SerializeField] CarMovement carMovementScript;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            collision.gameObject.SetActive(false);
            //Play Audio
            AudioManager.Instance.PlaySFX(AudioManager.Instance.objectBreakSound);
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            carMovementScript.allowMove = false;
        }
        else
        {
            carMovementScript.allowMove = true;
        }

        //Win Check
        if (collision.gameObject.CompareTag("Win"))
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Win);
        }
    }
}
