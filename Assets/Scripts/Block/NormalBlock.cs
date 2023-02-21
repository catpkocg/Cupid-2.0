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
        var mapTile = MapManager.Instance.map.MapTiles[Coord];
        
        PangMainBlock(this);
        PangNearBoxBlock(mapTile);
        PangIceOnBlock(mapTile);
        
        // Destroy(gameObject);
        // MapManager.Instance.map.MapTiles[Coord].MovableBlockOnMapTile = null;
        // Debug.Log("일반 블럭");
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

    private void PangNearBoxBlock(MapTile mapTile)
    {
        var mapTiles = MapManager.Instance.map.MapTiles;
        var nearPosList = MapManager.Instance.neighborPos.neighbor;
        for (int i = 0; i < nearPosList.Count; i++)
        {
            var nearPos = mapTile.MapTileCoord + nearPosList[i].neighborPos;
            if (mapTiles.ContainsKey(nearPos))
            {
                var nearTile = mapTiles[nearPos];
                if (nearTile.MovableBlockOnMapTile != null)
                {
                    if(nearTile.MovableBlockOnMapTile.value == 61)
                    {
                        nearTile.MovableBlockOnMapTile.Pang();
                    }
                    
                    
                }
            }
        }

    }

    private void PangIceOnBlock(MapTile mapTile)
    {
        if (mapTile.UnMovalbleBlockOnMapTile != null)
        {
            if (mapTile.UnMovalbleBlockOnMapTile.value == 71)
            {
                mapTile.UnMovalbleBlockOnMapTile.Pang();
            }
        }
    }
    
}
