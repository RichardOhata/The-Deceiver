using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    public PuzzleData puzzleData;
    public virtual void StartPuzzle()
    {
        Debug.Log($"Starting puzzle: {puzzleData.puzzleName}");
    }

    public abstract void SolvePuzzle();

    public virtual void ShowHint()
    {
       
    }
}
