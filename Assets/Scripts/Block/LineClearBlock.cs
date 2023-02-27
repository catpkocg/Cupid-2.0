using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Wayway.Engine;

public class LineClearBlock : Block
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
        var axis = mapTile.MovableBlockOnMapTile.value - 20;
        var blockValue = this.value;
        gameManager.ConditionStates[blockValue]++;
        gameManager.score += gameManager.gameConfig.LineClearBlockCondition * 20;
        PangMainBlock(this);
        PangNearBoxBlock(mapTile);
        PangIceOnBlock(mapTile);
        DeleteSameLineBlock(mapTile, axis);
    }

    public override void Move(MapTile mapTile)
    {
        IsMoving = true;
        var gameConfig = GameManager.Instance.gameConfig;
        mapTile.MovableBlockOnMapTile = this;
        mapTile.MovableBlockOnMapTile.Coord = mapTile.MapTileCoord;
        
        MoveAnimation(mapTile.transform.position);
    }

    private void DeleteSameLineBlock(MapTile mapTile, int line)
    {
        List<Block> sameLineBlocks = new List<Block>();
        var mapTiles = MapManager.Instance.map.MapTiles;
        mapTiles.Keys.ForEach(pos =>
        {
            if (mapTiles[pos].MovableBlockOnMapTile == null) return;
            if (CoordUtil.GetAxisValue(pos, mapTile.MapTileCoord, line))
            {
                mapTiles[pos].MovableBlockOnMapTile.Pang();
            }
        });
    }
    
}
