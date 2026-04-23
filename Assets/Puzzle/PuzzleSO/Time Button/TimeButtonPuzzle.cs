using System;
using TMPro;
using UnityEngine;

public class TimeButtonPuzzle : Puzzle
{
    [Header("Puzzle Components")]
    [SerializeField] private GameObject clockTextGO;
    [SerializeField] private GameObject buttons;
    private TextMeshPro clockText;
    private String time;
    private int currentIndex = 1;
    private int lastIndex = 4;

    [Header("UI References")]
    [SerializeField] private UIUpdate uiPrompt;
    [SerializeField] private GameObject slidingDoor;

    protected override void Awake()
    {
        base.Awake();
        if (SaveManager.Instance != null)
        {
            isSolved = SaveManager.Instance.currentData.puzzleProgress.timeButtonPuzzle.isSolved;
        }

        if (isSolved)
        {
            Animator anim = slidingDoor.GetComponent<Animator>();
            anim.SetTrigger("Door_Open");
            anim.speed = 100f;
            buttons.SetActive(false);
        }
    }
    public override void StartPuzzle()
    {
        if (!isSolved) base.StartPuzzle();
        clockText = clockTextGO.GetComponent<TextMeshPro>();
    }

    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        slidingDoor.GetComponent<Animator>().SetTrigger("Door_Open");
        uiPrompt.GetComponent<UIUpdate>().DisablePanel();
    }

    public override void UpdatePuzzleStatus()
    {
        SaveManager.Instance.currentData.puzzleProgress.timeButtonPuzzle.isSolved = isSolved;
        SaveManager.Instance.SaveGame();
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
