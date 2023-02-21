using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Wayway.Engine;

public class LineClearBlock : SpecialBlock
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
        var axis = mapTile.MovableBlockOnMapTile.value - 20;
        Destroy(gameObject);
        MapManager.Instance.map.MapTiles[Coord].MovableBlockOnMapTile = null;
        
        DeleteSameLineBlock(mapTile, axis);
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

    private void DeleteSameLineBlock(MapTile mapTile, int line)
    {
        List<Block> sameLineBlocks = new List<Block>();
        var mapTiles = MapManager.Instance.map.MapTiles;
        //var direction = mapTile.MovableBlockOnMapTile.value - 20;
        mapTiles.Keys.ForEach(pos =>
        {
            if (mapTiles[pos].MovableBlockOnMapTile != null)
            {
                if (CoordUtil.GetAxisValue(pos, mapTile.MapTileCoord, line))
                {
                    sameLineBlocks.Add(mapTiles[pos].MovableBlockOnMapTile);
                }
            }
        });

        for (int i = 0; i < sameLineBlocks.Count; i++)
        {
            sameLineBlocks[i].Pang();
        }
    }
}
