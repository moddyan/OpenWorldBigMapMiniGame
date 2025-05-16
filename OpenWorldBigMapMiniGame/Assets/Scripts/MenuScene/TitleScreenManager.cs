using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public void StartNewGame()
    {
        // WorldSaveGameManager.Instance.NameGame();
        WorldSaveGameManager.Instance.LoadWorldScene();
    }

}
