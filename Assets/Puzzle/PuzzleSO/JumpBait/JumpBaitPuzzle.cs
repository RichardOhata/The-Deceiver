using TMPro;
using UnityEngine;

public class JumpBaitPuzzle : Puzzle
{
    [SerializeField]
    private GameObject jumpTrigger;
    [SerializeField] private GameObject pauseMenuHint;
    [SerializeField] private GameObject jumpUITrigger;

    public override void StartPuzzle()
    {
        base.StartPuzzle();
        pauseMenuHint.SetActive(true);

    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        jumpTrigger.SetActive(false);

        if (pauseMenuHint != null)
            pauseMenuHint.SetActive(false);
    }

   public void HasDied()
    {
         jumpUITrigger.GetComponent<UIUpdate>().SetText("? to Jump");
    }

    public void UpdatePauseHint()
    {
        pauseMenuHint.GetComponent<TextMeshProUGUI>().text = "E";
    }
}
