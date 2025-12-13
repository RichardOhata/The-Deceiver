using System.Collections.Generic;
using UnityEngine;

public class InputCombination : Puzzle
{
    public enum PuzzleID
    {
        West,
        East,
        North,
    }

    [SerializeField] public PuzzleID instanceID;

    [SerializeField] public GameObject[] combinationComponents;

    [SerializeField] public GameObject currentComponent;

    public List<int> correctCombination;

    [SerializeField] private GameObject rightArrow;
    [SerializeField] private GameObject downArrow;

    public event System.Action<InputCombination> OnComponentRotated;
    public override void StartPuzzle()
    {
        base.StartPuzzle();
       
    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (instanceID)
        {
            case PuzzleID.West:
                correctCombination = new List<int> { 6, 2, 7, 4 };
                SetCombination(new List<(int, int)>
                {
                    (0, 6),
                    (2, 7)
                });
                break;
            case PuzzleID.East:
                correctCombination = new List<int> { 1, 3, 7, 8 };
                break;
            case PuzzleID.North:
                correctCombination = new List<int> { 2, 1, 4, 3 };
                break;

        }
    }

    /// <summary>
    /// Set specific combination values using index-value pairs.
    /// Example usage:
    /// SetCombination(new List<(int, int)> { (3, 5), (1, 2) });
    /// </summary>
    public void SetCombination(List<(int index, int value)> indexValuePairs)
    {
        foreach (var (index, value) in indexValuePairs)
        {
            if (index < 0 || index >= combinationComponents.Length)
            {
                Debug.LogWarning($"Invalid index {index} in SetCombination");
                continue;
            }

            var component = combinationComponents[index];
            if (component == null) continue;

            var cylinder = component.GetComponent<CombinationCylinder>();
            if (cylinder != null)
            {
                cylinder.SetCurrentValue(value);
            }
            else
            {
                Debug.LogWarning($"{component.name} has no CombinationCylinder attached!");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3f)) // 3f = ray distance
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject == rightArrow)
                {
                
                    RotateCombinationComponent();
                }
                else if (hitObject == downArrow)
                {
                  
                    MoveNextComponent();
                }
            }
        }
    }

    // Moves horizontally to the next component
    private void RotateCombinationComponent()
    {
       
        currentComponent.GetComponent<CombinationCylinder>().Rotate();
        OnComponentRotated?.Invoke(this); // Executes different functions after it rotates a combianation component
        CheckCombination();
    }


    // Moves vertically to the next component
    private void MoveNextComponent()
    {
        currentComponent.GetComponent<Outline>().enabled = false;
        int currentIndex = System.Array.IndexOf(combinationComponents, currentComponent);
        currentIndex = (currentIndex + 1) % combinationComponents.Length;
        currentComponent = combinationComponents[currentIndex];
        currentComponent.GetComponent<Outline>().enabled = true;
    }

    /// <summary>
    /// Returns a list of the current values from all combination components.
    /// </summary>
    public List<int> GetCurrentCombination()
    {
        List<int> currentValues = new List<int>();

        foreach (GameObject component in combinationComponents)
        {
            if (component == null)
            {
                currentValues.Add(-1); // or skip, depending on preference
                continue;
            }

            CombinationCylinder cylinder = component.GetComponent<CombinationCylinder>();
            if (cylinder != null)
            {
                currentValues.Add(cylinder.GetCurrentValue());
            }
            else
            {
                Debug.LogWarning($"{component.name} has no CombinationCylinder attached!");
                currentValues.Add(-1); // placeholder if missing
            }
        }

        return currentValues;
    }

    private void CheckCombination()
    {
        List<int> playerInput = GetCurrentCombination();

        bool isSolved = false;
        switch (instanceID)
        {
            case PuzzleID.West:
                // Exact match
                isSolved = IsExactMatch(playerInput, correctCombination);
                break;

            case PuzzleID.North:
                isSolved = IsExactMatch(playerInput, correctCombination);
                break;

            case PuzzleID.East:
                // Any permutation (unordered match)
                isSolved = IsPermutation(playerInput, correctCombination);
                break;

            default:
                break;
        }

        if (isSolved)
        {
            SolvePuzzle();
            enabled = false;
        }
    }


    private bool IsExactMatch(List<int> playerInput, List<int> correctCombination)
    {

        int combinedPlayerInput = int.Parse(string.Join("", playerInput));
        int combinedCorrectCombo = int.Parse(string.Join("", correctCombination));

        if (combinedPlayerInput == combinedCorrectCombo)
        {
            return true;  
          
        }
        return false;
    }

    private bool IsAscending(List<int> values)
    {
        for (int i = 1; i < values.Count; i++)
        {
            if (values[i] <= values[i - 1])
                return false;
        }
        return true;
    }

    private bool IsPermutation(List<int> playerInput, List<int> correct)
    {
        if (playerInput.Count != correct.Count)
            return false;

        // Copy lists so we don't mutate originals
        var tempInput = new List<int>(playerInput);
        var tempCorrect = new List<int>(correct);

        // Sort both and compare
        tempInput.Sort();
        tempCorrect.Sort();

        for (int i = 0; i < tempInput.Count; i++)
        {
            if (tempInput[i] != tempCorrect[i])
                return false;
        }
        return true;
    }


}

