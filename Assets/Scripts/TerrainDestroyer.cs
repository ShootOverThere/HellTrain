using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainDestroyer : MonoBehaviour
{
    public Tilemap terrain;
    
    public void DestroyTerrain(Vector3 explosionLocation, float radius)
    {
        for(float x = -radius/2f; x < radius/2f; x+=radius/4f)
        {
            for(float y = -radius/2f; y < radius/2f; y+=radius/4f)
            {
                Vector3Int tilePos = terrain.WorldToCell(explosionLocation + new Vector3(x,y,0));
                //Vector3Int tilePos = terrain.WorldToCell(explosionLocation);
                if(terrain.GetTile(tilePos) != null)
                {
                    DestroyTile(tilePos);
                }
            }
        }
    }

    void DestroyTile(Vector3Int tilePos)
    {
        terrain.SetTile(tilePos, null);        
    }
}
