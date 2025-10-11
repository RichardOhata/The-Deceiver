using UnityEngine;

public class JumpBaitPuzzle : Puzzle
{
    [SerializeField]
    private GameObject jumpTrigger;
    public override void StartPuzzle()
    {
        base.StartPuzzle();
    }
    public override void SolvePuzzle()
    {
        jumpTrigger.SetActive(false);
        puzzleData.isSolved = true;
    }
}
