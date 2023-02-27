using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Wayway.Engine;

public class IceBlocker : Block
{
    private void Awake()
    {
        OnPang += Pang;
    }

    private void OnDisable()
    {
        OnPang -= Pang;
    }

    public override void Pang()
    {
        //TODO: 이거 PangAnimation으로 Block.cs 에서 메소드 적어주면 좋을 듯
        var block = this;
        
        var blockValue = this.value;
        GameManager.Instance.ConditionStates[blockValue]++;
        MapManager.Instance.map.MapTiles[Coord].UnMovalbleBlockOnMapTile = null;
        
        var gameConfig = GameManager.Instance.gameConfig;
            
        block.transform.DOScale(Vector3.one * 0.8f, 0.1f)
            .SetEase(gameConfig.EasyType)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                Destroy(block.gameObject);
            });
    }
    
}