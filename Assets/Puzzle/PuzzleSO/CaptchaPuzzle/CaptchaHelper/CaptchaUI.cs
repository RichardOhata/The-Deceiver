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
    [SerializeField] private Button uiCloseButton;

    [SerializeField] private TextMeshProUGUI uiProgressText;

    [Header("Captcha Generator: ")]
    [SerializeField] private CaptchaGenerator captchaGenerator;

    [Header("Audio References: ")]
    [SerializeField] private AudioSource uiSuccess;
    [SerializeField] private AudioSource uiError;


    private Captcha currentCaptcha;

    [SerializeField] private char extraChar;
    private void Start()
    {
        GenerateCaptcha();
        uiRefreshButton.onClick.AddListener(GenerateCaptcha);
        uiSubmitButton.onClick.AddListener(Submit);
        uiCloseButton.onClick.AddListener(Close);
    }

    public void GenerateCaptcha()
    {
        uiSubmitButton.interactable = true;
        currentCaptcha = captchaGenerator.Generate();

        uiCodeImage.sprite = currentCaptcha.Image;
        uiErrorText.gameObject.SetActive(false);
    }

    private void Submit()
    {
        string enteredCode = uiCodeInput.text.Trim();
        uiErrorText.gameObject.SetActive(false);
        uiCorrectText.gameObject.SetActive(false);

        bool isValid;

        // Handle both cases (with or without extraChar)
        if (extraChar == '\0')
            isValid = captchaGenerator.IsCodeValid(enteredCode, currentCaptcha);
        else
            isValid = captchaGenerator.IsCodeValid(enteredCode, currentCaptcha, extraChar);

        if (isValid)
        {
            uiSubmitButton.interactable = false;
            uiSuccess.ignoreListenerPause = true;
            uiSuccess.Play();
            uiCorrectText.gameObject.SetActive(true);

            StartCoroutine(DelayedAction(1.0f, () =>
            {
                GameObject.FindGameObjectWithTag("Captcha Puzzle")
                    .GetComponent<CaptchaPuzzle>()
                    .ProgressPuzzle();

                uiCorrectText.gameObject.SetActive(false);
            }));
        }
        else
        {
            uiErrorText.gameObject.SetActive(true);
            uiError.Play();
            uiError.ignoreListenerPause = true;
        }
    }

    public void SetExtraChar(char newChar)
    {
        extraChar = newChar;
    }

    public void UpdateProgressText(int num1, int num2)
    {
        uiProgressText.text = num1 + "/" + num2;
    }

    private System.Collections.IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action?.Invoke();
    }

    public void ResetInputField()
    {
        uiCodeInput.text = "";
    }

    private void Close()
    {
        GameObject.FindGameObjectWithTag("Captcha Puzzle").GetComponent<CaptchaPuzzle>().CloseUI();
    }
} 
