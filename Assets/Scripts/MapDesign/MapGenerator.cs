using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Wayway.Engine;

public class MapGenerator : MonoBehaviour
{
    [TitleGroup("Prefabs")]
    [SerializeField] private Map mapDesignTemplate;
    [SerializeField] private MapTile mapTilePrefab;
    [SerializeField] private List<Block> blockerPrefabList;
    [SerializeField] private SpawnPos spawnPos;
    
    [TitleGroup("StageNumber")]
    [SerializeField] private int setStageNumber;

    
    [Button(ButtonSizes.Gigantic), GUIColor(0.2f, 1f, 0.2f)]
    
    public void GenerateMap()
    {
        // 인스턴스 한다
        var template = Instantiate(mapDesignTemplate, new Vector3(0, 0, 0), Quaternion.identity);
        // 맵의 범위를 설정하고 배경오브젝트를 깐다.
        GenerateFromPreset(transform.GetComponent<MapPreset>(), template);
        // 저장
        PrefabUtility.SaveAsPrefabAsset(template.gameObject, "Assets/Prefabs/Maps/Map"+setStageNumber+".prefab");
        //삭제
        
        // 스크립터블 오브젝트 정보 저장 하고 삭제
        DestroyImmediate(template.gameObject);
    }
    
    public void GenerateFromPreset(MapPreset mapPreset, Map template)
    {
        template.MapTiles = new Dictionary<Vector3Int, MapTile>();
        var backGroundTileMap = mapPreset.BackgroundMap.GetComponent<Tilemap>();
        var mapTemplate = SetMapTile(backGroundTileMap, mapTilePrefab, template.transform, template);
        
        var otherTileMapList = mapPreset.MapLayerList;
        if (otherTileMapList.Count != 0)
        { 
            for (var i = 0; i < otherTileMapList.Count; i++)
            {
                var tileMap = otherTileMapList[i].GetComponent<Tilemap>();
                
                SetOtherTile(tileMap, blockerPrefabList[i], template.transform, mapTemplate);
            }
        }
        var spawnPlace = mapPreset.SpawnPlace.GetComponent<Tilemap>();
        SettingSpawnPlace(spawnPlace, template);
        template.MapTiles = mapTemplate.MapTiles;
    }

    private Map SetMapTile(Tilemap tilemap, MapTile targetObject, Transform temPlate, Map template)
    {
        for (var i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
        {
            for (var j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
            {
                var currTileMapCoord = new Vector3Int(i, j, 0);
                var currCoord = CoordUtil.UnityCellToCube(currTileMapCoord);
                
                var currPos = tilemap.CellToWorld(currTileMapCoord);
                currPos = new Vector3(currPos.x, currPos.y, 0);
                
                if (tilemap.HasTile(currTileMapCoord))
                {
                    var instance = PrefabUtility.InstantiatePrefab(targetObject, temPlate.transform) as MapTile;
                    if (instance == null)
                    {
                        Debug.LogWarning("Casting to GameObject failed. Tile instance is null");
                    }
                    else
                    {
                        instance.transform.position = currPos;
                        instance.MapTileCoord = currCoord;
                        template.MapTiles.Add(currCoord,instance);
                        template.MapTilePresetDataList.Add(new MapTilePresetData(currCoord, instance));
                    }
                }
            }
        }
        return template;
    }

    private void SetOtherTile(Tilemap tilemap, Block blocks, Transform temPlate, Map template)
    {
        for (var i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
        {
            for (var j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
            {
                // 맵의 범위를 설정한다.
                var currTileMapCoord = new Vector3Int(i, j, 0);
                var currCoord = CoordUtil.UnityCellToCube(currTileMapCoord);
                
                // 해당 좌표의 위치를 계산한다.
                var currPos = tilemap.CellToWorld(currTileMapCoord);
                currPos = new Vector3(currPos.x, currPos.y, 0);
                
                if (tilemap.HasTile(currTileMapCoord))
                {
                    var instance = PrefabUtility.InstantiatePrefab(blocks, temPlate.transform) as Block;
                    if (instance == null)
                    {
                        Debug.LogWarning("Casting to GameObject failed. Tile instance is null");
                    }
                    else
                    {
                        instance.transform.position = currPos;
                        if (instance.IsMovable)
                        {
                            template.MapTiles[currCoord].MovableBlockOnMapTile = instance;
                        }
                        else
                        {
                            template.MapTiles[currCoord].UnMovalbleBlockOnMapTile = instance;
                        }
                    }
                }
            }
        }
    }
    
    private void SettingSpawnPlace(Tilemap tilemap, Map template)
    {
        for (var i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
        {
            for (var j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
            {
                var currTileMapCoord = new Vector3Int(i, j, 0);
                
                var currCoord = CoordUtil.UnityCellToCube(currTileMapCoord);
                
                var currPos = tilemap.CellToWorld(currTileMapCoord);
                // currPos = new Vector3(currPos.x, currPos.y, 0);
                
                if (tilemap.HasTile(currTileMapCoord))
                {
                    var instance = PrefabUtility.InstantiatePrefab(spawnPos, template.transform) as SpawnPos;
                    if (instance != null)
                    {
                        instance.transform.position = currPos;
                        instance.SpawnPosCoord = currCoord;
                        template.SpawnPlace.Add(instance);
                    }
                }
            }
        }
    }
}
