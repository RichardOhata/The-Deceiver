using TMPro;
using UnityEngine;

public class ABCPuzzle : Puzzle
{
    [Header("Puzzle Components")]
    [SerializeField] private GameObject monitor;

    [Header("UI References")]
    [SerializeField] private GameObject abcUI;
    [SerializeField] private GameObject backgroundDimmer;
    [SerializeField] private UIUpdate uiPrompt;

    [SerializeField] private GameObject slidingDoor;
    protected override void Awake()
    {
        base.Awake();
        if (SaveManager.Instance != null)
        {
            isSolved = SaveManager.Instance.currentData.puzzleProgress.abcPuzzle.isSolved;
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

    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        CloseUI();
        monitor.GetComponent<ExplodableMonitor>().ExplodeMonitor();
        slidingDoor.GetComponent<Animator>().SetTrigger("Door_Open");
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


    private bool CanInteractWithConsole()
    {
        if (base.player == null) return false;

        return LookAtUtility.IsPointedAt(base.playerCamera, monitor, 0.5f, 3.5f);
    }

    private void ToggleABCUI(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {

        if (!abcUI.activeSelf && CanInteractWithConsole())
        {
            abcUI.gameObject.SetActive(true);
            backgroundDimmer.SetActive(true);
            PauseMenuLogic.Instance.HandlePause(false);
        }
    }
    public override bool IsUIOpen => abcUI.activeSelf;
    public override void CloseUI()
    {
        abcUI.SetActive(false);
        backgroundDimmer.SetActive(false);
        PauseMenuLogic.Instance.HandlePause(false);
    }
}
