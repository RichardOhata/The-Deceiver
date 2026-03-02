using TMPro;
using UnityEngine;

public class ABCUI : MonoBehaviour
{
    [SerializeField] private float initialTime = 5.0f;
    [SerializeField] public float timer;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TMP_InputField userInputField;

    private string answer = "abcdefghijklmnopqrstuvwxyz";

    private void OnEnable()
    {
        userInputField.text = string.Empty;
        timer = initialTime;
        UpdateTimerText();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = initialTime;
            userInputField.text = string.Empty;
        }
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        timerText.text = "Type the Alphabet in " + timer.ToString("F2") + " seconds";
    }

    public void ValidateAnswer()
    {
        if (userInputField.text.ToLower() == answer)
        {
            StartCoroutine(DelayedAction(1.0f, () =>
            {
                GameObject.FindGameObjectWithTag("ABC Puzzle")
                    .GetComponent<ABCPuzzle>()
                    .SolvePuzzle();
            }));
        }
        else
        {
            userInputField.text = string.Empty;
        }
    }

    private System.Collections.IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action?.Invoke();
    }
}
