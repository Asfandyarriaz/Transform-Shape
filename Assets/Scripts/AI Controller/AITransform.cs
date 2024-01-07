using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITransform : MonoBehaviour
{
    [SerializeField] ParticleManager particleManagerScript;
    [SerializeField] GameObject detectionObject;
    AIData aiDataScript;

    AIController aiControllerScript;

    [Header("Raycast Setting Front")]
    [SerializeField] float frontLength;
    [SerializeField] Vector3 offsetFront;

    [Header("Raycast Setting Down")]
    [SerializeField] float downLength;
    [SerializeField] Vector3 offsetDown;

    [Header("Car Change Timer")]
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;
    //Variables 
    Quaternion resetRotation = Quaternion.Euler(0, 0, 0);

    //Flags
    private bool allowRaycastCheck = true;
    private bool changeToCar = false;
    private bool startCoroutine = true;
    private bool tranformActionPerformed = false;

    private void Start()
    {
        aiDataScript = GetComponent<AIData>();
        aiControllerScript = GetComponent<AIController>();
    }

    private void Update()
    {
        if (allowRaycastCheck)
        {
            CheckFront();
            CheckDown();
        }

        if (startCoroutine)
        {
            StartCoroutine(StartCarTimer());
            startCoroutine = false;
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
                Vector3 newPos = new Vector3(aiControllerScript.aiStartingPosition.position.x, currentPos.position.y, currentPos.position.z);
                aiControllerScript.tranformObjectsArr[i].transform.position = newPos;
                aiControllerScript.tranformObjectsArr[i].transform.rotation = resetRotation;
                aiControllerScript.tranformObjectsArr[i].SetActive(true);
                detectionObject.transform.SetParent(aiControllerScript.tranformObjectsArr[i].transform);

                //Set Particle as child and play
                PlayParticles(i);
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
        Debug.DrawRay(rayCastOrigin, Vector3.forward * frontLength, Color.green);
        if (Physics.Raycast(rayCastOrigin, Vector3.forward, out hit, frontLength))
        {
            //allowRaycastCheck = false;
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
            //allowRaycastCheck = false;
            tranformActionPerformed = true;
            PerfromActionOnRaycastHit(hit);
        }
    }

    void PerfromActionOnRaycastHit(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Obstacle"))
        {
            if (CheckCurrentActiveVehicle().name != "Tank")
            {
                ChangeToObject("Tank", DisableCurrentActive());
                //GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
            }
            allowRaycastCheck = true;
        }
        else if (hit.collider.CompareTag("Stairs"))
        {
            if (CheckCurrentActiveVehicle().name != "Character Walk")
            {
                ChangeToObject("Character Walk", DisableCurrentActive());
                GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
            }
            allowRaycastCheck = true;
        }
        else if (hit.collider.CompareTag("Water"))
        {
            if (CheckCurrentActiveVehicle().name != "Boat")
            {
                ChangeToObject("Boat", DisableCurrentActive());
                GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
            }
            allowRaycastCheck = true;
        }

        //TODO: Change To Fastest Availaible Vehicle
        //Set To Not 
        else if (!hit.collider.CompareTag("Water"))
        {
            if (CheckCurrentActiveVehicle().name == "Boat")
            {
                ChangeToObject(aiDataScript.fastestVehicleInCurrentLevel.name, DisableCurrentActive()); 
                GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
            }
            allowRaycastCheck = true;
        }
        if (hit.collider.CompareTag("Biketrail"))
        {
            if (CheckCurrentActiveVehicle().name != "Scooter")
            {
                ChangeToObject("Scooter", DisableCurrentActive());
                GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
            }
            allowRaycastCheck = true;
        }
        if (hit.collider.CompareTag("Fly"))
        {
            if (CheckCurrentActiveVehicle().name != "Airplane")
            {
                ChangeToObject("Airplane", DisableCurrentActive());
                GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
            }
            allowRaycastCheck = true;
        }

        //Change to car after plane
        
        tranformActionPerformed = false;
    }

    void ChangeToCar()
    {
        if (CheckCurrentActiveVehicle().name != "Car" || CheckCurrentActiveVehicle().name != "Boat" || CheckCurrentActiveVehicle().name != "Airplane")
        {
            ChangeToObject("Car", DisableCurrentActive());
            GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        }
    }  

    IEnumerator StartCarTimer()
    {
        float time = Random.Range(minTime, maxTime);

        while (time <= 0 || !tranformActionPerformed)
        {
            yield return new WaitForSeconds(1f);
            time -= 1;
        }

        if (time <= 0)
            ChangeToCar();

        startCoroutine = true;
    }

    //Tried to change difficulty using coroutine
    //But resulted in buggy behaviour
    /*IEnumerator PerfromActionOnRaycastHit(RaycastHit hit)
    {
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        if (hit.collider.CompareTag("Obstacle"))
        {
            if (CheckCurrentActiveVehicle().name != "Tank")
            {
                ChangeToObject("Tank", DisableCurrentActive());
                GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
            }
            allowRaycastCheck = true;
        }
        else if (hit.collider.CompareTag("Ramp"))
        {
            if (CheckCurrentActiveVehicle().name != "Character Walk")
            {
                ChangeToObject("Character Walk", DisableCurrentActive());
                GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
            }
            allowRaycastCheck = true;
        }
        else if (hit.collider.CompareTag("Water"))
        {
            if (CheckCurrentActiveVehicle().name != "Boat")
            {
                ChangeToObject("Boat", DisableCurrentActive());
                GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
            }
            allowRaycastCheck = true;
        }
        //Set To Not 
        else if (!hit.collider.CompareTag("Water"))
        {
            if (CheckCurrentActiveVehicle().name == "Boat")
            {
                ChangeToObject("Car", DisableCurrentActive());
                GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
            }
            allowRaycastCheck = true;
        }
        if (hit.collider.CompareTag("Biketrail"))
        {
            if (CheckCurrentActiveVehicle().name != "Scooter")
            {
                ChangeToObject("Scooter", DisableCurrentActive());
                GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
            }
            allowRaycastCheck = true;
        }
        if (hit.collider.CompareTag("Fly"))
        {
            if (CheckCurrentActiveVehicle().name != "Airplane")
            {
                ChangeToObject("Airplane", DisableCurrentActive());
                GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
            }
            allowRaycastCheck = true;
        }
    }*/
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
}
