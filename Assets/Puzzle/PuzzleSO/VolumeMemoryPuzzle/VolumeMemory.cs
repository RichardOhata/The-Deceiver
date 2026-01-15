using UnityEngine;

public class VolumeMemory : Puzzle
{
    [SerializeField]
    private GameObject fovRow;

    [SerializeField]
    private GameObject sensRow;

    [SerializeField]
    private GameObject volumeRow;

    public override void StartPuzzle()
    {
        base.StartPuzzle();

    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
    }


    private void Update()
    {
        UpdateRows(volumeRow, SettingsManager.Instance.volume);
        UpdateRows(sensRow, SettingsManager.Instance.sensitivity);
        UpdateRows(fovRow, Mathf.InverseLerp(0.1f, 100f, SettingsManager.Instance.fov));
    }
    
    private void UpdateRows(GameObject row, float percentage)
    {
        int childCount = row.transform.childCount;
        int amtToRender = Mathf.RoundToInt(percentage * childCount);
        
        for (int index = 0; index < childCount; index++)
        {
            if (index < amtToRender)
            {
                row.transform.GetChild(index).gameObject.SetActive(true);
            } else
            {
                row.transform.GetChild(index).gameObject.SetActive(false);
            }
         

        }
    }
}
