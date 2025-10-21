using UnityEngine;

public class InvisibleBridgePuzzle : Puzzle
{
    [SerializeField] private Collider invisibleCollider;
    [SerializeField] private GameObject backwardsAnchor;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float backwardsAngleThreshold = 90f;

    public override void StartPuzzle()
    {
        base.StartPuzzle();
    }
    public override void SolvePuzzle()
    {
        base.SolvePuzzle();
        puzzleData.isSolved = true;
    }

    private void Update()
    {
        if (IsLookingBackwards())
            invisibleCollider.isTrigger = false;
        else
            invisibleCollider.isTrigger = true;
    }

    private bool IsLookingBackwards()
    {
        Vector3 toanchor = (backwardsAnchor.transform.position - playerCamera.position).normalized;
        Vector3 forward = playerCamera.forward;
        float angle = Vector3.Angle(forward, toanchor);
        return angle < backwardsAngleThreshold;
    }
}
