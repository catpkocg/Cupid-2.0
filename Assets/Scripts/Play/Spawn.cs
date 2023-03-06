using System.Collections.Generic;
using UnityEngine;
using Wayway.Engine;
using Random = UnityEngine.Random;

/// <summary>
/// 스폰을 담당한다.
/// </summary>
public class Spawn : MonoBehaviour
{
    [SerializeField] private Transform blockContainer;
    [SerializeField] private List<Block> normalBlocks;
    
    public List<Block> lineClearBlocks;
    public List<Block> sameColorClearBlocks;
    public int moveCounter;
    
    private const float UpPosNum = 0.865977f;

    public void SpawnBlockOnTile(Map map)
    {
        var mapTiles = map.MapTiles;
        Debug.Log(mapTiles.Count);
        mapTiles.Keys.ForEach(mapTilePos =>
        {
            if (mapTiles[mapTilePos].MovableBlockOnMapTile == null)
            {
                var normalBlock =
                    Instantiate(normalBlocks[Random.Range(0, map.BlockNumber)],
                        mapTiles[mapTilePos].transform.position, Quaternion.identity);
                mapTiles[mapTilePos].MovableBlockOnMapTile = normalBlock;
                normalBlock.Coord = mapTilePos;
                // normalBlock.GetComponentInChildren<TextMeshPro>().text = mapTilePos.ToString();
            }
        });
    }

    public void SpawnForEmptyPlace()
    {
        var spawnPlace = MapManager.Instance.map.SpawnPlace;
        var mapTiles = MapManager.Instance.map.MapTiles;
        for (int i = 0; i < spawnPlace.Count; i++)
        {
            var howManyNeedToSpawn = MapUtil.CountNullPlace(mapTiles, spawnPlace[i].SpawnPosCoord);
            for (int j = 0; j < howManyNeedToSpawn; j++)
            {
                var spawnPos = spawnPlace[i].transform.position +
                               (new Vector3(0, UpPosNum, 0) * (j));
                var random = Random.Range(0, MapManager.Instance.map.BlockNumber);
                var block = Instantiate(normalBlocks[random], spawnPos, Quaternion.identity);
                
                MapManager.Instance.WhatWillMove.Add(block);
                MapManager.Instance.WhereToMove.Add((spawnPlace[i].SpawnPosCoord 
                                                     + new Vector3Int(0,1,-1) * (howManyNeedToSpawn - j)));
            }
        }
    }

    public Block SpawnRandomLineBlock(MapTile mapTile)
    {
        Destroy(mapTile.MovableBlockOnMapTile.gameObject);
        var random = Random.Range(0, 3);
        var randomLineClear = Instantiate(lineClearBlocks[random], mapTile.transform.position, Quaternion.identity);
        mapTile.MovableBlockOnMapTile = randomLineClear;
        randomLineClear.Coord = mapTile.MapTileCoord;
        randomLineClear.transform.localScale = new Vector3(0, 0, 0);
        return randomLineClear;
    }

    public void SpawnSameColorBlock(MapTile mapTile, int blockValue)
    {
        var block = Instantiate(sameColorClearBlocks[blockValue], mapTile.transform.position, Quaternion.identity);
        mapTile.MovableBlockOnMapTile = block;
        block.Coord = mapTile.MapTileCoord;
    }

    public void SpawnLineClearBlock(MapTile mapTile, int blockValue)
    {
        var block = Instantiate(lineClearBlocks[blockValue], mapTile.transform.position, Quaternion.identity);
        mapTile.MovableBlockOnMapTile = block;
        block.Coord = mapTile.MapTileCoord;
    }
    
    
}