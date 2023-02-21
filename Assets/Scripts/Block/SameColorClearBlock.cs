using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SameColorClearBlock : SpecialBlock
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

    private void Pang()
    {
        Destroy(gameObject);
        MapManager.Instance.map.MapTiles[Coord].MovableBlockOnMapTile = null;
        
        //맵매니저에서 같은색깔 전체 삭제하는 메소드 구현
        
        //같은 색깔 전체 삭제
        
        Debug.Log("특수 블럭2");
    }

    private void Move(MapTile mapTile)
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
