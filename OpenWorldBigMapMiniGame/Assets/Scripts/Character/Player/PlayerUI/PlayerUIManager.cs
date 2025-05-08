using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;
    
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

        playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    [Header("NETWORK JOIN")] 
    [SerializeField]
    private bool startGameAsClient;

    [HideInInspector] public PlayerUIHudManager playerUIHudManager;

    private void Update()
    {
        if (startGameAsClient)
        {
            startGameAsClient = false;
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.StartClient();
        }


    }
}
