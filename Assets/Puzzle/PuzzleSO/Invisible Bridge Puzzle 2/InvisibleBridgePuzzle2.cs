using Unity.VisualScripting;
using UnityEngine;

public class InvisibleBridgePuzzle2 : Puzzle
{
    [SerializeField] private GameObject invisibleBridge;

    private bool initialFullScreenState;

    private void Awake()
    {
        if (SaveManager.Instance != null)
        {
            isSolved = SaveManager.Instance.currentData.puzzleProgress.invisibleBridge2.isSolved;
        }
        if (isSolved)
        {
            DisplayBridge(true);
            
        }
    }


    public override void StartPuzzle()
    {
        if (isSolved) return;
        base.StartPuzzle();
    }
    public override void SolvePuzzle()
    {
        if (isSolved) return;
        base.SolvePuzzle();
    }

    public override void UpdatePuzzleStatus()
    {
        SaveManager.Instance.currentData.puzzleProgress.invisibleBridge2.isSolved = true;
        SaveManager.Instance.SaveGame();
        Debug.Log("Checkpoint and Puzzle State Auto-Saved!");
    }

    private void Start()
    {
        if (isSolved) return;
        
        initialFullScreenState = Screen.fullScreen;
        DisplayBridge(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isSolved) return;
        bool hasToggledScreen = (Screen.fullScreen != initialFullScreenState);

        DisplayBridge(hasToggledScreen);
    }

    private void DisplayBridge(bool isVisibile)
    {
        invisibleBridge.SetActive(isVisibile);
    }
}
