using UnityEngine;

[DefaultExecutionOrder(-100)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public InputSystem_Actions controls { get; private set; }
    [SerializeField] private GameObject[] terrainObjects;
    private void Awake()
    {
        SetTerrainActive(false);
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        controls = new InputSystem_Actions();
        controls.Enable();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Move Later
    public void SetTerrainActive(bool isActive)
    {
        foreach (var t in terrainObjects)
        {
            var terr = t.GetComponent<Terrain>();
            if (terr != null)
            {
                terr.drawHeightmap = isActive;
                terr.drawTreesAndFoliage = isActive;
            }

            var collider = t.GetComponent<TerrainCollider>();
            if (collider != null)
                collider.enabled = isActive;
        }
    }
}
