using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] GameObject tranformObjectsArr;
    [SerializeField] float speed;
    CharacterController characterController;
    IEnumerator playCoroutine;

    #region State Handling
    /// <summary>
    /// State Handling
    /// </summary>
    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if(state == GameManager.GameState.Play)
        {
            playCoroutine = Play();
            StartCoroutine(playCoroutine);
        }
        else
        {
            StopCoroutine(playCoroutine);
        }
    }
    #endregion

    private void Start()
    {
        characterController = tranformObjectsArr.GetComponent<CharacterController>();
        playCoroutine = Play();
    }
    #region GameLogic
    /// <summary>
    /// Game Logic
    /// </summary>
    IEnumerator Play()
    {
        while(true)
        {
            characterController.Move(Vector3.forward * speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnClickChangeState()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Start);
    }
    #endregion
}
