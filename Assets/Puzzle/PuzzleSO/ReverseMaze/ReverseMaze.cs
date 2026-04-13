using UnityEngine;

public enum MazeIdentifier
{
    Maze1,
    Maze2
}

public class ReverseMaze : Puzzle
{
    [SerializeField]
    private MazeIdentifier MazeID;

    [SerializeField]
    private FirstPersonAudio playerAudio;

    [SerializeField]
    private int activeZones = 0;

    [SerializeField]
    private Drawboard drawBoard;

    [SerializeField] private AreaManager areaManager;

    private void Awake()
    {
        if (SaveManager.Instance != null)
        {
            switch (MazeID)
            {
                case MazeIdentifier.Maze1:
                    isSolved = SaveManager.Instance.currentData.puzzleProgress.maze1Puzzle.isSolved;
                    break;
                case MazeIdentifier.Maze2:
                    isSolved = SaveManager.Instance.currentData.puzzleProgress.maze1Puzzle.isSolved;
                    break;
            }
            if (isSolved)
            {
                drawBoard.enabled = false;
            }
          
        }
    }

    private void Start()
    {
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FirstPersonAudio>();
    }
    public override void StartPuzzle()
    {
        base.StartPuzzle();
    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        drawBoard.enabled = false;
        areaManager.GetComponent<AreaManager>().OnPuzzleSolved();
    }

    public override void UpdatePuzzleStatus()
    {
        switch (MazeID)
        {
            case MazeIdentifier.Maze1:
                SaveManager.Instance.currentData.puzzleProgress.maze1Puzzle.isSolved = true;
                break;
            case MazeIdentifier.Maze2:
                SaveManager.Instance.currentData.puzzleProgress.maze2Puzzle.isSolved = true;
                break;
        }
        SaveManager.Instance.SaveGame();
    }

    public void MuteSteps(bool isMute)
    { 
        if (isMute)
        {
            activeZones++;
        } else
        {
            activeZones = Mathf.Max(0, activeZones - 1);
        }
        playerAudio.stepAudio.mute = (activeZones > 0);
        playerAudio.runningAudio.mute = (activeZones > 0);
    }

    public override bool IsUIOpen => drawBoard != null && drawBoard.mazeUIOpen;

    public override void CloseUI()
    {
        drawBoard.CloseMazeUI();
        PauseMenuLogic.Instance.HandlePause(false);
    }
}
