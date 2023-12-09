using UnityEngine;

public class CharacterObstacleBehaviour : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Character Collided");
            gameObject.SetActive(false);
            GameManager.Instance.UpdateGameState(GameManager.GameState.Lose);
        }
    }
}
