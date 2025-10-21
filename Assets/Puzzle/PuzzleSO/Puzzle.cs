using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    public PuzzleData puzzleData;
    public virtual void StartPuzzle()
    {
        Debug.Log($"Starting puzzle: {puzzleData.puzzleName}");
    }

    public virtual void SolvePuzzle()
    {
        Debug.Log($"Puzzle solved: {puzzleData.puzzleName}");
        PlaySolveSFX();
    }

    public virtual void ShowHint()
    {
       
    }

    private void PlaySolveSFX()
    {
        if (SFXManager.Instance != null && !puzzleData.isSolved)
        {
            SFXManager.Instance.PlaySFX();
        }
    }
}
