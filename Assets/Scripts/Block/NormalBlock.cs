using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NormalBlock : Block
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
        gameManager.score += 10;
        PangMainBlock(this);
        PangNearBoxBlock(mapTile);
        PangIceOnBlock(mapTile);
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

    
}
