using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;

public class MapGenerator : MonoBehaviour
{
    [TitleGroup("Prefabs")]
    [SerializeField] private Map mapDesignTemplate;
    [SerializeField] private MapTile mapTilePrefab;
    [SerializeField] private List<Block> blockPrefabList;
    [SerializeField] private SpawnPos spawnPos;
    
    [TitleGroup("ClearCondition")]
    [SerializeField] private List<ClearCondition> ClearConditionDataAdd;

    [SerializeField] private int PerfectScore;
    [SerializeField] private int MoveLimitAdd;
    [SerializeField] private int HowManyKindsOfBlock;
    
    [TitleGroup("StageNumber")]
    [SerializeField] private int setStageNumber;

    [Button(ButtonSizes.Gigantic), GUIColor(0.2f, 1f, 0.2f)]
    public void GenerateMap()
    {
        // 인스턴스 한다
        var template = Instantiate(mapDesignTemplate, new Vector3(0, 0, 0), Quaternion.identity);
        // 맵의 범위를 설정하고 배경오브젝트를 깐다.
        ClearConditionAdd(template);
        GenerateFromPreset(transform.GetComponent<MapPreset>(), template);
        // 저장
        
        template.BlockNumber = HowManyKindsOfBlock;
        var preset = Instantiate(this.gameObject);
        preset.gameObject.SetActive(false);
        preset.transform.SetParent(template.transform);
        PrefabUtility.SaveAsPrefabAsset(template.gameObject, "Assets/Prefabs/Maps/Resources/Map"+setStageNumber+".prefab");
        //template.ClearConditionData = ClearConditionData;
        
        //삭제
        // 스크립터블 오브젝트 정보 저장 하고 삭제
        DestroyImmediate(template.gameObject);
        
        Debug.Log(ClearConditionDataAdd.Count);
    }

    public void ClearConditionAdd(Map template)
    {
        for (int i = 0; i < ClearConditionDataAdd.Count; i++)
        {
            template.ClearConditionData.Add(ClearConditionDataAdd[i]);
        }

        template.MoveLimit = MoveLimitAdd;
        template.PerfectScore = PerfectScore;
    }
    public void GenerateFromPreset(MapPreset mapPreset, Map template)
    {
        template.MapTiles = new Dictionary<Vector3Int, MapTile>();
        var backGroundTileMap = mapPreset.BackgroundMap.GetComponent<Tilemap>();
        var mapTemplate = SetMapTile(backGroundTileMap, mapTilePrefab, template.transform, template);
        
        mapPreset.MapLayerList.ForEach(mapLayer =>
        {
            var tileMap = mapLayer.GetComponent<Tilemap>();
            var prefabByValue = blockPrefabList.Find(x => x.value == mapLayer.value);
            SetOtherTile(tileMap, prefabByValue, template.transform, mapTemplate);
        });
        
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
    private void SetOtherTile(Tilemap tilemap, Block block, Transform temPlate, Map template)
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
                    var instance = PrefabUtility.InstantiatePrefab(block, temPlate.transform) as Block;
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
                            instance.Coord = currCoord;

                        }
                        else
                        {
                            template.MapTiles[currCoord].UnMovalbleBlockOnMapTile = instance;
                            instance.Coord = currCoord;
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
#endif
[Serializable]
public class ClearCondition
{
    [HorizontalGroup("ClearCondition"), HideLabel] public ClearConditionBlock ConditionBlock;
    [HorizontalGroup("ClearCondition"), HideLabel] public int HowMuchForClear;

    public ClearCondition(ClearConditionBlock conditionBlock, int howMuchForClear)
    {
        ConditionBlock = conditionBlock;
        HowMuchForClear = howMuchForClear;
    }
}


