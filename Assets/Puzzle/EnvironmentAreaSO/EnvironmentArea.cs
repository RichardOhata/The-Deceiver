using UnityEngine;

public enum AreaID
{
    Summer,
    Autumn,
    Winter
}


[CreateAssetMenu(fileName = "EnvironmentArea", menuName = "Scriptable Objects/EnvironmentArea")]
public class EnvironmentArea : ScriptableObject
{
    [Header("Configuration")]

    public AreaID areaID;

    public int totalPuzzles;

    //public int totalManditoryPuzzles;

    public string GetProgressString(int currentSolved)
    {
        return $"{currentSolved}/{totalPuzzles}";
    }

    public bool IsAreaComplete(int currentSolved)
    {
        return currentSolved == totalPuzzles;
    }
}
