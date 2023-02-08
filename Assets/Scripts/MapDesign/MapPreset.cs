
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Wayway.Engine;

// BackGround Value 는 0 
// 나무상자는 66 값
// 얼음판때기는 77 값

public class MapPreset : MonoBehaviour
{
    
    public int StageNumber = 0;
    public MapLayer BackgroundMap;
    public List<MapLayer> MapLayerList = new();
    // Map information
    public List<Vector3Int> MapBounds;
    public Dictionary<Vector3Int, Block> MovableBlocks;
    public Dictionary<Vector3Int, Block> UnMovableBlocks;
    
    public Dictionary<Vector3Int, Block> allBlocks = new ();
    public Dictionary<Vector3Int, Blocker> Blockers= new ();
    
    
    [SerializeField] private MapDesignSpawn spawn;

    


    private Grid grid => transform.GetComponent<Grid>();

    [Button(ButtonSizes.Gigantic)]
    private void CreateMapPrefab()
    {
        MapBounds = new List<Vector3Int>();
        MovableBlocks = new Dictionary<Vector3Int, Block>();
        UnMovableBlocks = new Dictionary<Vector3Int, Block>();

        FindValidPosition(MapBounds);
        
        //Spawn On Tile? and Check Blocks Positions
        CreateMovableBlocks(MovableBlocks);
        CreateUnMovableBlocks(UnMovableBlocks);
        
        
        var Map = gameObject;
        PrefabUtility.SaveAsPrefabAsset(Map, "Assets/Prefabs/Prefabs.prefab");
    }
    

    private void Start()
    {
        spawn.SpawnOnTileMap();
    }


    private void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     var plane = new Plane();
        //     plane.Set3Points(Vector3.zero, Vector3.up, Vector3.right);
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     if (plane.Raycast(ray, out var enter))
        //     {
        //         Vector3 hitPoint = ray.GetPoint(enter);
        //         var cellCoord = grid.WorldToCell(hitPoint);
        //         var blockPos = Util.UnityCellToCube(cellCoord);
        //         
        //         
        //         
        //         
        //         //var clickBlock = Map.Instance.BlockPlace[blockPos];
        //         //var tilePos = Util.CubeToUnityCell(blockPos);
        //         //var putPos = grid.CellToWorld(tilePos);
        //
        //         
        //         
        //         //var block = clickBlock;
        //     }
        // }
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    var a = hit.collider.gameObject.transform.position;
                    var b = new Vector3Int((int)a.x, (int)a.y, (int)a.z);
                    var c = allBlocks[b];
                    
                    //var d = Blockers[b];
                    c.Pang();
                    //d.Pang();
                }
            }
        }
        
        
    }

    private void FindValidPosition(List<Vector3Int> mapBounds)
    {
        var tilemap = BackgroundMap.transform.GetComponent<Tilemap>();
    }

    private void CreateMovableBlocks(Dictionary<Vector3Int, Block> movableBlocks)
    {
        
    }

    private void FindMovableBlockPos()
    {
        
    }

    private void CreateUnMovableBlocks(Dictionary<Vector3Int, Block> unMovableBlocks)
    {
        
    }

    private void FindUnMovableBlockPos()
    {
        
    }
    
    
    // private void CheckBackGround()
    // {
    //     
    //     CalculateTiledPos(Tilemap, BackGroundPositions);
    //     BackGroundPositions.Values.ForEach(value =>
    //     {
    //         SpawnPlaceList.Add(value);
    //     });
    // }
    //
    // // 얼음의 value 값일 경우는 
    // // 특수 장애물 을 추가하는 경우?
    // // checkMapLayer에 조건 추가, 딕셔너리 한개 추가;
    //
    // private void CheckMapLayer()
    // {
    //     if (MapLayerList.Count != 0)
    //     {
    //         for (var i = 0; i < MapLayerList.Count; i++)
    //         {
    //             var tileMap = MapLayerList[i].GetComponent<Tilemap>();
    //             if (MapLayerList[i].value == 66)
    //             {
    //                 CalculateTiledPos(tileMap, WoodBlockerPositions);
    //             }
    //             else if (MapLayerList[i].value == 77)
    //             {
    //                 CalculateTiledPos(tileMap, IceBlockerPositions);
    //             }
    //             
    //         }
    //     }
    // }

    private void CalculateTiledPos(Tilemap tilemap, Dictionary<Vector3Int,Vector3> blockerPos)
    {
        for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++)
        {
            for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++)
            {
                var localPlace = new Vector3Int(n, p, 0);
                var cubeCoord = Util.UnityCellToCube(localPlace);
                var place = tilemap.CellToWorld(localPlace);
                var putPlace = new Vector3(place.x, place.y, 0);
                if (tilemap.HasTile(localPlace))
                {
                    blockerPos.Add(cubeCoord,putPlace);
                }
            }
        }
    }
    
}
