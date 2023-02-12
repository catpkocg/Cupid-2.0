using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [TitleGroup("Prefabs")]
    [SerializeField] private Map mapDesignTemplate;
    [SerializeField] private GameObject mapTilePrefab;
    [SerializeField] private List<Block> blockerPrefabList;
    
    [TitleGroup("StageNumber")]
    [SerializeField] private int setStageNumber;

    [Button(ButtonSizes.Gigantic), GUIColor(0.2f, 1f, 0.2f)]
    public void GenerateMap()
    {
        // 인스턴스 한다
        var temPlate = Instantiate(mapDesignTemplate, new Vector3(0, 0, 0), Quaternion.identity);
        // 맵의 범위를 설정하고 배경오브젝트를 깐다.
        GenerateFromPreset(transform.GetComponent<MapPreset>(), temPlate.transform);
        // 저장
        PrefabUtility.SaveAsPrefabAsset(temPlate.gameObject, "Assets/Prefabs/Maps/Map"+setStageNumber+".prefab");
        //삭제
        DestroyImmediate(temPlate.gameObject);
    }
    public void GenerateFromPreset(MapPreset mapPreset, Transform temPlate)
    {
        var backGroundTileMap = mapPreset.BackgroundMap.GetComponent<Tilemap>();
        SettingMapTile(backGroundTileMap, mapTilePrefab, temPlate);
        
        var otherTileMapList = mapPreset.MapLayerList;
        if (otherTileMapList.Count != 0)
        { 
            for (var i = 0; i < otherTileMapList.Count; i++)
            {
                var tileMap = otherTileMapList[i].GetComponent<Tilemap>();
                SettingObjectOnTile(tileMap, blockerPrefabList[i], temPlate);
            }
        }
        var spawnPlace = mapPreset.SpawnPlace.GetComponent<Tilemap>();
        SettingSpawnPlace(spawnPlace);
    }
    private void SettingMapTile(Tilemap tilemap, GameObject mapTile, Transform temPlate)
    {
        for (var i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
        {
            for (var j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
            {
                var currTileMapCoord = new Vector3Int(i, j, 0);
                var currCoord = Util.UnityCellToCube(currTileMapCoord);
                
                var currPos = tilemap.CellToWorld(currTileMapCoord);
                currPos = new Vector3(currPos.x, currPos.y, 0);
                
                if (tilemap.HasTile(currTileMapCoord))
                {
                    Instantiate(mapTile, currPos, Quaternion.identity, temPlate.transform);
                }
            }
        }
    }
    private void SettingObjectOnTile(Tilemap tilemap, Block blocks, Transform temPlate)
    {
        for (var i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
        {
            for (var j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
            {
                // 맵의 범위를 설정한다.
                var currTileMapCoord = new Vector3Int(i, j, 0);
                var currCoord = Util.UnityCellToCube(currTileMapCoord);
                
                // 해당 좌표의 위치를 계산한다.
                var currPos = tilemap.CellToWorld(currTileMapCoord);
                currPos = new Vector3(currPos.x, currPos.y, 0);
                
                if (tilemap.HasTile(currTileMapCoord))
                {
                    Instantiate(blocks, currPos, Quaternion.identity, temPlate.transform);
                }
            }
        }
    }
    private void SettingSpawnPlace(Tilemap tilemap)
    {
        for (var i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
        {
            for (var j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
            {
                var currTileMapCoord = new Vector3Int(i, j, 0);
                var currCoord = Util.UnityCellToCube(currTileMapCoord);
                var currPos = tilemap.CellToWorld(currTileMapCoord);
                
                if (tilemap.HasTile(currTileMapCoord))
                {
                    mapDesignTemplate.SpawnPlace.Add(currPos);
                }
            }
        }
    }
    
    
    
}
