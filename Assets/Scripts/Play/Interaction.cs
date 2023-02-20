using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wayway.Engine;

public class Interaction : MonoBehaviour
{
    public void OnTileClickHandler(Vector3Int coord)
    {
        var mapTile = MapManager.Instance.map.MapTiles[coord];
        var sameBlockList = MapManager.Instance.FindAllNearSameValue(mapTile.MovableBlockOnMapTile);
        if (sameBlockList.Count > 1)
        {
            for (int i = 0; i < sameBlockList.Count; i++)
            {
                sameBlockList[i].Pang();
            }
            
            GameManager.Instance.ChangeState(States.CheckTarget);
        }
    }
}
