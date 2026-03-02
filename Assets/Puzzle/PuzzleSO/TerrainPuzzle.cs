using TMPro;
using UnityEngine;

public class TerrainPuzzle : Puzzle
{
    [SerializeField] private GameObject interactTrigger;
    [SerializeField] private GameObject terrainUI;
    [SerializeField] private GameObject captchaConsole;
    private GameObject player;

    [SerializeField] private GameObject slidingDoor;
    [SerializeField] private GameObject monitor;

    [SerializeField] private TextMeshPro summerWallText;
    [SerializeField] private TextMeshPro autumnWallText;
    [SerializeField] private TextMeshPro winterWallText;
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
        slidingDoor.GetComponent<Animator>().SetTrigger("OpenSlidingDoor");
        slidingDoor.GetComponentInChildren<AudioSource>().PlayDelayed(1.5f);
        interactTrigger.SetActive(false);
        OnDisable();
    }

    private void OnEnable()
    {
        InputManager.Instance.controls.Player.Interact.performed += ToggleTerrainUI;
    }


    private void OnDisable()
    {
        InputManager.Instance.controls.Player.Interact.performed -= ToggleTerrainUI;
    }


    private bool IsPlayerNearConsole()
    {

        if (player == null) return false;

        float distance = Vector3.Distance(player.transform.position, captchaConsole.transform.position);
        return distance < 3f;
    }

    private void ToggleTerrainUI(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        interactTrigger.GetComponent<UIUpdate>().updatePanelText("");
        if (terrainUI.gameObject.activeSelf)
        {
            return;
        }

        if (!terrainUI.gameObject.activeSelf && IsPlayerNearConsole())
        {
            terrainUI.SetActive(true);
            PauseMenuLogic.Instance.HandlePause(false, false);
        }
    }
    public override bool IsUIOpen => terrainUI.activeSelf;
    public override void CloseUI()
    {
        terrainUI.SetActive(false);
        PauseMenuLogic.Instance.HandlePause(false);
    }


    public void UpdateTerrainWallText(string summer, string autumn, string winter)
    {
        summerWallText.text = summer;
        autumnWallText.text = autumn;
        winterWallText.text = winter;
    }
}
