using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    public PuzzleData puzzleData;
    public bool isSolved = false;
    public bool hasStarted = false;

    [Header("Save Settings")]
    public Transform checkpointLocation;

    public virtual void StartPuzzle()
    {
        if (isSolved) return;
        Debug.Log($"Starting puzzle: {puzzleData.puzzleName}");
        hasStarted = true;
        SaveCheckPointLocation(); 
    }

    public virtual void SolvePuzzle()
    {
        Debug.Log($"Puzzle solved: {puzzleData.puzzleName}");
        if (isSolved) return;
        isSolved = true;
        PlaySolveSFX();
        UpdatePuzzleStatus();
    }


    public virtual bool IsUIOpen => false;

    public virtual void CloseUI()
    {
       
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

    private void SaveCheckPointLocation()
    {
        if (checkpointLocation != null && SaveManager.Instance != null)
        {
            SaveManager.Instance.currentData.playerPosition.x = checkpointLocation.position.x;
            SaveManager.Instance.currentData.playerPosition.y = checkpointLocation.position.y;
            SaveManager.Instance.currentData.playerPosition.z = checkpointLocation.position.z;

            SaveManager.Instance.SaveGame();
            Debug.Log("Checkpoint and Puzzle State Auto-Saved!");
        }
    }

    public virtual void UpdatePuzzleStatus()
    {

    }
}
