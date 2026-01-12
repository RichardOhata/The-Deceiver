using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HangmanUI : MonoBehaviour
{
    [SerializeField]
    private string answer;

    [SerializeField]
    private char[] currentProgress;

    [SerializeField]
    private TextMeshProUGUI currentProgressText;

    [SerializeField]
    private TMP_InputField userInputTextField;

    public void PopulateAnswer(string answer)
    {
        this.answer = "Hello " + answer;
        currentProgress = new char[this.answer.Length];
        for (int i = 0; i < this.answer.Length; i++)
        {
            if (answer[i] == ' ')
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
        for (int index = 0; index < answer.Length; index++)
        {
           if (answer[index] == inputChar)
            {
                currentProgress[index] = inputChar;
            }
        }
        userInputTextField.text = "";
        UpdateText();
    }

    private void UpdateText()
    {
        currentProgressText.text = string.Join(" ", currentProgress);
    }

}
