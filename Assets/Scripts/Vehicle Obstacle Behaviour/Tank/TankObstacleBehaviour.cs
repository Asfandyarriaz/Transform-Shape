using UnityEngine;

public class TankObstacleBehaviour : MonoBehaviour
{
    [SerializeField] TankMovement tankMovementScript;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
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
            GameManager.Instance.UpdateGameState(GameManager.GameState.Win);
        }
    }
}
