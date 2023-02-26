using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
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
    // 시작하면 맵에 있는 정보를 통해, 이동할수있는 블럭이 없는곳에 블럭생성
    
    private const float UpPosNum = 0.865977f;

    //아이스 블럭의 value 는 66으로 한다.
    //박스 블럭의 value 는 77로 한다.
    //이동을 위한 변수들, 새로운 블락과 새로운블락이 이동할곳, 기존블럭과 기존블럭이 이동할곳
    
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
                normalBlock.GetComponentInChildren<TextMeshPro>().text = mapTilePos.ToString();
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
        Destroy(mapTile.MovableBlockOnMapTile);
        var random = Random.Range(0, 3);
        var randomLineClear = Instantiate(lineClearBlocks[random], mapTile.transform.position, Quaternion.identity);
        mapTile.MovableBlockOnMapTile = randomLineClear;
        randomLineClear.Coord = mapTile.MapTileCoord;
        randomLineClear.transform.localScale = new Vector3(0, 0, 0);
        return randomLineClear;
    }
    

    //remodeling

    // public void SpawnSpecialOneBlock(Vector3 putPos, int dir)
    // {
    //     var grid = Map.Instance.Tilemap.GetComponentInParent<Grid>();
    //     
    //     var cellCoord = grid.WorldToCell(putPos);
    //     var cubeCoord = Util.UnityCellToCube(cellCoord);
    //     Debug.Log(specialOne.Count + " 왜 길이가 짧아????????");
    //     var specialBlock = Instantiate(specialOne[dir-1], putPos, Quaternion.identity);
    //     specialBlock.transform.SetParent(blockBase);
    //     specialBlock.Coord = cubeCoord;
    //     specialBlock.specialValue = (int)gameConfig.SpecialBlock1Condition + dir;
    //     
    //     Map.Instance.BlockPlace[cubeCoord] = specialBlock;
    //     
    //     for (int i = 0; i < specialBlock.dir.Count; i++)
    //     {
    //         specialBlock.dir[i] = null;
    //     }
    //     specialBlock.foot = null;
    // }
    // public void SpawnSpecialTwoBlock(Vector3 putPos)
    // {
    //     var grid = Map.Instance.Tilemap.GetComponentInParent<Grid>();
    //     
    //     var cellCoord = grid.WorldToCell(putPos);
    //     var cubeCoord = Util.UnityCellToCube(cellCoord);
    //     
    //     var random = Random.Range(0, Map.Instance.GameConfig.BlockNumber);
    //     var specialBlock = Instantiate(specialTwo[random], putPos, Quaternion.identity);
    //     specialBlock.transform.SetParent(blockBase);
    //
    //     specialBlock.specialValue = (int)gameConfig.SpecialBlock2Condition;
    //     specialBlock.Coord = cubeCoord;
    //     Map.Instance.BlockPlace[cubeCoord] = specialBlock;
    //     
    //     for (int i = 0; i < specialBlock.dir.Count; i++)
    //     {
    //         specialBlock.dir[i] = null;
    //     }
    //
    //     specialBlock.foot = null;
    //     
    // }
    
    // public void MoveAllDown()
    // {
    //     var grid = Map.Instance.Tilemap.GetComponentInParent<Grid>();
    //     
    //     var sequenceAnim = DOTween.Sequence();
    //     
    //     for (int j = 0; j < notNewBlocks.Count; j++)
    //     {
    //         var pos = notNewBlocksPos[j];
    //         var posForMap = grid.WorldToCell(pos);
    //         sequenceAnim.Join(notNewBlocks[j].transform.DOMove(pos, gameConfig.AnimationSpeed).SetEase(gameConfig.EasyType));
    //         var blockCoord = Util.UnityCellToCube(posForMap);
    //         notNewBlocks[j].Coord = blockCoord;
    //         notNewBlocks[j].GetComponentInChildren<TextMeshPro>().text = blockCoord.ToString();
    //         Map.Instance.BlockPlace[blockCoord] = notNewBlocks[j];
    //     }
    //
    //     for (int i = 0; i < newBlocks.Count; i++)
    //     {
    //         var pos = newBlocksPos[i];
    //         var posForMap = grid.WorldToCell(pos);
    //         sequenceAnim.Join(newBlocks[i].transform.DOMove(pos, gameConfig.AnimationSpeed).SetEase(gameConfig.EasyType));
    //         var blockCoord = Util.UnityCellToCube(posForMap);
    //         newBlocks[i].Coord = blockCoord;
    //         newBlocks[i].GetComponentInChildren<TextMeshPro>().text = blockCoord.ToString();
    //         Map.Instance.BlockPlace[blockCoord] = newBlocks[i];
    //     }
    //
    //     moveCounter++;
    //     
    //     sequenceAnim.OnComplete(ChangeStatesForMove);
    // }
    // public void ChangeStatesForMove()
    // {
    //     Debug.Log("움직임 끝남");
    //     Map.Instance.DrawDirectionOnBlock();
    //     GameManager.Instance.State = States.DeleteBlock;
    // }
}