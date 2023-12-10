using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Cinemachine Setting
    [SerializeField] float startFOV, endFOV, duration;
    [SerializeField] AnimationCurve curve;
    [SerializeField] MovementController movementScript;
    CinemachineVirtualCamera cam;

    //Flags

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void Start()
    {
        cam = this.gameObject.GetComponent<CinemachineVirtualCamera>();
        cam.m_Lens.FieldOfView = startFOV;
    }
    private void Update()
    {
        if (GameManager.Instance.State == GameManager.GameState.Start)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    GameManager.Instance.UpdateGameState(GameManager.GameState.Camera);
                }
            }
        }
    }

    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Camera)
            StartCoroutine(ChangeFOV());
        else if (state == GameManager.GameState.Start)
            ResetFlags();

        if(state != GameManager.GameState.Play)
            CheckCurrentActiveObject();
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    IEnumerator ChangeFOV()
    {
        float time = 0;
        while (time < duration)
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, endFOV, curve.Evaluate(time / duration));
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    void CheckCurrentActiveObject()
    {
        if (cam.Follow.name != movementScript.CheckActiveVehicle("ref").name)
        {
            cam.Follow = movementScript.CheckActiveVehicle("ref").transform;
            cam.LookAt = movementScript.CheckActiveVehicle("ref").transform;
        }
    }

    void ResetFlags()
    {
        cam.m_Lens.FieldOfView = startFOV;
    }
}
