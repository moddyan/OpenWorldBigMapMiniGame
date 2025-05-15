using UnityEngine;
using System.IO;
using System;
public class SaveFileDataWriter
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    public bool CheckToSeeIfFileExists()
    {
        if (string.IsNullOrEmpty(saveDataDirectoryPath) || string.IsNullOrEmpty(saveFileName))
        {
            return false;
        }

        string fullPath = Path.Combine(saveDataDirectoryPath, saveFileName);
        return File.Exists(fullPath);
    }

    public void DeleteSaveFile()
    {
        if (string.IsNullOrEmpty(saveDataDirectoryPath) || string.IsNullOrEmpty(saveFileName))
        {
            return;
        }

        string fullPath = Path.Combine(saveDataDirectoryPath, saveFileName);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public void CreateNewSaveFile(CharacterSaveData data)
    {
        if (string.IsNullOrEmpty(saveDataDirectoryPath) || string.IsNullOrEmpty(saveFileName))
        {
            return;
        }

        string fullPath = Path.Combine(saveDataDirectoryPath, saveFileName);
        try
        {
            Directory.CreateDirectory(saveDataDirectoryPath);
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(fullPath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to create save file: {e.Message}");
        }
    }

    public CharacterSaveData LoadSaveFile()
    {
        if (string.IsNullOrEmpty(saveDataDirectoryPath) || string.IsNullOrEmpty(saveFileName))
        {
            return null;
        }

        string fullPath = Path.Combine(saveDataDirectoryPath, saveFileName);
        if (!File.Exists(fullPath))
        {
            return null;
        }

        string json = File.ReadAllText(fullPath);
        var data = JsonUtility.FromJson<CharacterSaveData>(json);
        return data;
    }
}
