using UnityEngine;

public class InvisibleBridgePuzzle : Puzzle
{

    [SerializeField] private GameObject eyeSymbol;
    [SerializeField] private GameObject invisibleBridge;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float maxAngle = 10f;
    [SerializeField] private bool isActive = false;
    public override void StartPuzzle()
    {
        base.StartPuzzle();
        isActive = true;
    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
    }

    private void Update()
    {
          if (IsLookingAtTarget() && isActive)
        {
            invisibleBridge.SetActive(true);
        } else
        {
            invisibleBridge.SetActive(false);
        }
    }

    private bool IsLookingAtTarget()
    {
        Vector3 directionToTarget = (eyeSymbol.transform.position - playerCamera.position).normalized;
        float angle = Vector3.Angle(playerCamera.forward, directionToTarget);
        return angle <= maxAngle;
    }
}
