using System;
using UnityEngine;

public class WorldSfxManager : MonoBehaviour
{
    public static WorldSfxManager Instance;

    [Header("Action Sounds")]
    public AudioClip rollSfx;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
