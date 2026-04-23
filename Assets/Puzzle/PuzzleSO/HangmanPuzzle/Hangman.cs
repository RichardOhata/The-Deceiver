using UnityEngine;
using System;
using System.Diagnostics;

// Attempt to find users name, make them do hangman, ask them if this is their actual name, if yes, puzzle solve, if not, ask them to input their real name
public class Hangman : Puzzle
{
    [Header("Puzzle Components")]
    [SerializeField] private GameObject monitor;
    [SerializeField] private string answer;
    [SerializeField] public string detectedUser = "";

    [Header("UI References")]
    [SerializeField] private GameObject hangmanUI;
    [SerializeField] private GameObject backgroundDimmer;
    [SerializeField] private UIUpdate uiPrompt;

    [SerializeField] private GameObject slidingDoor;

    protected override void Awake()
    {
        base.Awake();
        if (SaveManager.Instance != null)
        {
            isSolved = SaveManager.Instance.currentData.puzzleProgress.hangmanPuzzle.isSolved;
        }

        if (isSolved)
        {
            Animator anim = slidingDoor.GetComponent<Animator>();
            anim.SetTrigger("Door_Open");
            anim.speed = 100f;
            monitor.GetComponent<ExplodableMonitor>().ExplodeMonitor();
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
        player = GameObject.FindGameObjectWithTag("Player");

    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        CloseUI();
        monitor.GetComponent<ExplodableMonitor>().ExplodeMonitor();
        slidingDoor.GetComponent<Animator>().SetTrigger("Door_Open");
        OnDisable();
    }

    private void OnEnable()
    {
        if (isSolved) return;
        InputManager.Instance.controls.Player.Interact.performed += ToggleHangmanUI;
    }


    private void OnDisable()
    {
        InputManager.Instance.controls.Player.Interact.performed -= ToggleHangmanUI;
    }


    private bool CanInteractWithConsole()
    {
        if (base.player == null) return false;

        return LookAtUtility.IsPointedAt(base.playerCamera, monitor, 0.5f, 3.5f);
    }

    private void ToggleHangmanUI(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

        if (!hangmanUI.activeSelf && CanInteractWithConsole())
        {
            hangmanUI.gameObject.SetActive(true);
            backgroundDimmer.SetActive(true);
            PauseMenuLogic.Instance.HandlePause(false);
        }
    }

    public override bool IsUIOpen => hangmanUI.activeSelf;
    public override void CloseUI()
    {
        hangmanUI.SetActive(false);
        backgroundDimmer.SetActive(false);
        PauseMenuLogic.Instance.HandlePause(false);
    }

    public override void UpdatePuzzleStatus()
    {
        SaveManager.Instance.currentData.puzzleProgress.hangmanPuzzle.isSolved = true;
        SaveManager.Instance.SaveGame();
        UnityEngine.Debug.Log("Checkpoint and Puzzle State Auto-Saved!");
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
