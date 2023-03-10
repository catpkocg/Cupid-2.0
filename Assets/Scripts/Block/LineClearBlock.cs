using System.Collections.Generic;
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
        
        // 같은 라인클리어 블럭이 붙어있는지 확인해야함. 
        
        // 세임컬러 클리어 블럭이 붙어있는지 확인해야함.
        
        //var blockValue = this.value;
        gameManager.ConditionStates[20]++;
        gameManager.score += gameManager.gameConfig.LineClearBlockCondition * 20;
        
        PangMainBlock(this);
        //PangNearBoxBlock(mapTile);
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
                if (mapTiles[pos].MovableBlockOnMapTile.value < 10)
                {
                    PangMainBlock(mapTiles[pos].MovableBlockOnMapTile);
                }
                else
                {
                    mapTiles[pos].MovableBlockOnMapTile.Pang();
                }
            }
        });
    }
    
}
