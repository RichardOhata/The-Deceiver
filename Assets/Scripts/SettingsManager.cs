using Unity.AppUI.UI;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [SerializeField]
    private GameObject reticleObject;

    [Header("Current Values")]
    public float sensitivity = 0.2f;
    public float fov = 60.0f;
    public float volume = 0.75f;
    public float smoothing = 2.0f;
    public int reticle = 0;
    public int displayModeValue = 0;
    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 

        LoadSettings(); 
    }

    public void SetSensitivity(float newVal)
    {
        sensitivity = newVal;
        PlayerPrefs.SetFloat("Sensitivity", newVal);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponentInChildren<FirstPersonLook>().sensitivity = newVal;
        }
    }

    public void SetFOV(float newVal)
    {
        fov = newVal;
        PlayerPrefs.SetFloat("FOV", newVal);

 
        if (Camera.main != null)
            Camera.main.fieldOfView = fov;
    }

    public void SetVolume(float newVal)
    {
        volume = newVal;
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", newVal);
    }

    public void ToggleReticle(bool isOn)
    {
        if (reticleObject != null)
        {
            reticleObject.SetActive(isOn);
            reticle = (isOn == true ? 1 : 0);
            PlayerPrefs.SetInt("Reticle", isOn ? 1 : 0);
        }
    }

    public void SetSmoothing(float newVal)
    {
        smoothing = newVal;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponentInChildren<FirstPersonLook>().smoothing = newVal;
        }
        PlayerPrefs.SetFloat("Smoothing", newVal);
    }

    public void SetDisplayMode(int value)
    {
        displayModeValue = value;
        PlayerPrefs.SetInt("DisplayMode", value);

        if (value == 0)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else if (value == 1)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    private void LoadSettings()
    {
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 0.2f);
        fov = PlayerPrefs.GetFloat("FOV", 60.0f);
        volume = PlayerPrefs.GetFloat("Volume", 0.75f);
        reticle = PlayerPrefs.GetInt("Reticle", 0);
        smoothing = PlayerPrefs.GetFloat("Smoothing", 2.0f);

        AudioListener.volume = volume;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            FirstPersonLook playerLookSettings = playerObj.GetComponentInChildren<FirstPersonLook>();
            if (playerLookSettings != null)
            {
                playerLookSettings.sensitivity = sensitivity;
                playerLookSettings.smoothing = smoothing;
            }
        }

        if (Camera.main != null)
            Camera.main.fieldOfView = fov;

        if (reticleObject != null)
        {
            reticleObject.SetActive(reticle == 1 ? true : false);
        }
    }
}
