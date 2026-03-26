using UnityEngine;
using System.IO;
using System.Collections;
using System.Diagnostics;

[DefaultExecutionOrder(-100)]
public class SaveManager : MonoBehaviour
{
    private string saveFilePath;
    public static SaveManager Instance;
    public SaveData currentData = new SaveData();


    [Header("UI Reference")]
    public GameObject saveIcon;
    public float extraPaddingTime = 0.5f;

    public bool HasSaveFile => File.Exists(Application.persistentDataPath + "/SaveData.json");

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        saveFilePath = Application.persistentDataPath + "/SaveData.json";
        if (saveIcon != null) saveIcon.SetActive(false);
        LoadGame();
    }


    public void SaveGame()
    {
        StartCoroutine(DynamicSaveSequence());
    }

    private IEnumerator DynamicSaveSequence()
    {
     
        Stopwatch timer = new Stopwatch();
        timer.Start();

        if (saveIcon != null) saveIcon.SetActive(true);

   
        string json = JsonUtility.ToJson(currentData, true);
        File.WriteAllText(saveFilePath, json);

       
        timer.Stop();
        float saveDurationSeconds = (float)timer.Elapsed.TotalSeconds;
        float totalDisplayTime = saveDurationSeconds + extraPaddingTime;

        UnityEngine.Debug.Log($"Save took {saveDurationSeconds:F4}s. Showing icon for {totalDisplayTime:F4}s.");

   
        yield return new WaitForSeconds(totalDisplayTime);

        if (saveIcon != null) saveIcon.SetActive(false);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            currentData = JsonUtility.FromJson<SaveData>(json);
            UnityEngine.Debug.Log("Game successfully loaded from: " + saveFilePath);
        }
        else
        {
            UnityEngine.Debug.LogWarning("No save file found! Creating new save data.");
            currentData = new SaveData();
        }
    }

    [ContextMenu("Reset Save Data to Default")]
    public void ResetToDefault()
    {
        currentData = new SaveData();
        SaveGame();
        UnityEngine.Debug.Log("All save data has been reset to defaults via Inspector.");
    }
}
