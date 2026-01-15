using UnityEngine;

public class CombinationCylinder : MonoBehaviour
{

    private int symbolCount = 8; 
    private float rotationStep = 45f;
    [SerializeField] private int currentValue = 1;

    public void Rotate()
    {
        currentValue = (currentValue % symbolCount) + 1;
        if (currentValue > symbolCount) currentValue = 1;
        UpdateRotation();
    }

    public void SetCurrentValue(int value)
    {
        currentValue = ((value - 1 + symbolCount) % symbolCount) + 1;
        UpdateRotation();
    }

    public int GetCurrentValue()
    {
        return currentValue;
    }

    private void UpdateRotation()
    {
        float newRotation = (currentValue - 1) * rotationStep;
        transform.localRotation = Quaternion.Euler(0, newRotation, 0);
    }
}
