using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject quitButton;

    [SerializeField] private GameObject settingsUI;

    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject backButton;

    private void Start()
    {
        if (SaveManager.Instance.HasSaveFile)
        {
            continueButton.GetComponent<CutoutButtonHover>().SetDisabledState(false);
        }
        else
        {
            continueButton.GetComponent<CutoutButtonHover>().SetDisabledState(true);
        }
    }
    public void SettingUIOpen()
    {
        settingsUI.SetActive(true);
        ButtonVisibilityStatus(false);
    }

    public void SettingsUIClose()
    {
        settingsUI.SetActive(false);
        ButtonVisibilityStatus(true);
    }


    public void ButtonVisibilityStatus(bool setActive)
    {
        startButton.SetActive(setActive);
        settingsButton.SetActive(setActive);
        quitButton.SetActive(setActive);
    }

    public void SecondaryButtonVisibilityStatus(bool setActive)
    {
        newGameButton.SetActive(setActive);
        continueButton.SetActive(setActive);
        backButton.SetActive(setActive);
    }
}
