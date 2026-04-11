using UnityEngine;

public class VolumeMemory : Puzzle
{
    [SerializeField]
    private GameObject smoothingRow;

    [SerializeField]
    private GameObject sensRow;

    [SerializeField]
    private GameObject volumeRow;

    [SerializeField]
    private GameObject updatedCheckpoint;

    private void Awake()
    {
        if (SaveManager.Instance != null)
        {
            isSolved = SaveManager.Instance.currentData.puzzleProgress.volumeMemory.isSolved;
        }
    }

    public override void StartPuzzle()
    {
        base.StartPuzzle();

    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        //SaveCheckPointLocation(updatedCheckpoint);
    }


    private void Update()
    {
        UpdateRows(volumeRow, SettingsManager.Instance.volume);
        UpdateRows(sensRow, SettingsManager.Instance.sensitivity);
        UpdateRows(smoothingRow, 1f - SettingsManager.Instance.smoothing / 5f);
    }
    
    private void UpdateRows(GameObject row, float percentage)
    {
        int childCount = row.transform.childCount;
        int amtToRender = Mathf.RoundToInt(percentage * childCount);
        
        for (int index = 0; index < childCount; index++)
        {
            if (index < amtToRender)
            {
                row.transform.GetChild(index).gameObject.SetActive(true);
            } else
            {
                row.transform.GetChild(index).gameObject.SetActive(false);
            }
         

        }
    }

    public override void UpdatePuzzleStatus()
    {
        SaveManager.Instance.currentData.puzzleProgress.volumeMemory.isSolved = true;
        SaveManager.Instance.SaveGame();
        Debug.Log("Checkpoint and Puzzle State Auto-Saved!");
    }
}
