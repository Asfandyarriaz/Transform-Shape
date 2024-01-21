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
    [SerializeField] LevelManager levelManagerScript;
    MovementController movementControllerScript;

    [Header("Buttons")]
    [SerializeField] Image[] buttonArray;                //0.Character 1.Car 2.Tank 3.Scooter 4.Boat 5.Airplane
    [SerializeField] Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1.2f);

    [Header("Images")]
    [SerializeField] Sprite currentSelectedImage;
    [SerializeField] Sprite deselectedImage;

    [Header("Vehicle Selector")]
    [SerializeField] RectTransform vehicleSelectorObject;
    [SerializeField] float twoButtonsWidth;   //Width = 300, Height = 200
    [SerializeField] float threeButtonsWidth; //Width = 410, Height = 200
    [SerializeField] float fourButtonsWidth;  //Width = 500, Height = 200


    //Variables 
    [Header("Variables")]
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
            SetAllImagesToDefault();   
        }

        if(state == GameManager.GameState.SetupGameData)
        {
            SetCurrentActiveVehicleSpriteImage();
            SetTransformRectForVehicleSelector();
        }
    }

    private void Start()
    {
        movementControllerScript = GetComponent<MovementController>();
    }

    //TODO: Fix Hardcoded Values
    public void OnClickTransformCharacterWalk()
    {
        ChangeToObject("Character Walk", DisableCurrentActive()); //Set name same as object under TransformList object in hierarchy
        movementControllerScript.hasVehicleChanged = true;
        //GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);

        SetBackgroundImageAndDeselectAllOthers(0);
    }

    public void OnClickTransformCar()
    {
        ChangeToObject("Car", DisableCurrentActive());
        movementControllerScript.hasVehicleChanged = true;
        //GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);

        SetBackgroundImageAndDeselectAllOthers(1);
    }
    public void OnClickTransformTank()
    {
        ChangeToObject("Tank", DisableCurrentActive());
        movementControllerScript.hasVehicleChanged = true;
        //GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);

        SetBackgroundImageAndDeselectAllOthers(2);
    }
    public void OnClickTransformScooter()
    {
        ChangeToObject("Scooter", DisableCurrentActive());
        movementControllerScript.hasVehicleChanged = true;
        //GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);

        SetBackgroundImageAndDeselectAllOthers(3);
    }
    public void OnClickTransformBoat()
    {
        ChangeToObject("Boat", DisableCurrentActive());
        movementControllerScript.hasVehicleChanged = true;
        //GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);

        SetBackgroundImageAndDeselectAllOthers(4);
    }
    public void OnClickTransformPlane()
    {
        ChangeToObject("Airplane", DisableCurrentActive());
        ChangeToObject("Airplane", DisableCurrentActive(), "Ref Override");
        movementControllerScript.hasVehicleChanged = true;
        //GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);

        SetBackgroundImageAndDeselectAllOthers(5);

        //Manually set the y position higher to avoid plane clipping into ground
        Vector3 position = movementControllerScript.tranformObjectsArr[5].transform.position;
        position = new Vector3(position.x, position.y + 10f, position.z);
    }
    public void OnClickTransformGlider()
    {
        ChangeToObject("Glider", DisableCurrentActive());
        movementControllerScript.hasVehicleChanged = true;
        //GameManager.Instance.UpdateGameState(GameManager.GameState.Transform);
        //Play Audio
        AudioManager.Instance.PlaySFX(AudioManager.Instance.onButtonClick);

        SetBackgroundImageAndDeselectAllOthers(6);
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

                //Set Player Marker
                //particleManagerScript.FollowPlayerMarker(movementControllerScript.tranformObjectsArr[i].transform);
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
                movementControllerScript.tranformObjectsArr[i].SetActive(true);

                //Set Particle as child and play
                StartCoroutine(particleManagerScript.PlayTransformParticle(movementControllerScript.tranformObjectsArr[i].transform, particleManagerScript.transformParticlePlayer));
                break;
            }
        }
    }

    //TODO: Optimize get component
    void SetBackgroundImageAndDeselectAllOthers(int index)
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            if (i == index)
            {
                buttonArray[i].sprite = currentSelectedImage;
                buttonArray[i].gameObject.transform.localScale = selectedScale;

                //Set Interactable off 
                Button button = buttonArray[i].gameObject.GetComponent<Button>();
                button.interactable = false;


            }
            else
            {
                buttonArray[i].sprite = deselectedImage;
                buttonArray[i].gameObject.transform.localScale = new Vector3(1, 1, 1);

                //Set Interactable on 
                Button button = buttonArray[i].gameObject.GetComponent<Button>();
                button.interactable = true;
            }
        }
    }

    void SetAllImagesToDefault()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i].sprite = deselectedImage;
            buttonArray[i].gameObject.transform.localScale = new Vector3(1, 1, 1);

            //Set Interactable on 
            Button button = buttonArray[i].gameObject.GetComponent<Button>();
            button.interactable = true;
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

    void SetTransformRectForVehicleSelector()
    {
        int activeButtons = 0;
        Vector2 sizeDelta = vehicleSelectorObject.sizeDelta;

        activeButtons = levelManagerScript.levelProperties[levelManagerScript.Int_GetCurrentActiveLevel()].numberOfShapes;

        if (activeButtons == 2)
        {
            sizeDelta.x = twoButtonsWidth;
        }
        else if(activeButtons == 3)
        {
            sizeDelta.x = threeButtonsWidth;
        }
        else if(activeButtons == 4)
        {
            sizeDelta.x = fourButtonsWidth;
        }

        vehicleSelectorObject.sizeDelta = sizeDelta;
    }
}
