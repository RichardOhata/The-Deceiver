using UnityEngine;

public class StepSoundMemory : Puzzle
{
    [SerializeField]
    private GameObject pathTitles;
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
}
