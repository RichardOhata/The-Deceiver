using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    protected GameObject player;
    protected Camera playerCamera;
    public PuzzleData puzzleData;
    public bool isSolved = false;
    public bool hasStarted = false;

    [Header("Checkpoint Settings")]
    [SerializeField] private bool saveCheckpointOnStart;
    [SerializeField] private bool saveCheckpointOnSolve;
    
    [Header("Save Settings")]
    public Transform checkpointInitialLocation;
    public Transform checkpointFinishLocation;


    protected virtual void Awake()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        if (playerCamera == null) playerCamera = Camera.main;
    }

    public virtual void StartPuzzle()
    {
       
        if (isSolved) return;
        Debug.Log($"Starting puzzle: {puzzleData.puzzleName}");
        hasStarted = true;
        if (saveCheckpointOnStart)
        {
            SaveCheckPointLocation(checkpointInitialLocation);
        }
    }

    public virtual void SolvePuzzle()
    {
        Debug.Log($"Puzzle solved: {puzzleData.puzzleName}");
        if (isSolved) return;
        isSolved = true;
        PlaySolveSFX();
        UpdatePuzzleStatus();
        if (saveCheckpointOnSolve)
        {
            SaveCheckPointLocation(checkpointFinishLocation);
        }
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

    public void SaveCheckPointLocation(Transform checkpointLocation = null, GameObject overrideLocation = null)
    {
        Transform targetLocation = overrideLocation != null ? overrideLocation.transform : checkpointLocation;

        if (targetLocation != null && SaveManager.Instance != null)
        {
            SaveManager.Instance.currentData.playerPosition.x = targetLocation.position.x;
            SaveManager.Instance.currentData.playerPosition.y = targetLocation.position.y;
            SaveManager.Instance.currentData.playerPosition.z = targetLocation.position.z;

            SaveManager.Instance.SaveGame();
            Debug.Log("Checkpoint and Puzzle State Auto-Saved!");
        }
        else
        {
            Debug.LogWarning("Save failed: No valid checkpoint location or SaveManager found.");
        }
    }

    public virtual void UpdatePuzzleStatus()
    {

    }
}
