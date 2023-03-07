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
        mapTile.MovableBlockOnMapTile = this;
        mapTile.MovableBlockOnMapTile.Coord = mapTile.MapTileCoord;

        MoveAnimation(mapTile.transform.position);
    }
}
