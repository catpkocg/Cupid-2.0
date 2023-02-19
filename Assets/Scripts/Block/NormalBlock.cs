using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NormalBlock : Block
{
    public bool IsMoving;
    
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

    private void Pang()
    {
        Destroy(gameObject);
        GameManager.Instance.map.MapTiles[Coord] = null;
        Debug.Log("일반 블럭");
    }

    private void Move(MapTile mapTile)
    {
        IsMoving = true;
        var  gameConfig = GameManager.Instance.gameConfig;
        mapTile.MovableBlockOnMapTile = this;
        transform.DOMove(mapTile.transform.position, gameConfig.AnimationSpeed).SetEase(gameConfig.EasyType).OnComplete(ChangeCondition);
    }

    private void ChangeCondition()
    {
        IsMoving = false;
    }
    
}
