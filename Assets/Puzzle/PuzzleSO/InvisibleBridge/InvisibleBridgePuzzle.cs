using UnityEngine;

public class InvisibleBridgePuzzle : Puzzle
{

    [SerializeField] private GameObject eyeSymbol;
    [SerializeField] private GameObject invisibleBridge;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float maxAngle = 10f;

    [SerializeField] private Light eyeLight;
    private float maxIntensity = 1.0f;
    [SerializeField] private float intensitySpeed = 0.9f;

    private void Awake()
    {
        if (SaveManager.Instance != null)
        {
            isSolved = SaveManager.Instance.currentData.puzzleProgress.invisibleBridge.isSolved;
        }
    }
    public override void StartPuzzle()
    {
        base.StartPuzzle();
    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
    }

    public override void UpdatePuzzleStatus()
    {
        SaveManager.Instance.currentData.puzzleProgress.invisibleBridge.isSolved = true;
        SaveManager.Instance.SaveGame();
        Debug.Log("Checkpoint and Puzzle State Auto-Saved!");
    }

    private void Update()
    {
        bool looking = LookAtUtility.IsLookingAt(playerCamera, eyeSymbol.transform, maxAngle);
        invisibleBridge.SetActive(looking);
        LightUp(looking);
    }

    private void LightUp(bool increment)
    {
        float targetIntensity = increment ? maxIntensity : 0f;
        eyeLight.intensity = Mathf.MoveTowards(
            eyeLight.intensity,
            targetIntensity,
            intensitySpeed * Time.deltaTime
        );
    }

    private void OnDisable()
    {
        if (eyeLight != null)
        {
            eyeLight.intensity = 0f;
        }
    }
}
