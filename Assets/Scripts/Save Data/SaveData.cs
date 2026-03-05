using UnityEngine;


[System.Serializable]
public class PlayerPositionData
{
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
}


[System.Serializable]
public class JumpBaitPuzzleData
{
    public bool isSolved = false;
}


[System.Serializable]
public class InvisibleBridgePuzzleData
{
    public bool isSolved = false;
}

[System.Serializable]
public class CaptchaPuzzleData
{
    public bool isSolved = false;
    public int stage = 1;
    public bool isDoorOpened = false;
    public bool isConsoleDestroyed = false;
}

[System.Serializable]
public class CombinationWestPuzzleData
{
    public bool isSolved = false;
}

[System.Serializable]
public class CombinationEastPuzzleData
{
    public bool isSolved = false;
}

[System.Serializable]
public class CombinationNorthPuzzleData
{
    public bool isSolved = false;
}

[System.Serializable]
public class PuzzleProgressData
{
    public JumpBaitPuzzleData jumpBait = new JumpBaitPuzzleData();
    public InvisibleBridgePuzzleData invisibleBridge = new InvisibleBridgePuzzleData();
    public CaptchaPuzzleData captcha = new CaptchaPuzzleData();
    public CombinationEastPuzzleData combinationEast = new CombinationEastPuzzleData();
    public CombinationWestPuzzleData combinationWest = new CombinationWestPuzzleData();
    public CombinationNorthPuzzleData combinationNorth = new CombinationNorthPuzzleData();
}

public interface ISeasonalData
{
    int puzzleProgress { get; set; }
    bool areaComplete { get; set; }
}

[System.Serializable]
public class SummerEnvironmentData : ISeasonalData
{
    [SerializeField] private int _puzzleProgress = 0;
    [SerializeField] private bool _areaComplete = false;

    // Interface implementation
    public int puzzleProgress { get => _puzzleProgress; set => _puzzleProgress = value; }
    public bool areaComplete { get => _areaComplete; set => _areaComplete = value; }

    // Unique to Summer
    public bool hideLabyrinth = false;
}

[System.Serializable] 
public class AutumnEnvironmentData : ISeasonalData
{
    [SerializeField] private int _puzzleProgress = 0;
    [SerializeField] private bool _areaComplete = false;
    public int puzzleProgress { get => _puzzleProgress; set => _puzzleProgress = value; }
    public bool areaComplete { get => _areaComplete; set => _areaComplete = value; }
}

[System.Serializable] 
public class WinterEnvironmentData : ISeasonalData
{
    [SerializeField] private int _puzzleProgress = 0;
    [SerializeField] private bool _areaComplete = false;
    public int puzzleProgress { get => _puzzleProgress; set => _puzzleProgress = value; }
    public bool areaComplete { get => _areaComplete; set => _areaComplete = value; }
}

[System.Serializable]
public class SeasonalEnvironmentData
{
    public SummerEnvironmentData summerEnvironment = new SummerEnvironmentData();
    public AutumnEnvironmentData autumnEnvironment = new AutumnEnvironmentData();
    public WinterEnvironmentData winterEnvironment = new WinterEnvironmentData();
}

[System.Serializable]
public class SaveData
{
    public PlayerPositionData playerPosition = new PlayerPositionData();
    public PuzzleProgressData puzzleProgress = new PuzzleProgressData();
    public SeasonalEnvironmentData environmentData = new SeasonalEnvironmentData();
}
