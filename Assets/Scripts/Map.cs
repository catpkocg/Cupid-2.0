using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Wayway.Engine.Singleton;

public class Map : MonoSingleton<Map>
{
    [SerializeField] private Transform backGround;
    [SerializeField] private List<Tilemap> tilemapList;
    [SerializeField] private Camera cam;

    public Tilemap tilemap;
    public GameConfig gameConfig;
    
    public List<Vector3> canSpwanPlace = new List<Vector3>();
    public Dictionary<Vector3Int, Block> BlockPlace = new Dictionary<Vector3Int, Block>();
    private void Awake()
    {
        var tile = Instantiate(tilemapList[gameConfig.StageLevel], transform.position, Quaternion.identity);
        tile.transform.SetParent(backGround);
        tilemap = tile;
        FindCanPutTile();
    }

    void Start()
    {
        
    }

    private void FindCanPutTile()
    {
        for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++)
        {
            for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = new Vector3Int(n, p, 0);
                Vector3 place = tilemap.CellToWorld(localPlace);
                var putPlace = new Vector3(place.x, place.y, 0);
                if (tilemap.HasTile(localPlace))
                {
                    canSpwanPlace.Add(putPlace);
                }
            }
        }
        
        cam.transform.GetComponent<Camera>().orthographicSize = tilemap.cellBounds.xMax - tilemap.cellBounds.xMin;
    }
}
