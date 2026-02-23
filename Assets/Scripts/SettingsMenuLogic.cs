using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuLogic : MonoBehaviour
{
    [SerializeField]
    private Slider FOVSlider;

    [SerializeField]
    private Slider sensitivitySlider;

    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private Toggle reticleToggle;

    [SerializeField]
    private Slider smoothingSlider;

    [SerializeField]
    private TMP_Dropdown displayModeDropDown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FOVSlider.value = SettingsManager.Instance.fov;
        sensitivitySlider.value = SettingsManager.Instance.sensitivity;
        volumeSlider.value = SettingsManager.Instance.volume;
        reticleToggle.isOn = (SettingsManager.Instance.reticle == 1 ? true : false);
        smoothingSlider.value = SettingsManager.Instance.smoothing;
        displayModeDropDown.value = SettingsManager.Instance.displayModeValue;
    }


    public void UpdateSettingValues(string type)
    {
        switch (type)
        {
            case "FOV":
                SettingsManager.Instance.SetFOV(FOVSlider.value);
                break;

            case "Sensitivity":
                SettingsManager.Instance.SetSensitivity(sensitivitySlider.value);
                break;
            case "Volume":
                SettingsManager.Instance.SetVolume(volumeSlider.value);
                break;
            case "Reticle":
                SettingsManager.Instance.ToggleReticle(reticleToggle.isOn);
                break;
            case "Smoothing":
                SettingsManager.Instance.SetSmoothing(smoothingSlider.value);
                break;
            case "Dropdown":
                SettingsManager.Instance.SetDisplayMode(displayModeDropDown.value);
                break;
        }
    }

}
