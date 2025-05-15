using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;

    protected override void Awake()
    {
        base.Awake();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (!IsOwner)
            return;
         
        playerLocomotionManager.HandleAllMovement();
        playerStatsManager.RegenerateStamina();
    }

    protected override void LateUpdate()
    {
        if (!IsOwner)
            return;

        base.LateUpdate();

        PlayerCamera.Instance.HandleAllCameraActions();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // The player object by this client
        if (IsOwner)
        {
            PlayerCamera.Instance.player = this;
            PlayerInputManager.Instance.player = this;
            
            playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.Instance.playerUIHudManager.SetNewStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenerationTimer;
            var maxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            playerNetworkManager.maxStamina.Value = maxStamina;
            playerNetworkManager.currentStamina.Value = maxStamina;
            PlayerUIManager.Instance.playerUIHudManager.SetNewMaxStaminaValue(maxStamina);


        }
    }

    // public void SaveGameData(ref CharacterSaveData characterData)
    // {
    //     characterData.characterName = playerNetworkManager.characterName.Value.ToString();
    //     characterData.xPosition = transform.position.x;
    //     characterData.yPosition = transform.position.y;
    //     characterData.zPosition = transform.position.z;
    // }

    // public void LoadGameData(ref CharacterSaveData characterData)
    // {
    //     playerNetworkManager.characterName.Value = characterData.characterName;
    //     transform.position = new Vector3(characterData.xPosition, characterData.yPosition, characterData.zPosition);
    // }
}
