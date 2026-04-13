using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ABCPuzzle : Puzzle
{
    [SerializeField] private GameObject interactTrigger;
    [SerializeField] private GameObject abcUI;
    [SerializeField] private GameObject captchaConsole;
    private GameObject player;

    [SerializeField] private GameObject slidingDoor;
    [SerializeField] private GameObject monitor;

    private void Awake()
    {
        if (SaveManager.Instance != null)
        {
            isSolved = SaveManager.Instance.currentData.puzzleProgress.abcPuzzle.isSolved;
        }
        if (isSolved)
        {
            Animator anim = slidingDoor.GetComponent<Animator>();
            anim.SetTrigger("Door_Open");
            anim.speed = 100f;
            interactTrigger.SetActive(false);
            monitor.GetComponent<ExplodableMonitor>().ExplodeMonitor();
            OnDisable();
        }
    }

    private void Start()
    {
        if (isSolved)
        {
            OnDisable();
        }
    }
    public override void StartPuzzle()
    {
        if (isSolved) return; 
        base.StartPuzzle();
        player = GameObject.FindGameObjectWithTag("Player");

    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        CloseUI();
        monitor.GetComponent<ExplodableMonitor>().ExplodeMonitor();
        slidingDoor.GetComponent<Animator>().SetTrigger("Door_Open");
        interactTrigger.SetActive(false);
        OnDisable();
    }


    public override void UpdatePuzzleStatus()
    {
        SaveManager.Instance.currentData.puzzleProgress.abcPuzzle.isSolved = isSolved;
        SaveManager.Instance.SaveGame();
    }

    private void OnEnable()
    {
        if (isSolved) return;
        InputManager.Instance.controls.Player.Interact.performed += ToggleABCUI;
    }


    private void OnDisable()
    {
        InputManager.Instance.controls.Player.Interact.performed -= ToggleABCUI;
    }


    private bool IsPlayerNearConsole()
    {

        if (player == null) return false;

        float distance = Vector3.Distance(player.transform.position, captchaConsole.transform.position);
        return distance < 3f;
    }

    private void ToggleABCUI(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        interactTrigger.GetComponent<UIUpdate>().updatePanelText("");
        if (abcUI.gameObject.activeSelf)
        {
            return;
        }

        if (!abcUI.gameObject.activeSelf && IsPlayerNearConsole())
        {
            abcUI.SetActive(true);
            PauseMenuLogic.Instance.HandlePause(false, false);
        }
    }
    public override bool IsUIOpen => abcUI.activeSelf;
    public override void CloseUI()
    {
        abcUI.SetActive(false);
        PauseMenuLogic.Instance.HandlePause(false);
    }
}
