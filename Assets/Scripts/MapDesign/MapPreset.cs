
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
    public List<Vector3> SpawnPlaceList;
    public Dictionary<Vector3Int, Vector3> BackGroundPositions = new ();
    public Dictionary<Vector3Int, Vector3> IceBlockerPositions = new ();
    public Dictionary<Vector3Int, Vector3> WoodBlockerPositions = new ();

    private Grid grid => transform.GetComponent<Grid>();

    [Button(ButtonSizes.Gigantic)]
    private void CreateMapPrefab()
    {
        CheckBackGround();
        CheckMapLayer();
        var Map = gameObject;
        PrefabUtility.SaveAsPrefabAsset(Map, "Assets/Prefabs/Prefabs.prefab");
    }
    
    [Button(ButtonSizes.Gigantic)]
    private void Initialize()
    {
        SpawnPlaceList = new List<Vector3>();
        BackGroundPositions = new Dictionary<Vector3Int, Vector3>();
        IceBlockerPositions = new Dictionary<Vector3Int, Vector3>();
        WoodBlockerPositions = new Dictionary<Vector3Int, Vector3>();
    }

    private void CheckBackGround()
    {
        var Tilemap = BackgroundMap.transform.GetComponent<Tilemap>();
        CalculateTiledPos(Tilemap, BackGroundPositions);
        BackGroundPositions.Values.ForEach(value =>
        {
            SpawnPlaceList.Add(value);
        });
    }
    
    // 얼음의 value 값일 경우는 
    // 특수 장애물 을 추가하는 경우?
    // checkMapLayer에 조건 추가, 딕셔너리 한개 추가;
    
    private void CheckMapLayer()
    {
        if (MapLayerList.Count != 0)
        {
            for (var i = 0; i < MapLayerList.Count; i++)
            {
                var tileMap = MapLayerList[i].GetComponent<Tilemap>();
                if (MapLayerList[i].value == 66)
                {
                    CalculateTiledPos(tileMap, WoodBlockerPositions);
                }
                else if (MapLayerList[i].value == 77)
                {
                    CalculateTiledPos(tileMap, IceBlockerPositions);
                }
                
            }
        }
    }

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
