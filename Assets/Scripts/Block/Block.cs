using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    
    public bool IsMovable;
    public bool IsMoving;
    
    public int drawValue;
    public List<GameObject> dir;
    public GameObject foot;
    public Vector3Int Coord;
    
    //점수계산이 필요한가?? 현재는 없음
    //public int score;
    
    [SerializeField] public int value;

    protected Action OnPang;
    protected Action<MapTile> MoveBlock;
    
    public virtual void Pang()
    {
        OnPang?.Invoke();
    }
    public virtual void Move(MapTile mapTile)
    {
        MoveBlock?.Invoke(mapTile);
    }

    protected void MoveAnimation(Vector3 targetPos)
    {
        var gameConfig = GameManager.Instance.gameConfig;
        
        transform.DOScaleY(gameConfig.YScaleOnMove, gameConfig.ScaleAnimDuration)
            .SetEase(gameConfig.EasyType)
            .SetDelay(gameConfig.MoveAnimationDelay + gameConfig.AnimationSpeed - gameConfig.ScaleAnimDuration)
            .SetLoops(2, LoopType.Yoyo);
        transform.DOScaleX(gameConfig.XScaleOnMove, gameConfig.ScaleAnimDuration)
            .SetEase(gameConfig.EasyType)
            .SetDelay(gameConfig.MoveAnimationDelay + gameConfig.AnimationSpeed - gameConfig.ScaleAnimDuration)
            .SetLoops(2, LoopType.Yoyo);
        transform.DOMove(targetPos, gameConfig.AnimationSpeed)
            .SetDelay(gameConfig.MoveAnimationDelay)
            .SetEase(gameConfig.EasyType)
            .OnComplete(ChangeCondition);
    }
    
    protected void PangMainBlock(Block block)
    {
        MapManager.Instance.map.MapTiles[block.Coord].MovableBlockOnMapTile = null;

        var gameConfig = GameManager.Instance.gameConfig;
            
        block.transform.DOScale(Vector3.one * 0.8f, 0.1f)
            .SetEase(gameConfig.EasyType)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                var explosion = Instantiate(gameConfig.Explosion, block.transform.position, Quaternion.identity);
                DOVirtual.DelayedCall(0.5f, () => Destroy(explosion.gameObject));
                Destroy(block.gameObject);
            });
    }

    protected void PangNearBoxBlock(MapTile mapTile)
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

    protected void PangIceOnBlock(MapTile mapTile)
    {
        if (mapTile.UnMovalbleBlockOnMapTile != null)
        {
            if (mapTile.UnMovalbleBlockOnMapTile.value == 71)
            {
                mapTile.UnMovalbleBlockOnMapTile.Pang();
            }
        }
    }
    protected void ChangeCondition()
    {
        IsMoving = false;
    }
}

