using UnityEngine;

public class CharacterSfxManager : MonoBehaviour
{
    private AudioSource audioSource;
    
    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRollSfx()
    {
        // audioSource.PlayOneShot(WorldSfxManager.Instance.rollSfx);
    }
}
