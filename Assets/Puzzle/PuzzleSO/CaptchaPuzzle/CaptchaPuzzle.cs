using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class CaptchaPuzzle : Puzzle
{
    [Header("Puzzle Components")]
    [SerializeField] private int stage = 1;
    [SerializeField] private const int lastStage = 4;
    [SerializeField] private GameObject hiddenUIText;
    [SerializeField] private GameObject puzzleText;
    [SerializeField] private GameObject monitor;
 
    [Header("UI References")]
    [SerializeField] private GameObject captchaUI;
    [SerializeField] private GameObject backgroundDimmer;
    [SerializeField] private UIUpdate uiPrompt;


    [SerializeField] private GameObject slidingDoor;
    protected override void Awake()
    {
        base.Awake();
        if (SaveManager.Instance != null)
        {
            isSolved = SaveManager.Instance.currentData.puzzleProgress.captcha.isSolved;
            stage = SaveManager.Instance.currentData.puzzleProgress.captcha.stage;
            UpdateCaptchaAnswer();
        }
    }

    private void Start()
    {
        if (isSolved)
        {
            Animator anim = slidingDoor.GetComponent<Animator>();
            anim.SetTrigger("Door_Open");
            anim.speed = 100f;
            monitor.GetComponent<ExplodableMonitor>().ExplodeMonitor();
            puzzleText.SetActive(false);
            hiddenUIText.SetActive(true);
            OnDisable();
        }
    }
    private void Update()
    {
        if (isSolved) return;

    
        if (CanInteractWithConsole() && !IsUIOpen)
        {
            uiPrompt.updatePanelText(); 
        }
        else
        {
            uiPrompt.DisablePanel();
        }
    }

    public override void StartPuzzle()
    {
        base.StartPuzzle();
    }

    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        OnDisable();
    }

    private void OnEnable()
    {
        if (isSolved) return;
        InputManager.Instance.controls.Player.Interact.performed += ToggleCaptchaUI;
    }


    private void OnDisable()
    {
        InputManager.Instance.controls.Player.Interact.performed -= ToggleCaptchaUI;
    }

    public override void UpdatePuzzleStatus()
    {
        SaveManager.Instance.currentData.puzzleProgress.captcha.isSolved = isSolved;
        SaveManager.Instance.SaveGame();
        Debug.Log("Checkpoint and Puzzle State Auto-Saved!");
    }

    private bool CanInteractWithConsole()
    {
        if (base.player == null) return false;
       
        return LookAtUtility.IsPointedAt(base.playerCamera, monitor, 0.5f, 3.5f);
    }

    private void ToggleCaptchaUI(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    
        if (!captchaUI.activeSelf && CanInteractWithConsole())
        {
            captchaUI.gameObject.SetActive(true);
            backgroundDimmer.SetActive(true);
            PauseMenuLogic.Instance.HandlePause(false); 
        }
    }

    public override bool IsUIOpen => captchaUI.activeSelf;

    public override void CloseUI()
    {
        captchaUI.SetActive(false);
        backgroundDimmer.SetActive(false);
        PauseMenuLogic.Instance.HandlePause(false);
    }

    public void ProgressPuzzle()
    {
        stage++;
        UpdateCaptchaAnswer();
        SaveManager.Instance.currentData.puzzleProgress.captcha.stage = stage;
        captchaUI.GetComponent<CaptchaUI>().ResetInputField();
        captchaUI.GetComponent<CaptchaUI>().GenerateCaptcha();
    }

    private void UpdateCaptchaAnswer()
    {
        switch (stage)
        {
            case 2:
                captchaUI.GetComponent<CaptchaUI>().SetExtraChar('!');
                puzzleText.GetComponent<TextMeshPro>().text = "Solve the Captcha!";
                break;
            case 3:
                captchaUI.GetComponent<CaptchaUI>().SetExtraChar('.');
                puzzleText.SetActive(false);
                hiddenUIText.SetActive(true);
                break;

            case lastStage:
                if (isSolved) return;
                SolvePuzzle();
                CloseUI();
                monitor.GetComponent<ExplodableMonitor>().ExplodeMonitor();
                slidingDoor.GetComponent<Animator>().SetTrigger("Door_Open");
               
                return;
            default:
                break;
        }

        captchaUI.GetComponent<CaptchaUI>().UpdateProgressText(stage, lastStage - 1);
    }

}
