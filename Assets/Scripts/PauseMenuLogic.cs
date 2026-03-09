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


    private bool falseEnableJump = false;

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
            if (!InputManager.Instance.controls.Player.Jump.enabled)
            {
                falseEnableJump = true;
            }
            InputManager.Instance.controls.Player.Disable();
            player.gameObject.GetComponentInChildren<FirstPersonLook>().enabled = false;
            player.gameObject.GetComponentInChildren<FirstPersonAudio>().enabled = false;
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
            if (falseEnableJump)
            {
                InputManager.Instance.controls.Player.Jump.Disable();
            }
            falseEnableJump = false;
            player.gameObject.GetComponentInChildren<FirstPersonLook>().enabled = true;
            player.gameObject.GetComponentInChildren<FirstPersonAudio>().enabled = true;
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
        SceneManager.LoadScene("TitleScreen");
    }
}
