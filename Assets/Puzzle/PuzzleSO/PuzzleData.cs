using UnityEngine;

public enum PuzzleType
{
    Hangman,
    Lock,
    Memory,
    Other
} //To be implemented

[CreateAssetMenu(fileName = "PuzzleData", menuName = "Scriptable Objects/PuzzleData")]
public class PuzzleData : ScriptableObject
{
    public string puzzleName;
    public PuzzleType puzzleType;
    public string hint;
    public bool isSolved = false;

}
