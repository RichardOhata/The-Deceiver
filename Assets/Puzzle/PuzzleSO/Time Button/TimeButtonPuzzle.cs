using System;
using TMPro;
using UnityEngine;

public class TimeButtonPuzzle : Puzzle
{
    [SerializeField] private GameObject clockTextGO;
    private TextMeshPro clockText;
    private String time;

    [SerializeField] private GameObject UIPrompt;

    private int currentIndex = 1;
    private int lastIndex = 4;
    [SerializeField] private GameObject slidingDoor;
    public override void StartPuzzle()
    {
        base.StartPuzzle();
        clockText = clockTextGO.GetComponent<TextMeshPro>();
    }

    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        //UIPrompt.GetComponent<UIUpdate>().DisablePanel();
        slidingDoor.GetComponent<Animator>().SetTrigger("OpenSlidingDoor");
        slidingDoor.GetComponentInChildren<AudioSource>().PlayDelayed(1.5f);
    }

    void Update()
    {
        time = DateTime.Now.ToString("h:mm:ss");
        if (clockText != null)
        {
            // Updates every frame with the current local time + seconds
            clockText.text = time;
        }
   
    }

    public void CheckButtonPress(GameObject reference, int index, int numberToMatch)
    {
        if (index != currentIndex)
        {
            return;
        }
        if (time.Contains(numberToMatch.ToString())) {
            reference.SetActive(false);
            currentIndex++;
            if (currentIndex > lastIndex)
            {
                SolvePuzzle();
            }
        }
    }
}
