using System.Collections.Generic;
using UnityEngine;

public class TerrainOptimization : MonoBehaviour
{

    public class TerrainComponents
    {
        public Terrain Terrain;
        public TerrainCollider Collider;
    }

    [SerializeField] private GameObject[] terrainObjects;
    private List<TerrainComponents> cachedComponents = new List<TerrainComponents>();
    [SerializeField] private BoxCollider areaZone;
    private void Awake()
    {
        foreach (var t in terrainObjects)
        {
            cachedComponents.Add(new TerrainComponents
            {
                Terrain = t.GetComponent<Terrain>(),
                Collider = t.GetComponent<TerrainCollider>()
            });
        }

   
        SetTerrainActive(false);
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (areaZone.bounds.Contains(player.transform.position))
            {
                SetTerrainActive(true);
                Debug.Log("123");
            }
        }
    }

    public void SetTerrainActive(bool isActive)
    {
        foreach (var components in cachedComponents)
        {
            if (components.Terrain != null)
            {
                components.Terrain.drawHeightmap = isActive;
                components.Terrain.drawTreesAndFoliage = isActive;
            }

            if (components.Collider != null)
            {
                components.Collider.enabled = isActive;
            }
        }
    }
}
