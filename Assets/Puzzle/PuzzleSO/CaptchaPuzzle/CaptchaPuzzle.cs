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

    public override void StartPuzzle()
    {
        base.StartPuzzle();
        player = GameObject.FindGameObjectWithTag("Player");
        captchaUI.GetComponent<CaptchaUI>().SetExtraChar('!');
    }

    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        puzzleData.isSolved = true;
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
                player.gameObject.GetComponentInChildren<FirstPersonLook>().enabled = false;
                player.gameObject.GetComponentInChildren<FirstPersonAudio>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
            }
            else
            {
                CloseCaptcha();
            }
 
    }

    public bool IsCaptchaOpen => captchaUI.activeSelf;

    public void CloseCaptcha()
    {
        captchaUI.SetActive(false);
        player.GetComponentInChildren<FirstPersonLook>().enabled = true;
        player.GetComponentInChildren<FirstPersonAudio>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void ProgressPuzzle()
    {
        stage++;
        switch (stage)
        {
            case 2:
                captchaUI.GetComponent<CaptchaUI>().SetExtraChar('.');
                puzzleText.GetComponent<TextMeshPro>().text = "Solve the 'Captcha.'";
                break;
            case 3:
                captchaUI.GetComponent<CaptchaUI>().SetExtraChar('\0');
                hiddenUIText.SetActive(true);
                break;

            case lastStage:
                SolvePuzzle();
                PauseMenuLogic.Instance.HandlePause();
                captchaUI.SetActive(false);
                return;
        }

        captchaUI.GetComponent<CaptchaUI>().UpdateProgressText(stage, lastStage - 1);
        captchaUI.GetComponent<CaptchaUI>().ResetInputField();
        captchaUI.GetComponent<CaptchaUI>().GenerateCaptcha();
    }

}
