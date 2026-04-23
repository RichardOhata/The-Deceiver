using UnityEngine;

public class StepSoundMemory : Puzzle
{

    [Header("Puzzle Components")]
    [SerializeField] private GameObject pathTitles;

    [SerializeField] private GameObject correctTitles;

    [SerializeField] private GameObject incorrectTitles;

    [SerializeField]
    private GameObject slidingDoor;

    private FirstPersonAudio playerAudio;

    private int activeZones = 0;
    protected override void Awake()
    {
        base.Awake();
        if (SaveManager.Instance != null)
        {
            isSolved = SaveManager.Instance.currentData.puzzleProgress.stepSoundMemory.isSolved;
        }
        if (isSolved)
        {
            Animator anim = slidingDoor.GetComponent<Animator>();
            anim.SetTrigger("Door_Open");
            anim.speed = 100f;
        }
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FirstPersonAudio>();
    }

    public override void StartPuzzle()
    {
        base.StartPuzzle();

    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
    }

    public void ResetTiles()
    {
        foreach(BlankPlaneLogic child in pathTitles.GetComponentsInChildren<BlankPlaneLogic>())
        {
            child.ResetStatus();
        }
    }

    private void Update()
    {
        if (isSolved)
        {
            return;
        }

        if (CheckValidTiles() && CheckInValidTiles())
        {
            slidingDoor.GetComponent<Animator>().SetTrigger("Door_Open");
            SolvePuzzle();
        }
    }

    private bool CheckValidTiles()
    {
        foreach(BlankPlaneLogic tile in correctTitles.GetComponentsInChildren<BlankPlaneLogic>())
        {
            if (!tile.steppedOn)
            {
                return false;
            }
        }
        return true;
    }
    public void MuteSteps(bool isMute)
    {
        if (isMute)
        {
            activeZones++;
        }
        else
        {
            activeZones = Mathf.Max(0, activeZones - 1);
        }
        playerAudio.stepAudio.mute = (activeZones > 0);
        playerAudio.runningAudio.mute = (activeZones > 0);
    }

    private bool CheckInValidTiles()
    {
        foreach (BlankPlaneLogic tile in incorrectTitles.GetComponentsInChildren<BlankPlaneLogic>())
        {
            if (tile.steppedOn)
            {
                return false;
            }
        }
        return true;
    }


    public override void UpdatePuzzleStatus()
    {
        SaveManager.Instance.currentData.puzzleProgress.stepSoundMemory.isSolved = true;
        SaveManager.Instance.SaveGame();
        Debug.Log("Checkpoint and Puzzle State Auto-Saved!");
    }
}
