using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;

    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement Settings")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float sprintingSpeed = 6.5f;
    [SerializeField] float rotationSpeed = 15;
    [SerializeField] float sprintStaminaCost = 2;
    [SerializeField] float dodgeStaminaCost = 25;
    [SerializeField] float jumpStaminaCost = 25;

    private Vector3 rollDirection;


    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();
        if (player.IsOwner)
        {
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;
            
            // if not locked on, pass move amount
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, 
                player.playerNetworkManager.isSprinting.Value);
            
            // if locked on, pass horizontal and vertical movement
            
        }
    }

    public void HandleAllMovement()
    {
        // GROUNDED MOVEMENT
        HandleGroundedMovement();
        HandleRotation();

        // AERIAL MOVEMENT
    }

    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.Instance.verticalInput;
        horizontalMovement = PlayerInputManager.Instance.horizontalInput;
        moveAmount = PlayerInputManager.Instance.moveAmount;
        
        //CLAMP THE MOVEMENTS
    }

    void HandleGroundedMovement()
    {
        if (!player.canMove)
        {
            return;
        }
        GetMovementValues();
        moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
        moveDirection += PlayerCamera.Instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (player.playerNetworkManager.isSprinting.Value)
        {
            Debug.Log("Sprinting");
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else
        {
            if (PlayerInputManager.Instance.moveAmount > 0.5f)
            {
                // run
                Debug.Log("Running");
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
            {
                // walk
                Debug.Log("Walking");
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }
        
       
    }

    private void HandleRotation()
    {
        if (!player.canRotate)
        {
            return;
        }
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDirection += PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    public void HandleSprinting()
    {
        if (player.isPerformingAction)
        {
            player.playerNetworkManager.isSprinting.Value = false;
            return;
        }
        
        // if out of stamina, stop sprinting
        if (player.playerNetworkManager.currentStamina.Value <= 0)
        {
            player.playerNetworkManager.isSprinting.Value = false;
            return;
        }

        if (moveAmount >= 0.5f)
        {
            player.playerNetworkManager.isSprinting.Value = true;
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }

        // if sprinting, consume stamina
        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.playerNetworkManager.currentStamina.Value -= sprintStaminaCost * Time.deltaTime;
        }
   
        
    }
    
    public void AttemptToPerformDodge()
    {
        if (player.isPerformingAction)
        {
            return;
        }
        if (player.playerNetworkManager.currentStamina.Value <= 0)
        {
            return;
        }
        
        if (moveAmount > 0)
        {
            // perform a roll if we are moving
            rollDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
            rollDirection += PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
            rollDirection.y = 0;
            rollDirection.Normalize();
        
            var playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;
            
            player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);
        }
        else
        {
            // perform a backstep if we are standing still
            player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true);
        }

        // if dodging, consume stamina
        player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
        
    }

    public void AttemptToPerformJump()
    {
        if (player.isPerformingAction)
        {
            return;
        }
        if (player.playerNetworkManager.currentStamina.Value <= 0)
        {
            return;
        }

        if (player.isJumping)
        {
            return;
        }
        if (!player.isGrounded)
        {
            return;
        }

        // todo if two handed, play two handed jump animation

        player.playerAnimatorManager.PlayTargetActionAnimation("main_jump_01", false);
        player.isJumping = true;

        // if jumping, consume stamina
        player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;
        
    }

    public void ApplyJumpingVelocity()
    {
        // player.characterController.Move(Vector3.up * jumpForce * Time.deltaTime);
    }

}
