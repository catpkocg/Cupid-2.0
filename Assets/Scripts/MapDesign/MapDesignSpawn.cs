using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDesignSpawn : MonoBehaviour
{
    [SerializeField] private MapPreset mapPreset;
    [SerializeField] private List<Block> normalBlocks;
    [SerializeField] private List<Block> blockers;
    [SerializeField] private GameObject backGroundBlock;
    [SerializeField] private GameConfig gameConfig;
    
    public Transform blockCase;
    
    public void SettingForMapPrefabs()
    {
        var otherTileMapList = mapPreset.MapLayerList;
        if (otherTileMapList.Count != 0)
        { 
            for (var i = 0; i < otherTileMapList.Count; i++)
            {
                var tileMap = otherTileMapList[i].GetComponent<Tilemap>();
                if (otherTileMapList[i].IsMovable)
                {
                    SettingOtherBlocks(tileMap,blockers[i],mapPreset.MovableBlocks);
                }
                else
                {
                    SettingOtherBlocks(tileMap,blockers[i],mapPreset.UnMovableBlocks);
                }
                
            }
        }
        var backGroundTileMap = mapPreset.BackgroundMap.GetComponent<Tilemap>();
        SettingNormalBlocks(backGroundTileMap, normalBlocks);
    }
    
    private void SettingNormalBlocks(Tilemap tilemap, List<Block> blocks)
    {
        for (var i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
        {
            for (var j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
            {
                var localPlace = new Vector3Int(i, j, 0);
                var cubeCoord = Util.UnityCellToCube(localPlace);
                var place = tilemap.CellToWorld(localPlace);
                var putPlace = new Vector3(place.x, place.y, 0);
                if (!tilemap.HasTile(localPlace)) continue;
                if (mapPreset.MovableBlocks.ContainsKey(cubeCoord)) continue;
                var random = UnityEngine.Random.Range(0, gameConfig.BlockNumber);
                var block = Instantiate(blocks[random], putPlace, Quaternion.identity);
                var backGround = Instantiate(backGroundBlock, putPlace, Quaternion.identity);
                mapPreset.MapBounds.Add(cubeCoord);
                mapPreset.MovableBlocks.Add(cubeCoord,block);
                block.transform.SetParent(blockCase);
                backGround.transform.SetParent(blockCase);
            }
        }
    }
    
    private void SettingOtherBlocks(Tilemap tilemap, Block notNormalBlock, Dictionary<Vector3Int,Block> blockDictionary)
    {
        for (var i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
        {
            for (var j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
            {
                var localPlace = new Vector3Int(i, j, 0);
                var cubeCoord = Util.UnityCellToCube(localPlace);
                var place = tilemap.CellToWorld(localPlace);
                var putPlace = new Vector3(place.x, place.y, 0);
                if (!tilemap.HasTile(localPlace)) continue;
                var block = Instantiate(notNormalBlock, putPlace, Quaternion.identity);
                var backGround = Instantiate(backGroundBlock, putPlace, Quaternion.identity);
                mapPreset.MapBounds.Add(cubeCoord);
                blockDictionary.Add(cubeCoord,block);
                block.transform.SetParent(blockCase);
                backGround.transform.SetParent(blockCase);
            }
        }
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var a = (int)BlockersType.IceBlocker;
            Debug.Log(a);
        }
    }
}

enum BlockersType
{
    WoodBlocker = 0,
    IceBlocker = 1
    
}
