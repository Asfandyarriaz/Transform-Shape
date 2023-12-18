using UnityEngine;
/// <summary>
/// Controls Function on button click for transform buttons
/// </summary>
/// 

//TODO: Change Hardcoded values
public class ButtonController : MonoBehaviour
{
    [SerializeField] ParticleManager particleManagerScript;
    MovementController movementControllerScript;

    //Variables 
    Quaternion resetRotation = Quaternion.Euler(0, 0, 0);
    private void Start()
    {
        movementControllerScript = GetComponent<MovementController>();
    }
    public void OnClickTransformCharacterWalk()
    {
        ChangeToObject("Character Walk", DisableCurrentActive()); //Set name same as object under TransformList object in hierarchy
        GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);
    }

    public void OnClickTransformCar()
    {
        ChangeToObject("Car", DisableCurrentActive());
        GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);
    }
    public void OnClickTransformTank()
    {
        ChangeToObject("Tank", DisableCurrentActive());
        GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);
    }
    public void OnClickTransformBoat()
    {
        ChangeToObject("Boat", DisableCurrentActive());
        GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);
    }
    public void OnClickTransformPlane()
    {
        ChangeToObject("Airplane", DisableCurrentActive());
        GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);
    }
    public void OnClickTransformScooter()
    {
        ChangeToObject("Scooter", DisableCurrentActive());
        GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);
    }

    Transform DisableCurrentActive()
    {
        for(int i = 0; i < movementControllerScript.tranformObjectsArr.Length; i++) 
        {
            if (movementControllerScript.tranformObjectsArr[i].activeSelf)
            {
                movementControllerScript.tranformObjectsArr[i].SetActive(false);
                return movementControllerScript.tranformObjectsArr[i].transform;
            }
        }
        return null;
    }

    //Change to vehicle and move to previously selected vehicle 
    void ChangeToObject(string transformToName, Transform currentPos)
    {
        for (int i = 0; i < movementControllerScript.tranformObjectsArr.Length; i++)
        {
            if (movementControllerScript.tranformObjectsArr[i].name == transformToName)
            {
                //movementControllerScript.tranformObjectsArr[i].transform.position = currentPos.position;
                Vector3 newPos = new Vector3(movementControllerScript.startingPosition.position.x, currentPos.position.y, currentPos.position.z);
                movementControllerScript.tranformObjectsArr[i].transform.position = newPos;
                movementControllerScript.tranformObjectsArr[i].transform.rotation = resetRotation;
                Debug.Log("Change To Object Method");
                movementControllerScript.tranformObjectsArr[i].SetActive(true);

                //Set Particle as child and play
                StartCoroutine(particleManagerScript.PlayTransformParticle(movementControllerScript.tranformObjectsArr[i].transform));
                break;
            }
        }
    }
}
