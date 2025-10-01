using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    private bool isPaused = false;

    private GameObject player;

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
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        if (isPaused)
        {
            player.gameObject.GetComponentInChildren<FirstPersonLook>().enabled = false;
            player.gameObject.GetComponentInChildren<FirstPersonAudio>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        } else
        {
            player.gameObject.GetComponentInChildren<FirstPersonLook>().enabled = true;
            player.gameObject.GetComponentInChildren<FirstPersonAudio>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    }
}
