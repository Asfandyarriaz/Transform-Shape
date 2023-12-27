using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Controls Function on button click for transform buttons
/// </summary>
/// 

//TODO: Change Hardcoded values
public class ButtonController : MonoBehaviour
{
    [SerializeField] ParticleManager particleManagerScript;
    MovementController movementControllerScript;

    [Header("Buttons")]
    [SerializeField] Image[] buttonArray;                //0.Character 1.Car 2.Tank 3.Scooter 4.Boat 5.Airplane
    [SerializeField] Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1.2f);

    [Header("Images")]
    [SerializeField] Sprite currentSelectedImage;
    [SerializeField] Sprite deselectedImage;

    [Header("Variables")]
    //Variables 
    Quaternion resetRotation = Quaternion.Euler(0, 0, 0);
    [SerializeField] private float airplaneYOffset = 0.1f;
    [SerializeField] private float yOffset = 0.1f;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    //TODO: Get current level from save system
    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Start)
        {
            SetCurrentActiveVehicleSpriteImage();
        }
    }

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

        SetBackgroundImageAndDeselectAllOthers(0);
    }

    public void OnClickTransformCar()
    {
        ChangeToObject("Car", DisableCurrentActive());
        GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);

        SetBackgroundImageAndDeselectAllOthers(1);
    }
    public void OnClickTransformTank()
    {
        ChangeToObject("Tank", DisableCurrentActive());
        GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);

        SetBackgroundImageAndDeselectAllOthers(2);
    }
    public void OnClickTransformBoat()
    {
        ChangeToObject("Boat", DisableCurrentActive());
        GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);

        SetBackgroundImageAndDeselectAllOthers(4);
    }
    public void OnClickTransformPlane()
    {
        ChangeToObject("Airplane", DisableCurrentActive());
        ChangeToObject("Airplane", DisableCurrentActive(), "Ref Override");
        GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);

        SetBackgroundImageAndDeselectAllOthers(5);

        //Manually set the y position higher to avoid plane clipping into ground
        Vector3 position = movementControllerScript.tranformObjectsArr[5].transform.position;
        position = new Vector3(position.x, position.y + 10f, position.z);
    }
    public void OnClickTransformScooter()
    {
        ChangeToObject("Scooter", DisableCurrentActive());
        GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);

        SetBackgroundImageAndDeselectAllOthers(3);
    }

    Transform DisableCurrentActive()
    {
        for (int i = 0; i < movementControllerScript.tranformObjectsArr.Length; i++)
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
                Vector3 newPos = new Vector3(movementControllerScript.startingPosition.position.x, currentPos.position.y + yOffset, currentPos.position.z);
                movementControllerScript.tranformObjectsArr[i].transform.position = newPos;
                movementControllerScript.tranformObjectsArr[i].transform.rotation = resetRotation;
                movementControllerScript.tranformObjectsArr[i].SetActive(true);

                //Set Particle as child and play
                StartCoroutine(particleManagerScript.PlayTransformParticle(movementControllerScript.tranformObjectsArr[i].transform, particleManagerScript.transformParticlePlayer));
                break;
            }
        }
    }
    void ChangeToObject(string transformToName, Transform currentPos, string plane)
    {
        for (int i = 0; i < movementControllerScript.tranformObjectsArr.Length; i++)
        {
            if (movementControllerScript.tranformObjectsArr[i].name == transformToName)
            {
                //movementControllerScript.tranformObjectsArr[i].transform.position = currentPos.position;
                Vector3 newPos = new Vector3(movementControllerScript.startingPosition.position.x, currentPos.position.y + airplaneYOffset, currentPos.position.z);
                movementControllerScript.tranformObjectsArr[i].transform.position = newPos;
                movementControllerScript.tranformObjectsArr[i].transform.rotation = resetRotation;
                Debug.Log("Change To Object Method");
                movementControllerScript.tranformObjectsArr[i].SetActive(true);

                //Set Particle as child and play
                StartCoroutine(particleManagerScript.PlayTransformParticle(movementControllerScript.tranformObjectsArr[i].transform, particleManagerScript.transformParticlePlayer));
                break;
            }
        }
    }

    void SetBackgroundImageAndDeselectAllOthers(int index)
    {

        for (int i = 0; i < buttonArray.Length; i++)
        {
            if (i == index)
            {
                buttonArray[i].sprite = currentSelectedImage;
                buttonArray[i].gameObject.transform.localScale = selectedScale;
            }
            else
            {
                buttonArray[i].sprite = deselectedImage;
                buttonArray[i].gameObject.transform.localScale = new Vector3(1, 1, 1);
            }

        }
    }

    void SetCurrentActiveVehicleSpriteImage()
    {
        for (int i = 0; i < movementControllerScript.tranformObjectsArr.Length; i++)
        {
            if (movementControllerScript.tranformObjectsArr[i].gameObject.activeSelf)
            {
                buttonArray[i].sprite = currentSelectedImage;
                buttonArray[i].gameObject.transform.localScale = selectedScale;
                
            }
            else
            {
                buttonArray[i].sprite = deselectedImage;
                buttonArray[i].gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
