using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITransform : MonoBehaviour
{
    [SerializeField] ParticleManager particleManagerScript;
    [SerializeField] GameObject detectionObject;
    AIData aiDataScript;

    AIController aiControllerScript;
    [Header("Layer Mask For All Raycast")]
    [SerializeField] LayerMask layer;

    [Header("Raycast Setting Front")]
    [SerializeField] float frontLength;
    [SerializeField] Vector3 offsetFront;

    [Header("Raycast Setting Down")]
    [SerializeField] float downLength;
    [SerializeField] Vector3 offsetDown;

    [Header("Car Change Timer")]
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;

    [Header("Turn Off Raycast Time")]
    [SerializeField] private float tunOffRaycastTime;
    //Variables 
    Quaternion resetRotation = Quaternion.Euler(0, 0, 0);

    //Flags
    private bool allowRaycastCheck = true;
    private bool tranformActionPerformed = false;
    private bool updateAllowed = false;
    private bool allowAirplaneCoroutine = true;

    private bool isRaycastCoroutineAllowed = true;

    //Timer
    private float timerDuration = 1f; // Set your desired timer duration in seconds
    private float timerStartTime;

    //Vehicle Timer
    private float vehicleStartTimer;
    private float vehicleDuration = 1f;

    //y offset Vehicle Transformation
    private float yOffset = 0.1f;

    [Header("Grounded Offset")]
    [SerializeField] Vector3 groundOffset;
    [SerializeField] float groundRaycastLength;
    [Header("Raycast Front Offset For Vehicle Change")]
    [SerializeField] Vector3 frontOffset;
    [SerializeField] float frontRaycastLength;

    #region State Handling
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
        if (state == GameManager.GameState.Play)
        {
            updateAllowed = true;
        }
        else
        {
            updateAllowed = false;
        }
    }
    #endregion
    private void Start()
    {
        aiControllerScript = GetComponent<AIController>();
        aiDataScript = GetComponent<AIData>();
        StartTimer();
    }

    private void Update()
    {
        if (Time.frameCount % 2 == 0)
        {
            if (updateAllowed)
            {
                ResetFlags();
                if (allowRaycastCheck)
                {
                    CheckFront();
                    CheckDown();
                }
                ChangeToVehicleTimer();

                if (allowAirplaneCoroutine)
                {
                    StartCoroutine(ChangeToVehicleAfterPlane());
                }
            }
        }
    }

    //TODO: optimize this function
    GameObject CheckCurrentActiveVehicle()
    {
        for (int i = 0; i < aiControllerScript.tranformObjectsArr.Length; i++)
        {
            if (aiControllerScript.tranformObjectsArr[i].activeSelf)
            {
                return aiControllerScript.tranformObjectsArr[i].gameObject;
            }
        }
        return null;
    }
    Transform DisableCurrentActive()
    {
        for (int i = 0; i < aiControllerScript.tranformObjectsArr.Length; i++)
        {
            if (aiControllerScript.tranformObjectsArr[i].activeSelf)
            {
                aiControllerScript.tranformObjectsArr[i].SetActive(false);
                return aiControllerScript.tranformObjectsArr[i].transform;
            }
        }
        return null;
    }

    //Change to vehicle and move to previously selected vehicle 
    void ChangeToObject(string transformToName, Transform currentPos)
    {
        for (int i = 0; i < aiControllerScript.tranformObjectsArr.Length; i++)
        {
            if (aiControllerScript.tranformObjectsArr[i].name == transformToName)
            {
                //movementControllerScript.tranformObjectsArr[i].transform.position = currentPos.position;
                Vector3 newPos = new Vector3(aiControllerScript.aiStartingPosition.position.x, currentPos.position.y + yOffset, currentPos.position.z);
                aiControllerScript.tranformObjectsArr[i].transform.position = newPos;
                aiControllerScript.tranformObjectsArr[i].transform.rotation = resetRotation;
                aiControllerScript.tranformObjectsArr[i].SetActive(true);

                //Set Detection object as the child of activated vehicle
                detectionObject.transform.SetParent(aiControllerScript.tranformObjectsArr[i].transform);
                detectionObject.transform.localPosition = new Vector3(0, 0, 0);

                //Set Particle as child and play
                PlayParticles(i);

                //TODO: Reset Flag Using Timers
                tranformActionPerformed = true;
                RestartTimer();
                //StartCoroutine(ResetFlags());
                break;
            }
        }
    }


    /// <summary>
    /// Raycasting will be done from Detection Object 
    /// If it hits any terrain or obstacle perform relevant logic
    /// </summary>
    /// <returns></returns>
    #region Detection Logic To Transfrom To Different Shapes 
    void CheckFront()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (detectionObject.transform.position + offsetFront);
        Debug.DrawRay(rayCastOrigin, transform.forward * frontLength, Color.green);
        if (Physics.Raycast(rayCastOrigin, Vector3.forward, out hit, frontLength))
        {           
            PerfromActionOnRaycastHit(hit);
        }
    }
    void CheckDown()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (detectionObject.transform.position + offsetDown);
        Debug.DrawRay(rayCastOrigin, Vector3.down * downLength, Color.red);
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, downLength))
        {
            PerfromActionOnRaycastHit(hit);
        }
    }
    bool RaycastDownForAirplane()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (detectionObject.transform.position + offsetDown);
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(rayCastOrigin, Vector3.down * hit.distance, Color.red);
            if (hit.collider.CompareTag("Climbable") || hit.collider.CompareTag("Stairs") || hit.collider.CompareTag("Biketrail") || hit.collider.CompareTag("Water"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }
    void PerfromActionOnRaycastHit(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Obstacle"))
        {
            ObstacleBehavior();
        }
        else if (hit.collider.CompareTag("Stairs"))
        {
            if (aiControllerScript.GenerateProbability(aiControllerScript.difficulty))
            {
                if (CheckCurrentActiveVehicle().name != "Character Walk")
                {
                    ChangeToObject("Character Walk", DisableCurrentActive());
                    aiControllerScript.hasVehicleChanged = true;
                    //GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
                }
            }
            else
            {
                if (isRaycastCoroutineAllowed)
                    StartCoroutine(TurnOffRaycast());
            }
            //allowRaycastCheck = true;
        }
        else if (hit.collider.CompareTag("Climbable"))
        {
            if (aiControllerScript.GenerateProbability(aiControllerScript.difficulty))
            {
                if (CheckCurrentActiveVehicle().name != "Character Walk")
                {
                    ChangeToObject("Character Walk", DisableCurrentActive());
                    aiControllerScript.hasVehicleChanged = true;
                    //GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
                }
            }
            else
            {
                if (isRaycastCoroutineAllowed)
                    StartCoroutine(TurnOffRaycast());
            }
            //allowRaycastCheck = true;
        }
        else if (hit.collider.CompareTag("Water"))
        {
            if (aiControllerScript.GenerateProbability(aiControllerScript.difficulty))
            {
                if (CheckCurrentActiveVehicle().name != "Boat")
                {
                    ChangeToObject("Boat", DisableCurrentActive());
                    aiControllerScript.hasVehicleChanged = true;
                }
            }
            else
            {
                if (isRaycastCoroutineAllowed)
                    StartCoroutine(TurnOffRaycast());
            }
            //allowRaycastCheck = true;
        }
        /*//Set To Not 
        else if (!hit.collider.CompareTag("Water"))
        {
            if (aiControllerScript.GenerateProbability(aiControllerScript.difficulty))
            {
                if (CheckCurrentActiveVehicle().name == "Boat")
                {
                    ChangeToObject(aiDataScript.fastestVehicleInCurrentLevel.name, DisableCurrentActive());
                    aiControllerScript.hasVehicleChanged = true;
                    //GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
                }
            }
            else
            {
                if (isRaycastCoroutineAllowed)
                    StartCoroutine(TurnOffRaycast());
            }
            //allowRaycastCheck = true;
        }*/
        else if (hit.collider.CompareTag("Biketrail"))
        {
            if (aiControllerScript.GenerateProbability(aiControllerScript.difficulty))
            {
                if (CheckCurrentActiveVehicle().name != "Scooter")
                {
                    ChangeToObject("Scooter", DisableCurrentActive());
                    aiControllerScript.hasVehicleChanged = true;
                    //GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
                }
            }
            else
            {
                if (isRaycastCoroutineAllowed)
                    StartCoroutine(TurnOffRaycast());
            }
            //allowRaycastCheck = true;
        }
        else if (hit.collider.CompareTag("Fly"))
        {
            if (aiControllerScript.GenerateProbability(aiControllerScript.difficulty))
            {
                if (CheckCurrentActiveVehicle().name != "Airplane")
                {
                    ChangeToObject("Airplane", DisableCurrentActive());
                    aiControllerScript.hasVehicleChanged = true;
                    //GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
                }
            }
            else
            {
                if (isRaycastCoroutineAllowed)
                    StartCoroutine(TurnOffRaycast());
            }
            //allowRaycastCheck = true;
        }

        //RestartVehicleTimer();
        //Change to car after plane
        //ChangeToCarAfterPlane();
    }

    void ChangeToVehicle()
    {
        //if (CheckCurrentActiveVehicle().name != "Car" && CheckCurrentActiveVehicle().name != "Boat" && CheckCurrentActiveVehicle().name != "Airplane")
        //{
            //Debug.Log("Change To Vehicle Called : " + CheckCurrentActiveVehicle().name + " Name : " + this.transform.parent.gameObject.name);
            if (CheckCurrentActiveVehicle().name != aiDataScript.fastestVehicleInCurrentLevel.name)
            {
                ChangeToObject(aiDataScript.fastestVehicleInCurrentLevel.name, DisableCurrentActive());
                aiControllerScript.hasVehicleChanged = true;
                //GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
            }
        //}
    }

    IEnumerator ChangeToVehicleAfterPlane()
    {
        allowAirplaneCoroutine = false;
        if (CheckCurrentActiveVehicle().name.Equals("Airplane"))
        {
            yield return new WaitForSeconds(Random.Range(3, 6));
            if (RaycastDownForAirplane())
            {
                if (aiControllerScript.GenerateProbability(aiControllerScript.difficulty))
                {
                    ChangeToObject(aiDataScript.fastestVehicleInCurrentLevel.name, DisableCurrentActive());
                    aiControllerScript.hasVehicleChanged = true;
                }
            }
            else
            {
                if (isRaycastCoroutineAllowed)
                    StartCoroutine(TurnOffRaycast());
            }
        }
        allowAirplaneCoroutine = true;
    }

    #region Raycast Vehicle Transform
    bool CheckFrontTransform()
    {
        Vector3 raycastOrigin = (detectionObject.transform.position + frontOffset);
        Debug.DrawRay(raycastOrigin, transform.forward * frontRaycastLength, Color.black);
        if (Physics.Raycast(raycastOrigin, Vector3.forward * frontRaycastLength, out RaycastHit hit, frontRaycastLength, layer))
        {
            //Debug.Log("Hit Raycast Front : " + hit.collider.tag);
            if (hit.collider.CompareTag("Climbable") || hit.collider.CompareTag("Stairs") || hit.collider.CompareTag("Biketrail") || hit.collider.CompareTag("Water"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return true;
    }

    //Check For Grounded
    //Then Check For Constraints
    //Then Perform Vehicle Change Into Fastest Vehicle In Level
    bool CheckGrounded()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = (detectionObject.transform.position + groundOffset);
        Debug.DrawRay(rayCastOrigin, Vector3.down * groundRaycastLength, Color.white);
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, groundRaycastLength, layer))
        {
            //Debug.Log("Check Ground Raycast Change To Vehicle Check : " + hit.collider.tag + " Name : " + this.transform.parent.gameObject.name);
            if (hit.collider.CompareTag("Climbable") || hit.collider.CompareTag("Stairs") || hit.collider.CompareTag("Biketrail") || hit.collider.CompareTag("Water"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    #endregion
    void ChangeToVehicleTimer()
    {
        if (Time.time - vehicleStartTimer >= vehicleDuration)
        {
            if (CheckFrontTransform() && CheckGrounded())
            {
                if (tranformActionPerformed == false)
                {
                    if (aiControllerScript.GenerateProbability(aiControllerScript.difficulty))
                    {
                        ChangeToVehicle();
                    }
                    else
                    {
                        RestartVehicleTimer();
                    }
                }
            }
            RestartVehicleTimer();
        }
    }
    #endregion



    //Play Particles according to the parents name 
    void PlayParticles(int index)
    {
        if (transform.parent.gameObject.name.Equals("AI_1"))
        {
            StartCoroutine(particleManagerScript.PlayTransformParticle(aiControllerScript.tranformObjectsArr[index].transform, particleManagerScript.transformParticleAI));
        }
        else if (transform.parent.gameObject.name.Equals("AI_2"))
        {
            StartCoroutine(particleManagerScript.PlayTransformParticle(aiControllerScript.tranformObjectsArr[index].transform, particleManagerScript.transformParticleAI2));
        }
        else if (transform.parent.gameObject.name.Equals("AI_3"))
        {
            StartCoroutine(particleManagerScript.PlayTransformParticle(aiControllerScript.tranformObjectsArr[index].transform, particleManagerScript.transformParticleAI3));
        }
    }

    void ResetFlags()
    {
        // Check if the timer has elapsed
        if (Time.time - timerStartTime >= timerDuration)
        {
            // Timer has elapsed, reset the flag
            tranformActionPerformed = false;

            // Restart the timer
            RestartTimer();
        }
    }
    private void StartTimer()
    {
        timerStartTime = Time.time;
        vehicleStartTimer = Time.time;
    }
    private void RestartTimer()
    {
        // Reset the timer start time to the current time
        timerStartTime = Time.time;
    }
    private void RestartVehicleTimer()
    {
        vehicleStartTimer = Time.time;
    }
    IEnumerator TurnOffRaycast()
    {
        isRaycastCoroutineAllowed = false;
        allowRaycastCheck = false;

        yield return new WaitForSeconds(tunOffRaycastTime);
        allowRaycastCheck = true;
        isRaycastCoroutineAllowed = true;
    }
    IEnumerator TurnOffRaycast(float time)
    {
        isRaycastCoroutineAllowed = false;
        allowRaycastCheck = false;
        yield return new WaitForSeconds(time);
        allowRaycastCheck = true;
        isRaycastCoroutineAllowed = true;
    }

    #region Change To Different Vehicles Functions
    //Change to tank if avalaible else change to airplane
    void ObstacleBehavior()
    {
        if (aiControllerScript.GenerateProbability(aiControllerScript.difficulty))
        {
            bool tankAvailable = true;
            for (int i = 0; i < aiDataScript.activeVehicles.Count; i++)
            {
                if (!aiDataScript.activeVehicles[i].Equals("Tank"))
                {
                    tankAvailable = false;
                }
                else
                {
                    tankAvailable = true;
                    break;
                }
            }

            if (tankAvailable)
            {
                if (CheckCurrentActiveVehicle().name != "Tank")
                {
                    ChangeToObject("Tank", DisableCurrentActive());
                    aiControllerScript.hasVehicleChanged = true;
                }
            }
            else
            {
                ChangeToObject("Airplane", DisableCurrentActive());
                aiControllerScript.hasVehicleChanged = true;
            }
        }
        else
        {
            if (isRaycastCoroutineAllowed)
                StartCoroutine(TurnOffRaycast());
        }
    }
    #endregion
}
