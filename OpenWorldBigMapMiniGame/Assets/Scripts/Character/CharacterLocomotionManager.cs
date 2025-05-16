using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Ground Check")]
    [SerializeField] protected float gravity = -5.55f;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float groundCheckDistance = 0.2f;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float groundedYVelocity = -20f;
    [SerializeField] protected float fallStartYVelocity = -5f;
    protected bool fallingVelocitySet = false;
    protected float inAirTimer = 0;

    protected virtual void Awake() {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update() {
        HandleGroundCheck();

        if (character.isGrounded)
        {
            // if not attemping to jump or move upward
            if(yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocitySet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            if(!character.isJumping && !fallingVelocitySet)
            {
                fallingVelocitySet = true;
                yVelocity.y = fallStartYVelocity;
            }
            inAirTimer += Time.deltaTime;
            character.animator.SetFloat("InAirTimer", inAirTimer);
            yVelocity.y += gravity * Time.deltaTime;
        }
        character.characterController.Move(yVelocity * Time.deltaTime);
       
    }

    protected void HandleGroundCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckDistance, groundLayer);
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(character.transform.position, groundCheckDistance);
    }
}