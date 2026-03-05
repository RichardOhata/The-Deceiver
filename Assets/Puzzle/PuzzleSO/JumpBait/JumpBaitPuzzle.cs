using TMPro;
using UnityEngine;

public class JumpBaitPuzzle : Puzzle
{
    [SerializeField] private GameObject jumpTrigger;
    [SerializeField] private TextMeshProUGUI pauseMenuText;
    [SerializeField] private GameObject jumpUITrigger;
    [SerializeField] private GameObject firstPlatform;
    [SerializeField] private GameObject secondPlatform;
    private bool playerIsInRangeFirst = false;
    private bool playerIsInRangeSecond = false;

    private void Awake()
    {
        if (SaveManager.Instance != null)
        {
            isSolved = SaveManager.Instance.currentData.puzzleProgress.jumpBait.isSolved;
        }
    }
    public override void StartPuzzle()
    {
        if (!base.hasStarted)
        {
            base.StartPuzzle();
        }
    }

    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        jumpTrigger.SetActive(false);
    }

    public override void UpdatePuzzleStatus()
    {
        SaveManager.Instance.currentData.puzzleProgress.jumpBait.isSolved = true;
        SaveManager.Instance.SaveGame();
        Debug.Log("Checkpoint and Puzzle State Auto-Saved!");
    }

    private void Update()
    {
        if (playerIsInRangeFirst && Input.GetKeyDown(KeyCode.Space))
        {
            DisableFirstPlatform();
        }

        if (playerIsInRangeSecond && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F)))
        {
            DisableSecondPlatform();
        }
    }

    public void HasDied()
    {
        jumpUITrigger.GetComponent<UIUpdate>().SetText("? to Jump");
        firstPlatform.SetActive(true);
        playerIsInRangeFirst = false;
        secondPlatform.SetActive(true);
        playerIsInRangeSecond = false;
    }

    public void UpdatePauseMenu(string condition)
    {
        switch (condition)
        {
            case "reset":
                pauseMenuText.text = "PAUSED";
                break;
            case "first":
                pauseMenuText.text = "FAUSED";
                break;
            case "second":
                pauseMenuText.text = "EAUSED";
                break;
        }
    } 

    public void DisableFirstPlatform()
    {
        firstPlatform.SetActive(false);
    }

    public void DisableSecondPlatform()
    {
        secondPlatform.SetActive(false);
    }

    public void SetPlayerInRangeFirst(bool state)
    {
        playerIsInRangeFirst = state;
    }

    public void SetPlayerInRangeSecond(bool state)
    {
        playerIsInRangeSecond = state;
    }
}
