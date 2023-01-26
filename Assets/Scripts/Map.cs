using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Wayway.Engine;
using Wayway.Engine.Singleton;

public class Map : MonoSingleton<Map>
{
    [SerializeField] private Transform backGround;
    [SerializeField] private List<GameObject> mapList;
    [SerializeField] private Camera cam;

    public Tilemap tilemap;
    public GameConfig gameConfig;
    public NeighborPos neighborPos;
    
    public List<Vector3> canSpwanPlace = new List<Vector3>();
    public Dictionary<Vector3Int, Block> BlockPlace = new Dictionary<Vector3Int, Block>();

    protected override void Awake()
    {
        var newMap = Instantiate(mapList[gameConfig.StageLevel], transform.position, Quaternion.identity);
        var presentTile = newMap.GetComponentInChildren<Tilemap>();
        tilemap = presentTile;
    }

    void Start()
    {
        FindCanPutTile();
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

    public void DeleteBlock(Vector3Int clickPos)
    {
        Destroy(BlockPlace[clickPos].gameObject);
        BlockPlace[clickPos] = null;
        Debug.Log(BlockPlace.Count);
    }

    /*private bool IsSameValue(Block block)
    {
        //if(block.value == 
    }*/
    
    public List<Block> FindAllNearSameValue(Block block)
    {
        var toSearch = new List<Block>();
        var searched = new List<Block>();
        var allSameBlocks = new List<Block>();
        toSearch.Add(block);
        allSameBlocks.Add(block);
        var tempCount = 0;
        while (!toSearch.IsNullOrEmpty())
        {
            var currSearchTarget = toSearch[0];
            var sameBlocks = FindNearSameValue(currSearchTarget);
            if (!sameBlocks.IsNullOrEmpty())
            {
                for (var i = 0; i < sameBlocks.Count; i++)
                {
                    var currSameBlock = sameBlocks[i];
                    if (!searched.Contains(currSameBlock) && !toSearch.Contains(currSameBlock))
                    {
                        allSameBlocks.Add(currSameBlock);
                        toSearch.Add(currSameBlock);
                    }
                }
            }
            searched.Add(currSearchTarget);
            toSearch.Remove(currSearchTarget);

            if(500 < tempCount) break;
            tempCount++;
        }
        return allSameBlocks;
    }
    
    public List<Block> FindNearSameValue(Block block)
    {
        List<Block> sameBlockList = new List<Block>();
        for (int i = 0; i < neighborPos.neighbor.Count; i++)
        {
            var neighbor = block.Coord + neighborPos.neighbor[i].neighborPos;
            var tilePos = Util.CubeToUnityCell(neighbor);
            if (tilemap.HasTile(tilePos))
            {
                var neiborValue = BlockPlace[neighbor].value;
                if (neiborValue != null && neiborValue == block.value)
                {
                    sameBlockList.Add(BlockPlace[neighbor]);
                    Debug.Log(neighbor);
                }
            }
        }
        return sameBlockList;
    }
    
}
