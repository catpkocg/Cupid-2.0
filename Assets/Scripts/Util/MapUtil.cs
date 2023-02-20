using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapUtil
{
    public static int CountNullPlace(Dictionary<Vector3Int, MapTile> mapTiles, Vector3Int wantToCheckPos)
    {
        var nullCount = 0;
        var targetPos = wantToCheckPos + new Vector3Int(0, 1, -1);
        while (mapTiles.ContainsKey(targetPos))
        {
            if (mapTiles[targetPos].MovableBlockOnMapTile == null)
            {
                nullCount++;
            }
            targetPos += new Vector3Int(0, 1, -1);
        }
        return nullCount;
    }
    
}