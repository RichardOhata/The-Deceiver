using NUnit.Framework;
using System;
using UnityEngine;

public class MultiInputCombinationPuzzle : Puzzle
{
    [SerializeField] private InputCombination[] inputCombinationPuzzles;
    [SerializeField] private GameObject[] flowers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void StartPuzzle()
    {
        base.StartPuzzle();

    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
    }
    void Start()
    {
        foreach (InputCombination puzzle in inputCombinationPuzzles)
        {

            if (puzzle.instanceID == InputCombination.PuzzleID.West) {
                puzzle.OnComponentRotated += ChangeEnvironment;
            }
        }
        //puzzle.OnComponentRotated += CheckSolutionProximity;
   
    }

    private void ChangeEnvironment(InputCombination inputCombination)
    {
        int currentIndex = System.Array.IndexOf(inputCombination.combinationComponents, inputCombination.currentComponent);
 
        if (inputCombination.currentComponent.GetComponent<CombinationCylinder>().GetCurrentValue() == inputCombination.correctCombination[currentIndex])
        {
            flowers[currentIndex].SetActive(true);
        } else
        {
            flowers[currentIndex].SetActive(false);
        }
    }

    private void CheckSolutionProximity()
    {
        Debug.Log("Checking how close the combination is to the solution...");
    }

}
