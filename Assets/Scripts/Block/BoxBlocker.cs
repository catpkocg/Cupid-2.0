using DG.Tweening;
using UnityEngine;

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
        var block = this;
        
        var blockValue = this.value;
        GameManager.Instance.ConditionStates[blockValue]++;
        MapManager.Instance.map.MapTiles[Coord].MovableBlockOnMapTile = null;
        
        var gameConfig = GameManager.Instance.gameConfig;
        
        block.transform.DOScale(Vector3.one * 0.8f, 0.1f)
            .SetEase(gameConfig.EasyType)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                Destroy(block.gameObject);
            });
    }

    public override void Move(MapTile mapTile)
    {
        IsMoving = true;
        var gameConfig = GameManager.Instance.gameConfig;
        mapTile.MovableBlockOnMapTile = this;
        mapTile.MovableBlockOnMapTile.Coord = mapTile.MapTileCoord;
        
        MoveAnimation(mapTile.transform.position);
        
        // transform.DOMove(mapTile.transform.position, gameConfig.AnimationSpeed).SetEase(gameConfig.EasyType).OnComplete(ChangeCondition);
    }
}