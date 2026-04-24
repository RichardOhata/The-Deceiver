using TMPro;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [Header("Settings")]
    public EnvironmentArea currentArea;

    [SerializeField] private TextMeshPro progressionText;
    [SerializeField] private GameObject progressionTextBoard;

    private int _puzzlesSolved = 0;


    private ISeasonalData envData => GetCurrentAreaData();

    private void Awake()
    {
        if (SaveManager.Instance != null)
        {
            _puzzlesSolved = envData.puzzleProgress;
        }
    }
    void Start()
    {
        if (envData.areaComplete)
        {
            progressionTextBoard.GetComponent<Animator>().Play("Progress_Text_Board_Slide_Down", 0, 0f);
            Destroy(progressionText);
            progressionText = null;
        } else
        {
            if (progressionText != null)
            {
                UpdateUI();
            }
        }
        
    }

    public void OnPuzzleSolved()
    {
        _puzzlesSolved++;

        envData.puzzleProgress = _puzzlesSolved;
       

        // Clamp to ensure we don't go over the total defined in the SO
        if (_puzzlesSolved > currentArea.totalPuzzles)
            _puzzlesSolved = currentArea.totalPuzzles;

        if (progressionText != null)
        {
            UpdateUI();
        }

        if (currentArea.IsAreaComplete(_puzzlesSolved))
        {
            progressionTextBoard.GetComponent<Animator>().SetTrigger("Slide_Down");
            Destroy(progressionText);
            envData.areaComplete = true;
        }
        SaveManager.Instance.SaveGame();
    }

    private void UpdateUI()
    {
        // Use the helper method from the SO
        string progress = currentArea.GetProgressString(_puzzlesSolved);
        progressionText.text = progress;
    }

    private ISeasonalData GetCurrentAreaData()
    {
        var env = SaveManager.Instance.currentData.environmentData;
        return currentArea.areaID switch
        {
            AreaID.Summer => env.summerEnvironment,
            AreaID.Autumn => env.autumnEnvironment,
            AreaID.Winter => env.winterEnvironment,
            _ => env.summerEnvironment
        };
    }
}
