using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    string savePath;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        savePath = Application.persistentDataPath + "/TransformShape.fun";
    }
    public void SaveData(PlayerData data)
    {
        if (CheckSaveData(data))
        {
            string jsonData = JsonUtility.ToJson(data);
            File.WriteAllText(savePath, jsonData);
        }
        else
        {
            Debug.LogWarning("No game data to save");
        }
    }

    public PlayerData LoadData()
    {
        if(File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);
            return JsonUtility.FromJson<PlayerData>(jsonData);
        }
        else
        {
            Debug.LogWarning("No save file found.");
            return null;
        }
    }

    //Check if any field is not empty 
    //TODO: No checks for totalCash variable
    bool CheckSaveData(PlayerData data)
    {
        if(data.GetCharacterLevel() == 0)
        {
            return false;
        }
        if (data.GetCarLevel() == 0)
        {
            return false;
        }
        if (data.GetTankLevel() == 0)
        {
            return false;
        }
        if (data.GetBoatLevel() == 0)
        {
            return false;
        }
        if (data.GetAirplaneLevel() == 0)
        {
            return false;
        }
        if (data.GetScooterLevel() == 0)
        {
            return false;
        }
        return true;
    }
}
