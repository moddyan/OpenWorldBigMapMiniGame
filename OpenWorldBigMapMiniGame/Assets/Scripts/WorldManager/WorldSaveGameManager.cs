using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance;
    [SerializeField] private PlayerManager player;

    // [Header("Save/Load")]
    // [SerializeField] private bool saveGame;
    // [SerializeField] private bool loadGame;

    [Header("World Scene Index")]
    [SerializeField] private int worldSceneIndex = 1;

    [Header("Save File Data Writer")]
    private SaveFileDataWriter saveFileDataWriter;


    public CharacterSaveData currentCharacterData;
    private string saveFileName;

    [Header("Character Slots")]
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;


    public int WorldSceneIndex => worldSceneIndex;
    
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

    private void Update()
    {
       
    }



    // public void NameGame()
    // {
    //     saveFileName = DecideCharacterFileNameBasedOnSlot(currentCharacterSlot);
    //     currentCharacterData = new CharacterSaveData();
    // }

    // public void LoadGame()
    // {
    //     saveFileName = DecideCharacterFileNameBasedOnSlot(currentCharacterSlot);
    //     saveFileDataWriter = new SaveFileDataWriter();
    //     saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
    //     saveFileDataWriter.saveFileName = saveFileName;
    //     currentCharacterData = saveFileDataWriter.LoadSaveFile();

    //     player.LoadGameData(ref currentCharacterData);

    //     StartCoroutine(LoadWorldScene());
    // }

    // public void SaveGame()
    // {
    //     saveFileName = DecideCharacterFileNameBasedOnSlot(currentCharacterSlot);
    //     saveFileDataWriter = new SaveFileDataWriter();
    //     saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
    //     saveFileDataWriter.saveFileName = saveFileName;

    //     player.SaveGameData(ref currentCharacterData);

    //     saveFileDataWriter.CreateNewSaveFile(currentCharacterData);
    // }

    public async void LoadWorldScene()
    {
        var loadOperator = SceneManager.LoadSceneAsync(worldSceneIndex);
        await loadOperator;
        NetworkManager.Singleton.StartHost();
    }
}
