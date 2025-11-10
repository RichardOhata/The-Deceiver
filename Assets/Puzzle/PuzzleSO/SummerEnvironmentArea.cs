using NUnit.Framework;
using System;
using UnityEngine;

public class SummerEnvironmentArea : Puzzle
{
    [SerializeField] private InputCombination[] inputCombinationPuzzles;
    [SerializeField] private GameObject[] flowers;
    [SerializeField] private GameObject[] terrainObjects;
    [SerializeField] private GameObject[] notablePoints;
    [SerializeField] private GameObject labyrinth;
    [SerializeField] private bool labyrinthHidden = false;
    [SerializeField] private bool allowHiddenLabyrinth = false;
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
    }

    private void Update()
    {
        if (labyrinthHidden || !allowHiddenLabyrinth)
            return;
        if (LookAtUtility.IsLookingAtAny(GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>().transform, notablePoints, maxAngle: 15f, minDistance: 1f))
        {
            labyrinth.SetActive(false);
            labyrinthHidden = true;
        }
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

    public void AllowHiddenLabyrinth()
    {
        allowHiddenLabyrinth = true;
    }

}
