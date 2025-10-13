using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaptchaUI : MonoBehaviour { 
    [Header("UI References: ")]

    [SerializeField] private Image uiCodeImage;
    [SerializeField] private TMP_InputField uiCodeInput;
    [SerializeField] private TextMeshProUGUI uiErrorText;
    [SerializeField] private TextMeshProUGUI uiCorrectText;
    [SerializeField] private Button uiRefreshButton;
    [SerializeField] private Button uiSubmitButton;

    [Header("Captcha Generator: ")]
    [SerializeField] private CaptchaGenerator captchaGenerator;

    private Captcha currentCaptcha;

    private void Start()
    {
        GenerateCaptcha();
        uiRefreshButton.onClick.AddListener(GenerateCaptcha);
        uiSubmitButton.onClick.AddListener(Submit);
    }

    private void GenerateCaptcha()
    {
        currentCaptcha = captchaGenerator.Generate();

        uiCodeImage.sprite = currentCaptcha.Image;
        uiErrorText.gameObject.SetActive(false);
    }

    private void Submit()
    {
        string enteredCode = uiCodeInput.text;

        if (captchaGenerator.IsCodeValid(enteredCode, currentCaptcha))
        {
            uiErrorText.gameObject.SetActive(false);
            uiCorrectText.gameObject.SetActive(true);
        } else
        {
            uiErrorText.gameObject.SetActive(true);
        }
    }


} 
