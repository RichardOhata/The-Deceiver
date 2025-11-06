using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    public PuzzleData puzzleData;
    public bool isSolved = false;
    public virtual void StartPuzzle()
    {
        Debug.Log($"Starting puzzle: {puzzleData.puzzleName}");
    }

    public virtual void SolvePuzzle()
    {
        Debug.Log($"Puzzle solved: {puzzleData.puzzleName}");
        if (isSolved) return;
        isSolved = true;

        PlaySolveSFX();
    }

    public virtual void ShowHint()
    {
       
    }

    private void PlaySolveSFX()
    {
        if (SFXManager.Instance != null && isSolved)
        {
            SFXManager.Instance.PlaySFX();
        }
    }
}
