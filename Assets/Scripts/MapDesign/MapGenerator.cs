using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [TitleGroup("Prefabs")]
    [SerializeField] private Map mapTemplate;
    [SerializeField] private GameObject mapTilePrefab;
    [SerializeField] private List<Block> normalBlockPrefabList;
    [SerializeField] private List<Block> blockerPrefabList;
    
    [TitleGroup("Configuration")]
    [SerializeField] private GameConfig gameConfig;
    
    private List<Vector3Int> bounds = new();
    
    [Button(ButtonSizes.Gigantic), GUIColor(0.2f, 1f, 0.2f)]
    public void GenerateMap()
    {
        // 새로운 맵을 인스턴스화 한다.
        
        // 맵의 범위를 설정하고 배경오브젝트를 깐다.

        // 각 맵타일 레이어에 해당하는 블럭프리팹을 생성한다 : 무버블
        
        // 각 맵타일 레이어에 해당하는 블럭프리팹을 생성한다 : 언무버블
        
    }

    // public Map GenerateFromPreset(MapPreset mapPreset)
    // {
    //     // 배경을 깐다.
    //     
    //     // 
    //     
    //     var otherTileMapList = mapPreset.MapLayerList;
    //     if (otherTileMapList.Count != 0)
    //     { 
    //         for (var i = 0; i < otherTileMapList.Count; i++)
    //         {
    //             var tileMap = otherTileMapList[i].GetComponent<Tilemap>();
    //             if (otherTileMapList[i].IsMovable)
    //             {
    //                 SettingOtherBlocks(tileMap,blockers[i],mapPreset.MovableBlocks);
    //             }
    //             else
    //             {
    //                 SettingOtherBlocks(tileMap,blockers[i],mapPreset.UnMovableBlocks);
    //             }
    //             
    //         }
    //     }
    //     var backGroundTileMap = mapPreset.BackgroundMap.GetComponent<Tilemap>();
    //     SettingNormalBlocks(backGroundTileMap, normalBlockPrefabList);
    // }
    //
    // private void SettingNormalBlocks(Tilemap tilemap, List<Block> normalBlockPrefabList)
    // {
    //     for (var i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
    //     {
    //         for (var j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
    //         {
    //             // 맵의 범위를 설정한다.
    //             var currTileMapCoord = new Vector3Int(i, j, 0);
    //             var currCoord = Util.UnityCellToCube(currTileMapCoord);
    //             mapPreset.MapBounds.Add(currCoord);
    //             
    //             // 해당 좌표의 위치를 계산한다.
    //             var currPos = tilemap.CellToWorld(currTileMapCoord);
    //             currPos = new Vector3(currPos.x, currPos.y, 0);
    //             
    //             // 맵의 범위안에 배경을 생성한다.
    //             Instantiate(mapTilePrefab, currPos, Quaternion.identity, blockCase);
    //             
    //             // 이 레이어가 채워져야하는지 판단한다. 이미 채워져있으면 아무것도 하지 않는다.
    //             if (!tilemap.HasTile(currTileMapCoord) 
    //                 && mapPreset.MovableBlocks.ContainsKey(currCoord)) continue;
    //             
    //             // 노말블럭을 생성한다.
    //             var block = Instantiate(normalBlockPrefabList[Random.Range(0, gameConfig.BlockNumber)], currPos, Quaternion.identity, blockCase);
    //             mapPreset.MovableBlocks.Add(currCoord, block);
    //         }
    //     }
    // }
    //
    // private void CreateMapBounds()
    // {
    //     
    // }
    //
    // // 메소드: 해당 프리팹을 타일맵 타일이 있는 위치들에 생성한다. 생성한 인스턴스를 반환한다.
    // private void InstantiateBlock(Tilemap tilemap, Block blockPrefab)
    // {
    //     
    // }
    //
    // private void SettingOtherBlocks(Tilemap tilemap, Block blockPrefabNotNormal, Dictionary<Vector3Int,Block> blockDictionary)
    // {
    //     for (var i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
    //     {
    //         for (var j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
    //         {
    //             var localPlace = new Vector3Int(i, j, 0);
    //             var cubeCoord = Util.UnityCellToCube(localPlace);
    //             var place = tilemap.CellToWorld(localPlace);
    //             var putPlace = new Vector3(place.x, place.y, 0);
    //             if (!tilemap.HasTile(localPlace)) continue;
    //             var block = Instantiate(blockPrefabNotNormal, putPlace, Quaternion.identity);
    //             var backGround = Instantiate(mapTilePrefab, putPlace, Quaternion.identity);
    //             mapPreset.MapBounds.Add(cubeCoord);
    //             blockDictionary.Add(cubeCoord,block);
    //             block.transform.SetParent(blockCase);
    //             backGround.transform.SetParent(blockCase);
    //         }
    //     }
    // }
}
