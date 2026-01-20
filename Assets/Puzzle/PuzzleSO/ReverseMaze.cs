using UnityEngine;

public class ReverseMaze : Puzzle
{

    [SerializeField]
    private FirstPersonAudio playerAudio;

    private void Start()
    {
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

    public void MuteSteps(bool isMute)
    {
        playerAudio.stepAudio.mute = isMute;
    }
}
