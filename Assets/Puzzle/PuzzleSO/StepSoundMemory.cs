using UnityEngine;

public class StepSoundMemory : Puzzle
{
    [SerializeField]
    private GameObject pathTitles;

    [SerializeField]
    private GameObject correctTitles;

    [SerializeField]
    private GameObject incorrectTitles;

    [SerializeField]
    private GameObject slidingDoor;

    public override void StartPuzzle()
    {
        base.StartPuzzle();

    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
    }


    public void MuteFootSteps()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FirstPersonAudio>().stepAudio.mute = true;
    }
    public void UnMuteFootSteps()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FirstPersonAudio>().stepAudio.mute = false;
    }

    public void ResetTiles()
    {
        MuteFootSteps();
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
            slidingDoor.GetComponent<Animator>().SetTrigger("OpenSlidingDoor");
            slidingDoor.GetComponentInChildren<AudioSource>().PlayDelayed(1.5f);
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

}
