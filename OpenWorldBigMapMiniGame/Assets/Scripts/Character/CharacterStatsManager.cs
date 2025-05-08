using UnityEngine;
using UnityEngine.UIElements;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Stamina")]
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    [SerializeField] private float staminaRegenerationAmount = 2;
    [SerializeField] float staminaRegenerationDelay = 2;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public int CalculateStaminaBasedOnEnduranceLevel(int enduranceLevel)
    {
        float stamina = 0;

        stamina = enduranceLevel * 10;

        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegenerateStamina()
    {
        if (!character.IsOwner)
            return;
        if (character.characterNetworkManager.isSprinting.Value)
            return;
        if (character.isPerformingAction)
            return;

        staminaRegenerationTimer += Time.deltaTime;
        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;
                if (staminaTickTimer >= 0.1f)
                {
                    character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                    staminaTickTimer = 0;
                }
            }
        }
    }

    public virtual void ResetStaminaRegenerationTimer(float prevStamina, float newStamina)
    {
        if (newStamina < prevStamina)
        {
            staminaRegenerationTimer = 0;
        }
    }
}
