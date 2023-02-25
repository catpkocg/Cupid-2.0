using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Wayway.Engine;

public class SameColorClearBlock : Block
{
    private void Awake()
    {
        OnPang += Pang;
        MoveBlock += Move;
    }

    private void OnDisable()
    {
        OnPang -= Pang;
        MoveBlock += Move;
    }

    public override void Pang()
    {
        var gameManager = GameManager.Instance;
        var mapTile = MapManager.Instance.map.MapTiles[Coord];
        var blockValue = this.value;
        gameManager.ConditionStates[blockValue]++;
        gameManager.score += gameManager.gameConfig.SameColorClearBlockCondition * 30;
        PangMainBlock(this);
        PangNearBoxBlock(mapTile);
        PangIceOnBlock(mapTile);
        DeleteSameColorBlock(mapTile);
    }

    public override void Move(MapTile mapTile)
    {
        IsMoving = true;
        var gameConfig = GameManager.Instance.gameConfig;
        mapTile.MovableBlockOnMapTile = this;
        mapTile.MovableBlockOnMapTile.Coord = mapTile.MapTileCoord;
        transform.DOMove(mapTile.transform.position, gameConfig.AnimationSpeed).SetEase(gameConfig.EasyType).OnComplete(ChangeCondition);
    }
    
    private void ChangeCondition()
    {
        IsMoving = false;
    }
    
    private void DeleteSameColorBlock(MapTile mapTile)
    {
        var sameColorBlocks = new List<Block>();
        var mapTiles = MapManager.Instance.map.MapTiles;
        var randomNumber = Random.Range(0, MapManager.Instance.gameConfig.BlockNumber);
        
        Debug.Log(randomNumber +"이값 전부다 블럭 터뜨림");
        
        mapTiles.Keys.ForEach(pos =>
        {
            if (mapTiles[pos].MovableBlockOnMapTile == null) return;
            if (mapTiles[pos].MovableBlockOnMapTile.value == randomNumber+1)
            {
                sameColorBlocks.Add(mapTiles[pos].MovableBlockOnMapTile);
            }
        });

        foreach (var sameColor in sameColorBlocks)
        {
            sameColor.Pang();
        }
    }


}
