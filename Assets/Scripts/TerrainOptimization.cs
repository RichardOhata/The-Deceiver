using System.Collections.Generic;
using UnityEngine;

public class TerrainOptimization : MonoBehaviour
{

    private class TerrainComponents
    {
        public Terrain Terrain;
        public TerrainCollider Collider;
    }

    [SerializeField] private GameObject[] terrainObjects;
    private List<TerrainComponents> cachedComponents = new List<TerrainComponents>();

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
