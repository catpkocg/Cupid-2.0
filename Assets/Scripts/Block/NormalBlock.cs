using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NormalBlock : Block
{
    // public BlockType BlockType { get; set; }
    //
    // public bool IsMovable;
    //
    // public bool IsMoving;
    //
    // public int ShapeValue;
    // public List<GameObject> dir;
    // public GameObject foot;
    // public Vector3Int Coord;
    // public int score;
    //
    // [SerializeField] public int value;
    //
    // protected Action OnPang;
    // protected Action<MapTile> MoveBlock;
    
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
        Debug.Log("일반 블럭");
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
