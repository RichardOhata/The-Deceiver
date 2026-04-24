using UnityEngine;

public class SkyboxTrigger : MonoBehaviour
{
    [Header("Skybox Settings")]
    [SerializeField] private Material newSkyboxMaterial;
    [SerializeField] private bool revertOnExit = true;

    private Material originalSkybox;

    private void Awake()
    {
        // Store the starting skybox so we can go back to it
        originalSkybox = RenderSettings.skybox;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Use your "Player" tag to ensure NPCs don't trigger the sky change
        if (other.CompareTag("Player"))
        {
            ChangeSkybox(newSkyboxMaterial);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (revertOnExit && other.CompareTag("Player"))
        {
            ChangeSkybox(originalSkybox);
        }
    }

    private void ChangeSkybox(Material skyMaterial)
    {
        if (skyMaterial != null)
        {
            RenderSettings.skybox = skyMaterial;

            // This line is crucial! It tells Unity to update the ambient lighting 
            // and reflections to match the new sky colors.
            DynamicGI.UpdateEnvironment();

            Debug.Log($"Skybox changed to: {skyMaterial.name}");
        }
    }
}
