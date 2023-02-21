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

    private void Pang()
    {
        var mapTile = MapManager.Instance.map.MapTiles[Coord];
        PangMainBlock(this);
        PangNearBoxBlock(mapTile);
        PangIceOnBlock(mapTile);
        // Destroy(gameObject);
        // MapManager.Instance.map.MapTiles[Coord].MovableBlockOnMapTile = null;
        DeleteSameColorBlock(mapTile);
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
    
    private void DeleteSameColorBlock(MapTile mapTile)
    {
        List<Block> sameColorBlocks = new List<Block>();
        var mapTiles = MapManager.Instance.map.MapTiles;
        int randomNumber = Random.Range(0, MapManager.Instance.gameConfig.BlockNumber);
        //var direction = mapTile.MovableBlockOnMapTile.value - 20;
        mapTiles.Keys.ForEach(pos =>
        {
            if (mapTiles[pos].MovableBlockOnMapTile != null)
            {
                if (mapTiles[pos].MovableBlockOnMapTile.value == randomNumber)
                {
                    sameColorBlocks.Add(mapTiles[pos].MovableBlockOnMapTile);
                }
            }
        });

        for (int i = 0; i < sameColorBlocks.Count; i++)
        {
            sameColorBlocks[i].Pang();
        }
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
                if (nearTile.MovableBlockOnMapTile != null && nearTile.MovableBlockOnMapTile.value == 61)
                {
                    nearTile.MovableBlockOnMapTile.Pang();
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
