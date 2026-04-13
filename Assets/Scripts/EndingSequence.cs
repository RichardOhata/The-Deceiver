using System;
using TMPro;
using UnityEngine;
using System.Collections;

public class EndingSequence : MonoBehaviour
{

    [Header("Cinematic References")]
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private float timeBetweenText = 1.5f; // How long to wait before showing the next line
    [SerializeField] private float textFadeSpeed = 1f;     // How fast the text fades in

    [Header("UI Text Elements (Top to Bottom)")]
    [SerializeField] private TextMeshProUGUI gameCompleteHeader;
    [SerializeField] private TextMeshProUGUI puzzleHeader;
    [SerializeField] private TextMeshProUGUI puzzleStats;
    [SerializeField] private TextMeshProUGUI timeHeader;
    [SerializeField] private TextMeshProUGUI timeStats;

    [SerializeField]
    [Tooltip("Drag the parent Ending Panel here")]
    private GameObject endingPanel;


    [SerializeField]
    [Tooltip("Drag the Main Menu Button here")]
    private GameObject mainMenuButton;
    private void Start()
    {
        if (endingPanel != null) endingPanel.SetActive(false);
        if (mainMenuButton != null) mainMenuButton.SetActive(false);
        // 1. Make sure all text is completely invisible when the scene starts
        SetTextAlpha(gameCompleteHeader, 0);
        SetTextAlpha(puzzleHeader, 0);
        SetTextAlpha(puzzleStats, 0);
        SetTextAlpha(timeHeader, 0);
        SetTextAlpha(timeStats, 0);
    }

    /// <summary>
    /// Call this function from your final puzzle/door to start the ending!
    /// </summary>
    public void TriggerEnding()
    {
        StartCoroutine(CinematicRoutine());
    }

    private IEnumerator CinematicRoutine()
    {
        // 1. DISABLE PLAYER INPUTS
        // This assumes you are using the InputManager we set up earlier!
        if (InputManager.Instance != null)
        {
            InputManager.Instance.controls.Disable();

            // Unlock the cursor so they can eventually click a "Main Menu" button
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // 2. FADE SCREEN TO BLACK
        if (screenFader != null)
        {
             screenFader.FadeToBlack();
        }

        // Wait an extra second in the dark for dramatic effect
        yield return new WaitForSeconds(1f);

        // 3. PREPARE THE DATA
        PrepareStats();
        if (endingPanel != null) endingPanel.SetActive(true);
        // 4. FADE IN TEXT ONE BY ONE

        // "GAME COMPLETE"
        yield return FadeTextIn(gameCompleteHeader);
        yield return new WaitForSeconds(timeBetweenText);

        // "PUZZLE COMPLETION" & "18 / 18"
        yield return FadeTextIn(puzzleHeader);
        yield return FadeTextIn(puzzleStats);
        yield return new WaitForSeconds(timeBetweenText);

        // "TIME SPENT" & "00h 00m 00s"
        yield return FadeTextIn(timeHeader);
        yield return FadeTextIn(timeStats);

        if (mainMenuButton != null)
        {
            mainMenuButton.SetActive(true);
        }
    }

    private void PrepareStats()
    {
        if (SaveManager.Instance == null || SaveManager.Instance.currentData == null) return;

        // --- FORMAT TIME (00h 00m 00s) ---
        float finalTime = SaveManager.Instance.currentData.totalPlayTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(finalTime);

        timeStats.text = string.Format("{0:D2}h {1:D2}m {2:D2}s",
            (int)timeSpan.TotalHours,
            timeSpan.Minutes,
            timeSpan.Seconds);

        // --- CALCULATE PUZZLES ---
        int solvedCount = CalculateSolvedPuzzles();
        puzzleStats.text = $"{solvedCount} / 18";
    }

    // --- HELPER METHODS FOR FADING TEXT ---

    private void SetTextAlpha(TextMeshProUGUI textElement, float alpha)
    {
        if (textElement == null) return;
        Color c = textElement.color;
        c.a = alpha;
        textElement.color = c;
    }

    private IEnumerator FadeTextIn(TextMeshProUGUI textElement)
    {
        if (textElement == null) yield break;

        Color c = textElement.color;
        while (c.a < 1f)
        {
            c.a += Time.deltaTime * textFadeSpeed;
            textElement.color = c;
            yield return null;
        }

        // Ensure it hits exactly 1 at the end
        c.a = 1f;
        textElement.color = c;
    }

    private int CalculateSolvedPuzzles()
    {
        int count = 0;
        PuzzleProgressData progress = SaveManager.Instance.currentData.puzzleProgress;

        if (progress.jumpBait.isSolved) count++;
        if (progress.invisibleBridge.isSolved) count++;
        if (progress.captcha.isSolved) count++;
        if (progress.combinationEast.isSolved) count++;
        if (progress.combinationWest.isSolved) count++;
        if (progress.combinationNorth.isSolved) count++;
        if (progress.pathMemory.isSolved) count++;
        if (progress.volumeMemory.isSolved) count++;
        if (progress.stepSoundMemory.isSolved) count++;
        if (progress.hangmanPuzzle.isSolved) count++;
        if (progress.fillInTheHole.isSolved) count++;
        if (progress.maze1Puzzle.isSolved) count++;
        if (progress.maze2Puzzle.isSolved) count++;
        if (progress.abcPuzzle.isSolved) count++;
        if (progress.timeButtonPuzzle.isSolved) count++;
        if (progress.invisibleBridge2.isSolved) count++;
        if (progress.npcPuzzle.isSolved) count++;
        if (progress.terrainPuzzle.isSolved) count++;

        return count;
    }
}
