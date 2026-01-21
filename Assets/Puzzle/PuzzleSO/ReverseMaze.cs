using UnityEngine;

public class ReverseMaze : Puzzle
{

    [SerializeField]
    private FirstPersonAudio playerAudio;

    [SerializeField]
    private int activeZones = 0;

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
        if (isMute)
        {
            activeZones++;
        } else
        {
            activeZones = Mathf.Max(0, activeZones - 1);
        }
        playerAudio.stepAudio.mute = (activeZones > 0);
    }
}
