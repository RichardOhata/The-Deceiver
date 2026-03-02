using TMPro;
using UnityEngine;

public class TerrainPuzzleUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField summerInput;
    [SerializeField] private TMP_InputField autumnInput;
    [SerializeField] private TMP_InputField winterInput;

    private string answer = "key";

    private TerrainPuzzle puzzleGO;

    private void Awake()
    {
        puzzleGO = GameObject.FindGameObjectWithTag("Terrain Puzzle")
            .GetComponent<TerrainPuzzle>();
    }

    // Update is called once per frame
    void Update()
    { 
       puzzleGO.UpdateTerrainWallText(summerInput.text, autumnInput.text, winterInput.text);
    }

    public void ValidateAnswer()
    {
        if (summerInput.text.ToLower() == answer[0].ToString() && autumnInput.text.ToLower() == answer[1].ToString() && winterInput.text.ToLower() == answer[2].ToString())
        {
            StartCoroutine(DelayedAction(1.0f, () =>
            {
                puzzleGO.SolvePuzzle();
            }));
        }
       
    }

    private System.Collections.IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action?.Invoke();
    }
}
