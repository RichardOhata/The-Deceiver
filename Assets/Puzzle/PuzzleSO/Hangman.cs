using UnityEngine;

using System;
using System.Diagnostics;
//using System.DirectoryServices.AccountManagement;

// Attempt to find users name, make them do hangman, ask them if this is their actual name, if yes, puzzle solve, if not, ask them to input their real name
public class Hangman : Puzzle
{
    [SerializeField] private GameObject interactTrigger;
    [SerializeField] private GameObject hangmanUI;
    [SerializeField] private GameObject captchaConsole;
    private GameObject player;

    [SerializeField] private GameObject slidingDoor;
    [SerializeField] private GameObject monitor;


    [SerializeField] private string answer;

    [SerializeField] public string detectedUser = "";

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
        InputManager.Instance.controls.Player.Interact.performed += ToggleHangmanUI;
    }


    private void OnDisable()
    {
        InputManager.Instance.controls.Player.Interact.performed -= ToggleHangmanUI;
    }


    private bool IsPlayerNearConsole()
    {

        if (player == null) return false;

        float distance = Vector3.Distance(player.transform.position, captchaConsole.transform.position);
        return distance < 3f; 
    }

    private void ToggleHangmanUI(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        interactTrigger.GetComponent<UIUpdate>().updatePanelText("");
        if (hangmanUI.gameObject.activeSelf)
        {
            return;
        }

        if (!hangmanUI.gameObject.activeSelf && IsPlayerNearConsole())
        {
            hangmanUI.SetActive(true);
            InputManager.Instance.controls.Player.Disable();
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

    public bool IsHangmanOpen => hangmanUI.activeSelf;

    public void CloseCaptcha()
    {
        hangmanUI.SetActive(false);
        InputManager.Instance.controls.Player.Enable();
        player.GetComponentInChildren<FirstPersonLook>().enabled = true;
        player.GetComponentInChildren<FirstPersonAudio>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }


    // Code for determining player's full name for surprise factor. This part of the code was helped developed with the use of AI.
    void Start()
    {
            string player_name = "";
            #if UNITY_STANDALONE_WIN
                player_name = GetWindowsFullName(); // Retrieves window's first name
            UnityEngine.Debug.Log(player_name);
            #endif


            if (string.IsNullOrEmpty(player_name))
            {
                // Check Steam Username through its API
            }

            if (string.IsNullOrEmpty(player_name))
            {
                player_name = System.Environment.UserName;
        }



        detectedUser = !string.IsNullOrEmpty(player_name) ? player_name : "Player";

        hangmanUI.GetComponent<HangmanUI>().PopulateAnswer(detectedUser);
    }

    private string GetWindowsFullName()
    {
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                // This command specifically asks for the "Full Name" linked to the account
                Arguments = "/c wmic useraccount where name='%username%' get fullname",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                // WMIC output usually includes the header "FullName", we strip it out
                string cleanName = output.Replace("FullName", "").Trim(); 
                if (!string.IsNullOrEmpty(cleanName))
                {
                    return cleanName.Split(' ')[0];
                }
                return string.Empty;
            }
        }
        catch
        {
            return string.Empty; // Last resort fallback
        }
    }

}
