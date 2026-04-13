using System;
using UnityEngine;

public class PlaytimeTracker : MonoBehaviour
{
    public static PlaytimeTracker Instance { get; private set; }

    private float loadedPlayTime = 0f;
    private float currentSessionTime = 0f;

    // Add this little flag!
    private bool isQuitting = false;

    public float TotalPlayTime => loadedPlayTime + currentSessionTime;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.currentData != null)
        {
            loadedPlayTime = SaveManager.Instance.currentData.totalPlayTime;
        }
    }

    private void Update()
    {
        currentSessionTime += Time.deltaTime;
    }

    public void UpdateSaveData()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.currentData != null)
        {
            SaveManager.Instance.currentData.totalPlayTime = TotalPlayTime;
        }
    }

    public string GetFormattedPlayTime()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(TotalPlayTime);

        return string.Format("{0:D2}:{1:D2}:{2:D2}",
            (int)timeSpan.TotalHours,
            timeSpan.Minutes,
            timeSpan.Seconds);
    }

    private void OnApplicationQuit()
    {
     
        isQuitting = true;

        UpdateSaveData();

        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveGame();
            Debug.Log("Game closed: Playtime saved!");
        }
    }

    private void OnDestroy()
    {

        if (Instance == this && !isQuitting)
        {
            UpdateSaveData();
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.SaveGame();
                Debug.Log("Scene changed: Playtime saved!");
            }
        }
    }
}
