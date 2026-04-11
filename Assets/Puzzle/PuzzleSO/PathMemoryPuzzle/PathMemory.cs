using System.Collections.Generic;
using UnityEngine;

public class PathMemory : Puzzle
{
    [SerializeField]
    private GameObject updatedCheckpoint;

    [Header("Camera Control Swapping")]
    [SerializeField] private FirstPersonLook normalLookScript;
    [SerializeField] private RubberBandCamera puzzleLookScript;
    [SerializeField] private Vector3 puzzleFacingDirection = new Vector3(0, 0, 0);
    [SerializeField] private List<GameObject> keyChangeTiles;

    private bool isRandomizingTiles = false;
    private void Awake()
    {
        if (SaveManager.Instance != null)
        {
            isSolved = SaveManager.Instance.currentData.puzzleProgress.pathMemory.isSolved;
        }
    }
    public override void StartPuzzle()
    {
        base.StartPuzzle();
    }

    private void OnEnable()
    {
     
        if (normalLookScript != null) normalLookScript.enabled = false;

        if (puzzleLookScript != null)
        {
            puzzleLookScript.lockedBodyRotation = puzzleFacingDirection;
            puzzleLookScript.enabled = true;
        }
    }

    private void OnDisable()
    {
    
        RestoreNormalCamera();
    }

    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        //SaveCheckPointLocation(updatedCheckpoint);
        RestoreNormalCamera();
    }

    public void RestoreNormalCamera()
    {
        if (puzzleLookScript != null) puzzleLookScript.enabled = false;
        if (normalLookScript != null) normalLookScript.enabled = true;
    }


    public void RandomizeTiles()
    {
        if (!isRandomizingTiles)
        {
            isRandomizingTiles = true;
            ShuffleModifierTiles();
        }
    }
   

    private void ShuffleModifierTiles()
    {
        // If the list is empty, don't try to shuffle
        if (keyChangeTiles == null || keyChangeTiles.Count <= 1) return;

        // We use the classic "Fisher-Yates" shuffle algorithm
        for (int i = 0; i < keyChangeTiles.Count; i++)
        {
            // Pick a random tile from the remaining ones
            int randomIndex = Random.Range(i, keyChangeTiles.Count);

            // Swap their physical positions in the world!
            Vector3 tempPosition = keyChangeTiles[i].transform.position;
            keyChangeTiles[i].transform.position = keyChangeTiles[randomIndex].transform.position;
            keyChangeTiles[randomIndex].transform.position = tempPosition;
        }

        isRandomizingTiles = false;
    }

    public override void UpdatePuzzleStatus()
    {
        SaveManager.Instance.currentData.puzzleProgress.pathMemory.isSolved = true;
        SaveManager.Instance.SaveGame();
        Debug.Log("Checkpoint and Puzzle State Auto-Saved!");
    }
}
