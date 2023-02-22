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
        var blockValue = this.value;
        GameManager.Instance.ConditionStates[blockValue]++;
        MapManager.Instance.map.MapTiles[Coord].UnMovalbleBlockOnMapTile = null;
        Destroy(gameObject);
    }
    
}