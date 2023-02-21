using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Wayway.Engine;

public class BoxBlocker : Block
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
        MapManager.Instance.map.MapTiles[Coord].MovableBlockOnMapTile = null;
        Destroy(gameObject);
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