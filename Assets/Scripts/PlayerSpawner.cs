using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private void Start()
    {
        // 1. Double-check that the SaveManager exists to prevent errors
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager is missing from the scene!");
            return;
        }

        // 2. Dig into the nested data to get your saved coordinates
        float savedX = SaveManager.Instance.currentData.playerPosition.x;
        float savedY = SaveManager.Instance.currentData.playerPosition.y;
        float savedZ = SaveManager.Instance.currentData.playerPosition.z;

        // Rebuild the Vector3 position
        Vector3 spawnPosition = new Vector3(savedX, savedY, savedZ);

        // 3. Prepare the Rigidbody for a sudden teleport
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // 4. Move the player and force the physics engine to sync
        transform.position = spawnPosition;
        Physics.SyncTransforms();

        Debug.Log("Player successfully spawned at saved checkpoint: " + spawnPosition);
    }
}
