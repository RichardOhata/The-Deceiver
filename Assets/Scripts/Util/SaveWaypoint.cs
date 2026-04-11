using UnityEngine;

public class SaveWaypoint : MonoBehaviour
{
    public enum SpawnType { UseGameObject, UseManualCoordinates }

    [Header("Mode Selection")]
    public SpawnType spawnMode;

    [Header("Spawn Options")]
    [Tooltip("Drag the GameObject whose position you want to copy.")]
    public Transform spawnReferenceObject;

    [Tooltip("Manually type coordinates here if mode is set to Manual.")]
    public Vector3 manualCoordinates;

    private bool hasSaved = false;

    public void SetWaypoint()
    {
        if (SaveManager.Instance == null) return;
        if (hasSaved) return;
        Vector3 finalPosition;

        // Choose the position based on the dropdown selection
        if (spawnMode == SpawnType.UseGameObject && spawnReferenceObject != null)
        {
            finalPosition = spawnReferenceObject.position;
        }
        else
        {
            finalPosition = manualCoordinates;
        }
        hasSaved = true;
        // Apply to Save Data
        SaveManager.Instance.currentData.playerPosition.x = finalPosition.x;
        SaveManager.Instance.currentData.playerPosition.y = finalPosition.y;
        SaveManager.Instance.currentData.playerPosition.z = finalPosition.z;

        SaveManager.Instance.SaveGame();

        Debug.Log($"Checkpoint updated to {finalPosition} using {spawnMode} mode.");
    }

    // This helps you see the coordinates in the editor even when the game isn't running
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 previewPos = (spawnMode == SpawnType.UseGameObject && spawnReferenceObject != null)
                             ? spawnReferenceObject.position : manualCoordinates;

        Gizmos.DrawWireSphere(previewPos, 0.5f);
        Gizmos.DrawLine(transform.position, previewPos);
    }
}
