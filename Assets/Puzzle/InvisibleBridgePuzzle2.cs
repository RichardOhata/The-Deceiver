using UnityEngine;

public class InvisibleBridgePuzzle2 : Puzzle
{
    [SerializeField] private GameObject invisibleBridge;

    private bool initialFullScreenState;
    public override void StartPuzzle()
    {
        base.StartPuzzle();
    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
    }

    private void Start()
    {
        initialFullScreenState = Screen.fullScreen;
        DisplayBridge(false);
    }

    // Update is called once per frame
    void Update()
    {
        bool hasToggledScreen = (Screen.fullScreen != initialFullScreenState);

        DisplayBridge(hasToggledScreen);
    }

    private void DisplayBridge(bool isVisibile)
    {
        invisibleBridge.SetActive(isVisibile);
    }
}
