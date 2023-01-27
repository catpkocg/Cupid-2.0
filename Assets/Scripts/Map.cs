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

    public List<Vector3Int> newBlockSpawnPos;

    protected override void Awake()
    {
        var newMap = Instantiate(mapList[gameConfig.StageLevel], transform.position, Quaternion.identity);
        var presentTile = newMap.GetComponentInChildren<Tilemap>();
        tilemap = presentTile;
        newBlockSpawnPos = new List<Vector3Int>()
        {
        new Vector3Int(-3, -1, 4), new Vector3Int(-2, -2, 4), new Vector3Int(-1, -3, 4),
        new Vector3Int(0, -4, 4), new Vector3Int(1, -4, 3), new Vector3Int(2, -4, 2),
        new Vector3Int(3, -4, 1)

        };
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

        cam.transform.GetComponent<Camera>().orthographicSize = 6f; //tilemap.cellBounds.xMax - tilemap.cellBounds.xMin;
    }

    public void DeleteBlockList(List<Block> sameBlockList)
    {
        for (int i = 0; i < sameBlockList.Count; i++)
        {
            DeleteBlock(sameBlockList[i].Coord);
        }
    }

    public void DeleteBlock(Vector3Int clickPos)
    {
        Destroy(BlockPlace[clickPos].gameObject);
        BlockPlace[clickPos] = null;
        Debug.Log(BlockPlace.Count);
    }

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
            if (tilemap.HasTile(tilePos) && BlockPlace[neighbor] != null)
            {
                var neiborValue = BlockPlace[neighbor].value;
                if (neiborValue == block.value)
                {
                    sameBlockList.Add(BlockPlace[neighbor]);
                }
            }
        }
        return sameBlockList;
    }

    public int CountNullPlace(Vector3Int wantToCheckPos)
    {
        var nullCount = 0;
        var targetPos = wantToCheckPos + new Vector3Int(0, 1, -1);
        var tilePos = Util.CubeToUnityCell(targetPos);
        //Debug.Log(targetPos);
        while (tilemap.HasTile(tilePos))
        {
            //Debug.Log(targetPos);
            if (BlockPlace[targetPos] == null)
            {
                nullCount++;
            }
            targetPos += new Vector3Int(0, 1, -1);
            tilePos = Util.CubeToUnityCell(targetPos);
        }
        return nullCount;
    }
    
    
    
}
