using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;

public class HangmanUI : MonoBehaviour
{
    [SerializeField]
    private string answer;

    [SerializeField]
    private string playerName;

    [SerializeField]
    private char[] currentProgress;

    [SerializeField]
    private TextMeshProUGUI currentProgressText;

    [SerializeField]
    private TMP_InputField userInputTextField;

    [SerializeField]
    private TextMeshProUGUI errorText;

    [SerializeField]
    private int maxIncorrectCount = 3;

    [SerializeField]
    private int currentIncorrectCount = 0;

    [SerializeField]
    private string[] answerList = {"Hello", "Welcome", "Player", "Are you", "I see you", "Behind you"};

    public void PopulateAnswer(string answer)
    {
        playerName = answer;
        PickRandomAnswer();
        currentProgress = new char[this.answer.Length];
        ResetCurrentProgress();
    }

    private void PickRandomAnswer()
    {
        if (answerList.Length > 0)
        {
            int randomIndex = Random.Range(0, answerList.Length);
            string randomPrefix = answerList[randomIndex];
            answer = randomPrefix + " " + playerName;

            currentProgress = new char[this.answer.Length];

        }
    }
        private void ResetCurrentProgress()
    {
        for (int i = 0; i < this.answer.Length; i++)
        {
            if (this.answer[i] == ' ')
            {
                currentProgress[i] = ' '; // Show spaces immediately
            }
            else
            {
                currentProgress[i] = '_'; // Hide letters
            }
        }
        UpdateText();
    }

    public void CheckInputChar()
    {
        string input = userInputTextField.text;
        char inputChar = input.ToCharArray()[0];
        if (inputChar == ' ') return;
        bool correctGuess = false;

        if (!currentProgress.Contains(inputChar))
        {
            for (int index = 0; index < answer.Length; index++)
            {
                if (answer[index] == char.ToUpper(inputChar))
                {
                    currentProgress[index] = char.ToUpper(inputChar);
                    correctGuess = true;
                }
                else if (answer[index] == char.ToLower(inputChar))
                {
                    currentProgress[index] = char.ToLower(inputChar);
                    correctGuess = true;
                }
            }
        }
      

        if (!correctGuess)
        {
            currentIncorrectCount++;
            errorText.text += "X";
            if (currentIncorrectCount >= maxIncorrectCount)
            {
                errorText.text = "";
                PickRandomAnswer();
                ResetCurrentProgress();
                currentIncorrectCount = 0;
            }
        }
        userInputTextField.text = "";
        UpdateText();
    }

    private void UpdateText()
    {
        currentProgressText.text = string.Join(" ", currentProgress);
        CheckStatus();
    }

    private void CheckStatus()
    {
        for (int i = 0; i < currentProgress.Length; i++)
        {
            if (currentProgress[i] == '_')
            {
                return;
            }
        }

        StartCoroutine(DelayedAction(1.0f, () =>
        {
            gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("Hangman Puzzle")
                .GetComponent<Hangman>()
                .SolvePuzzle();
        }));
    }

    private System.Collections.IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action?.Invoke();
    }
}
