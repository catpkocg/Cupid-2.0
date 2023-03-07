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
        var blockValue = value;
        gameManager.ConditionStates[30]++;
        gameManager.score += gameManager.gameConfig.SameColorClearBlockCondition * 30;
        PangMainBlock(this);
        PangNearBoxBlock(mapTile);
        PangIceOnBlock(mapTile);
        DeleteSameColorBlock(blockValue);
    }

    public override void Move(MapTile mapTile)
    {
        IsMoving = true;
        
        var gameConfig = GameManager.Instance.gameConfig;
        
        mapTile.MovableBlockOnMapTile = this;
        mapTile.MovableBlockOnMapTile.Coord = mapTile.MapTileCoord;
        
        MoveAnimation(mapTile.transform.position);
    }

    private void DeleteSameColorBlock(int cakeValue)
    {
        var sameColorBlocks = new List<Block>();
        var mapTiles = MapManager.Instance.map.MapTiles;
        var sameValue = cakeValue - 30 + 1;
        //var randomNumber = Random.Range(0, MapManager.Instance.gameConfig.BlockNumber);
        //Debug.Log(randomNumber +"이값 전부다 블럭 터뜨림");
        
        mapTiles.Keys.ForEach(pos =>
        {
            if (mapTiles[pos].MovableBlockOnMapTile == null) return;
            if (mapTiles[pos].MovableBlockOnMapTile.value == sameValue)
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
