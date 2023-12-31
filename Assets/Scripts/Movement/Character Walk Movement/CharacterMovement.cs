using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour, IInterfaceMovement
{
    [SerializeField] VehicleProperties vehicleProperties;
    CharacterController characterController;
    public bool allowMove;
    public bool isOnGround;
    public bool isClimbable;

    [Header("Speed")]
    [SerializeField] private float speed;
    [SerializeField] private float climbingSpeed;

    [Header("Speed Increment")]
    [SerializeField] private float incrementSpeedPercentage = 5f;


    //Animator
    private Animator animator;
    //Flags
    private bool runOnce = false;
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
        if (state == GameManager.GameState.Start)
        {
            runOnce = false;
        }
        if (state == GameManager.GameState.Play)
        {
            
            IncrementSpeed();
        }
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        speed = vehicleProperties.speed;
    }

    public void Movement()
    {
        if (allowMove)
        {
            if (isClimbable)
            {
                characterController.Move(Vector3.up * (speed - climbingSpeed) * Time.deltaTime);
            }
            else
            {
                characterController.Move(Vector3.forward * speed * Time.deltaTime);
            }
        }
        else
        {
            characterController.Move(Vector3.down * vehicleProperties.speed * Time.deltaTime);
        }
        if(!isOnGround && !isClimbable) { characterController.Move(Vector3.down * vehicleProperties.speed * Time.deltaTime); }

        AnimationController();
    }

    //5 % Increment with each level
    void IncrementSpeed()
    {
        
        if (vehicleProperties.currentUpgradeLevel >= 1 && runOnce != true)
        {
            speed = vehicleProperties.speed;
            incrementSpeedPercentage = incrementSpeedPercentage * vehicleProperties.currentUpgradeLevel -1;
            speed += Mathf.RoundToInt(vehicleProperties.speed * (incrementSpeedPercentage / 100));
            runOnce = true;
        }
    }
    void AnimationController()
    {
        if(isClimbable)
        {
            
            animator.SetBool("b_IsClimbing", true);
            animator.SetBool("b_IsRunning", false);
        }
        else
        {
            animator.SetBool("b_IsRunning", true);
            animator.SetBool("b_IsClimbing", false); 
        }
    }
}
