using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterManager : NetworkBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;
    
    [HideInInspector] public CharacterNetworkManager characterNetworkManager;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool isJumping = false;
    public bool isGrounded = true;

    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;




    protected virtual void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
    }

    protected virtual void Update()
    {
        animator.SetBool("IsGrounded", isGrounded);

        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, characterNetworkManager.networkPosition.Value,
                ref characterNetworkManager.networkPositionVelocity, characterNetworkManager.networkPositionSmoothTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, characterNetworkManager.networkRotation.Value,
                characterNetworkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate()
    {
        
    }


}
