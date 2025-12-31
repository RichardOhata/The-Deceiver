using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject FOVSlider;

    [SerializeField]
    private GameObject sensitivitySlider;

    [SerializeField]
    private GameObject volumeSlider;

    [SerializeField]
    private GameObject reticleToggle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FOVSlider.GetComponent<Slider>().value = SettingsManager.Instance.fov;
        sensitivitySlider.GetComponent<Slider>().value = SettingsManager.Instance.sensitivity;
        volumeSlider.GetComponent<Slider>().value = SettingsManager.Instance.volume;
    }


    public void UpdateSettingValues(string type)
    {
        switch (type)
        {
            case "FOV":
                SettingsManager.Instance.SetFOV(FOVSlider.GetComponent<Slider>().value);
                break;

            case "Sensitivity":
                SettingsManager.Instance.SetSensitivity(sensitivitySlider.GetComponent<Slider>().value);
                break;
            case "Volume":
                SettingsManager.Instance.SetVolume(volumeSlider.GetComponent<Slider>().value);
                break;
            case "Reticle":
                SettingsManager.Instance.ToggleReticle(reticleToggle.GetComponent<Toggle>().isOn);
                break;
        }
    }

}
