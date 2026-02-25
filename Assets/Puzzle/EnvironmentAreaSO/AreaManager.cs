using TMPro;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [Header("Settings")]
    public EnvironmentArea currentArea;

    [SerializeField] private TextMeshPro progressionText;
    [SerializeField] private GameObject slidingDoor;

    private int _puzzlesSolved = 0;
    void Start()
    {
        // Example: Load data from save system
        // _puzzlesSolved = SaveSystem.Load(currentArea.areaID);
        if (progressionText != null)
        {
            UpdateUI();
        }
        
    }

    public void OnPuzzleSolved()
    {
        _puzzlesSolved++;

        // Clamp to ensure we don't go over the total defined in the SO
        if (_puzzlesSolved > currentArea.totalPuzzles)
            _puzzlesSolved = currentArea.totalPuzzles;

        if (progressionText != null)
        {
            UpdateUI();
        }

        if (currentArea.IsAreaComplete(_puzzlesSolved))
        {
            slidingDoor.GetComponent<Animator>().SetTrigger("OpenSlidingDoor");
            slidingDoor.GetComponentInChildren<AudioSource>().PlayDelayed(1.5f);
            Destroy(progressionText);
            //Debug.Log($"Area {currentArea.displayName} Completed!");
        }
    }

    private void UpdateUI()
    {
        // Use the helper method from the SO
        string progress = currentArea.GetProgressString(_puzzlesSolved);
        progressionText.text = progress;
        Debug.Log($"Current Progress: {progress}");
    }
}
