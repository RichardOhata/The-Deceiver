using UnityEngine;

public class JumpBaitPuzzle : Puzzle
{
    [SerializeField]
    private GameObject jumpTrigger;
    [SerializeField] private GameObject pauseMenuHint;
    public override void StartPuzzle()
    {
        base.StartPuzzle();
        pauseMenuHint.SetActive(true);

    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        jumpTrigger.SetActive(false);
        puzzleData.isSolved = true;

        if (pauseMenuHint != null)
            pauseMenuHint.SetActive(false);
    }
}
