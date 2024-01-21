using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Cinemachine Setting
    [SerializeField] float startFOV, endFOV, duration;
    [SerializeField] AnimationCurve curve;
    [SerializeField] MovementController movementScript;

    //Change Camera Settings
    [SerializeField] CinemachineVirtualCamera cam;
    CinemachineTransposer Transposer;

    [Header("Follow offset ")]
    [SerializeField] Vector3 followOffset;
    private Vector3 tempFollowOffset;


    [Header("High Speed Follow Offset")]
    [SerializeField] Vector3 highSpeedFollowOffset = new Vector3(9.47903061f, 8.09588623f, -9.26142883f);
    private float carSpeedCameraChangeThershold = 5f;
    private Vector3 originalFollowOffset;

    //Variables
    GameObject activeVehicle;
    Coroutine lerpRef;

    //Flags
    bool changeToNew = true;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void Start()
    {
        activeVehicle = null;

        cam = this.gameObject.GetComponent<CinemachineVirtualCamera>();
        cam.m_Lens.FieldOfView = startFOV;

        Transposer = cam.GetCinemachineComponent<CinemachineTransposer>();
        tempFollowOffset = Transposer.m_FollowOffset;

        originalFollowOffset = Transposer.m_FollowOffset;

        StartCoroutine(ChangeFollowOffset());
    }
    private void Update()
    {
        if (movementScript.hasVehicleChanged)
            CheckCurrentActiveObject();
    }

    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Camera)
            StartCoroutine(ChangeFOV());
        else if (state == GameManager.GameState.Start)
            ResetFlags();

        if (state != GameManager.GameState.Play)
            CheckCurrentActiveObject();

        if (state == GameManager.GameState.Cash)
            this.gameObject.SetActive(false);
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
            activeVehicle = movementScript.CheckActiveVehicle("ref");
            cam.Follow = activeVehicle.transform;
            cam.LookAt = activeVehicle.transform;
        }
    }
    #region On Click Change Camera
    public void OnClickChangeCamera()
    {

        if (changeToNew)
        {
            StartCoroutine(Lerp(Transposer.m_FollowOffset, followOffset));
            changeToNew = false;
        }
        else
        {
            StartCoroutine(Lerp(Transposer.m_FollowOffset, tempFollowOffset));
            changeToNew = true;
        }
    }

    IEnumerator Lerp(Vector3 start, Vector3 end)
    {
        float time = 0;
        while (time < duration)
        {
            Transposer.m_FollowOffset = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        time = 0;
    }
    #endregion

    void ResetFlags()
    {
        cam.m_Lens.FieldOfView = startFOV;
        if (!gameObject.activeSelf) { gameObject.SetActive(true); }
    }

    IEnumerator ChangeFollowOffset()
    {
        bool hasComponent = false;
        activeVehicle = movementScript.CheckActiveVehicle("ref");
        while (true)
        {

            if (activeVehicle.gameObject != null)
            {
                Debug.Log("Acitve Vehicle : " + activeVehicle.name);
                if (activeVehicle.gameObject.name.Equals("Car") && hasComponent != true)
                {
                    CarMovementController carMovementControllerScript = activeVehicle.GetComponent<CarMovementController>();
                    if (carMovementControllerScript != null)
                    {
                        hasComponent = true;
                        if (hasComponent)
                        {
                            if (lerpRef != null)
                                StopCoroutine(lerpRef);
                            Transposer.m_FollowOffset = originalFollowOffset;
                        }

                        while (activeVehicle.activeSelf && activeVehicle.gameObject.name.Equals("Car"))
                        {
                            if (carMovementControllerScript.carSpeed > carSpeedCameraChangeThershold)
                            {
                                yield return new WaitForSeconds(1f);
                                lerpRef = StartCoroutine(Lerp(Transposer.m_FollowOffset, highSpeedFollowOffset));
                            }
                            else
                            {
                                if (lerpRef != null)
                                    StopCoroutine(lerpRef);
                                StartCoroutine(Lerp(Transposer.m_FollowOffset, originalFollowOffset));
                            }
                            yield return null;
                        }
                    }
                }
                else
                {
                    StartCoroutine(Lerp(Transposer.m_FollowOffset, originalFollowOffset));
                    //Transposer.m_FollowOffset = originalFollowOffset;
                    hasComponent = false;
                }
            }
            else
            {
                Debug.LogWarning("Object is null. Please check and handle accordingly.");
            }
            yield return null;
        }
    }

}
