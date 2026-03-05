using UnityEngine;
using System.IO;

[DefaultExecutionOrder(-100)]
public class SaveManager : MonoBehaviour
{
    private string saveFilePath;

    public static SaveManager Instance;

    public SaveData currentData = new SaveData();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        saveFilePath = Application.persistentDataPath + "/SaveData.json";

        LoadGame();
    }


    public void SaveGame()
    {
        string json = JsonUtility.ToJson(currentData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game successfully saved to: " + saveFilePath);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            currentData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game successfully loaded from: " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("No save file found! Creating new save data.");
            currentData = new SaveData();
        }
    }
}
