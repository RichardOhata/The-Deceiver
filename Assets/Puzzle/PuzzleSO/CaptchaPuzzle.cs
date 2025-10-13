using UnityEngine;

public class CaptchaPuzzle : Puzzle
{
    
    [SerializeField] private GameObject captchaUI;
    [SerializeField] private GameObject captchaConsole;
    private GameObject player;
    public override void StartPuzzle()
    {
        base.StartPuzzle();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void SolvePuzzle()
    {
        puzzleData.isSolved = true;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {

            if (IsPlayerNearConsole())
            {
                ToggleCaptchaUI();
            }
        }
    }
    private bool IsPlayerNearConsole()
    {
            // Replace later
        if (player == null) return false;

        float distance = Vector3.Distance(player.transform.position, captchaConsole.transform.position);
        return distance < 3f; // example: within 3 units
    }
    private void ToggleCaptchaUI()
    {
        if (!captchaUI.gameObject.activeSelf)
        {
            captchaUI.SetActive(true);
            player.gameObject.GetComponentInChildren<FirstPersonLook>().enabled = false;
            player.gameObject.GetComponentInChildren<FirstPersonAudio>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        } else
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
}
