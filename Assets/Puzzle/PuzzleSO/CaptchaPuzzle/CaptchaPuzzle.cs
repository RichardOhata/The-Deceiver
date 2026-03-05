using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class CaptchaPuzzle : Puzzle
{
    
    [SerializeField] private GameObject captchaUI;
    [SerializeField] private GameObject captchaConsole;
    [SerializeField] private int stage = 1;
    [SerializeField] private const int lastStage = 4;
    [SerializeField] private GameObject interactTrigger;
    [SerializeField] private GameObject hiddenUIText;

    [SerializeField] private GameObject puzzleText;
    private GameObject player;

    [SerializeField] private GameObject slidingDoor;
    [SerializeField] private GameObject monitor;

    private void Awake()
    {
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
            slidingDoor.GetComponent<Animator>().Play("OpenSlidingDoor", 0, 1f);
            interactTrigger.SetActive(false);
            monitor.GetComponent<ExplodableMonitor>().ExplodeMonitor();
            puzzleText.SetActive(false);
            hiddenUIText.SetActive(true);
        }
    }

    public override void StartPuzzle()
    {
        base.StartPuzzle();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        interactTrigger.SetActive(false);
        OnDisable();
    }

    private void OnEnable()
    {
        InputManager.Instance.controls.Player.Interact.performed += ToggleCaptchaUI;
    }


    private void OnDisable()
    {
        InputManager.Instance.controls.Player.Interact.performed -= ToggleCaptchaUI;
    }

    public override void UpdatePuzzleStatus()
    {
        SaveManager.Instance.currentData.puzzleProgress.captcha.isSolved = isSolved;
        SaveManager.Instance.currentData.puzzleProgress.captcha.isDoorOpened = isSolved;
        SaveManager.Instance.currentData.puzzleProgress.captcha.isConsoleDestroyed = isSolved;

        SaveManager.Instance.currentData.puzzleProgress.captcha.stage = stage;
        SaveManager.Instance.SaveGame();
        Debug.Log("Checkpoint and Puzzle State Auto-Saved!");
    }

    private bool IsPlayerNearConsole()
    {
          
        if (player == null) return false;

        float distance = Vector3.Distance(player.transform.position, captchaConsole.transform.position);
        return distance < 3f; // example: within 3 units
    }

    private void ToggleCaptchaUI(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        interactTrigger.GetComponent<UIUpdate>().updatePanelText("");
        if (captchaUI.gameObject.activeSelf)
        {
            return;
        }

        if (!captchaUI.gameObject.activeSelf && IsPlayerNearConsole())
        {
            captchaUI.SetActive(true);
            PauseMenuLogic.Instance.HandlePause(false);
        }
    }

    public override bool IsUIOpen => captchaUI.activeSelf;

    public override void CloseUI()
    {
        captchaUI.SetActive(false);
        PauseMenuLogic.Instance.HandlePause(false);
    }

    public void ProgressPuzzle()
    {
        stage++;
        UpdateCaptchaAnswer();
        UpdatePuzzleStatus();
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
                SolvePuzzle();
                CloseUI();
                monitor.GetComponent<ExplodableMonitor>().ExplodeMonitor();
                slidingDoor.GetComponent<Animator>().SetTrigger("OpenSlidingDoor");
                slidingDoor.GetComponentInChildren<AudioSource>().PlayDelayed(1.5f);
                return;
            default:
                break;
        }

        captchaUI.GetComponent<CaptchaUI>().UpdateProgressText(stage, lastStage - 1);
    }

}
