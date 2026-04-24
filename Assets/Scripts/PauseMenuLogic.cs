using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject settingsMenu;

    private bool isPaused = false;

    private GameObject player;

    public static PauseMenuLogic Instance { get; private set; }


    // --- The State Cache Variables ---
    private bool wasJumpEnabled;
    private bool wasCameraEnabled;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        InputManager.Instance.controls.UI.Pause.performed += OnPausePerformed;
    }

    private void OnDisable()
    {
        InputManager.Instance.controls.UI.Pause.performed -= OnPausePerformed;
    }

    private void OnPausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        HandlePause(true);
      
    }

    public void HandlePause(bool fromPauseMenu, bool stopTime = true, bool lockCursor = false)
    {
        Puzzle[] allPuzzles = GameObject.FindObjectsByType<Puzzle>(FindObjectsSortMode.None);
        if (fromPauseMenu)
        {
            foreach (Puzzle puzzle in allPuzzles)
            {
                if (puzzle.IsUIOpen)
                {
                    puzzle.CloseUI();
                    return;
                }
            }
        }
    

        if (settingsMenu.activeSelf == true)
        {
            settingsMenu.SetActive(false);
            pauseMenu.SetActive(true);
            return;
        }


        isPaused = !isPaused;
        if (fromPauseMenu)
        {
            pauseMenu.SetActive(isPaused);
        }
       
        
        if (isPaused)
        {
            wasJumpEnabled = InputManager.Instance.controls.Player.Jump.enabled;

            FirstPersonLook fpl = player.gameObject.GetComponentInChildren<FirstPersonLook>();
            if (fpl != null)
            {
                wasCameraEnabled = fpl.enabled;
                fpl.enabled = false; // Turn it off so it doesn't read mouse inputs in the menu
            }

            InputManager.Instance.controls.Player.Disable();
            var audioScript = player.gameObject.GetComponentInChildren<FirstPersonAudio>();
            if (audioScript != null)
            {
                audioScript.enabled = false;
                audioScript.stepAudio.mute = true;
                audioScript.runningAudio.mute = true;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            AudioListener.pause = true;
            if (stopTime)
            {
                Time.timeScale = 0;
            } 
        }
        else
        {
           
            InputManager.Instance.controls.Player.Enable();
            if (!wasJumpEnabled)
            {
                InputManager.Instance.controls.Player.Jump.Disable();
            }   
            FirstPersonLook fpl = player.gameObject.GetComponentInChildren<FirstPersonLook>();
            if (fpl != null)
            {
                fpl.enabled = wasCameraEnabled;
            }
            var audioScript = player.gameObject.GetComponentInChildren<FirstPersonAudio>();
            if (audioScript != null)
            {
                audioScript.enabled = true;
                audioScript.stepAudio.mute = false;
                audioScript.runningAudio.mute = false;
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;

            AudioListener.pause = false;
        }
    }

    public void SettingsTransition()
    {
        pauseMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
    }

    public void PauseMenuTransition()
    {
        pauseMenu.gameObject.SetActive(true);
        settingsMenu.gameObject.SetActive(false);
    }



    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title Screen");
        AudioListener.pause = false;
    }
}
