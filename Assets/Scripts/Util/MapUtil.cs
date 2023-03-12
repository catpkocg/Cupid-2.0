using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wayway.Engine;

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
    
    public static int CalCulateMaxAndMinDif(List<int> allNum)
    {
        var min = allNum.Min();
        var max = allNum.Max();
    
        var amount = max - min;
    
        return amount;
    }
    
    public static int CalCulateAverage(List<int> allNum)
    {
        int Sum = allNum.Sum();
        int Average = Sum / allNum.Count;
    
        return Average;
    }
    
    public static List<Block> FindAllNearSameValue(Block block)
    {
        var toSearch = new List<Block>();
        var searched = new List<Block>();
        var allSameBlocks = new List<Block>();
        toSearch.Add(block);
        allSameBlocks.Add(block);
        var tempCount = 0;
        while (!toSearch.IsNullOrEmpty())
        {
            var currSearchTarget = toSearch[0];
            var sameBlocks = FindNearSameValue(currSearchTarget, MapManager.Instance.neighborPos);
            if (!sameBlocks.IsNullOrEmpty())
            {
                for (var i = 0; i < sameBlocks.Count; i++)
                {
                    var currSameBlock = sameBlocks[i];

                    if (!searched.Contains(currSameBlock)
                        && !toSearch.Contains(currSameBlock))
                    {
                        allSameBlocks.Add(currSameBlock);
                        toSearch.Add(currSameBlock);
                    }
                }
            }

            searched.Add(currSearchTarget);
            toSearch.Remove(currSearchTarget);

            if (500 < tempCount) break;
            tempCount++;
        }

        Debug.Log(allSameBlocks.Count);
        return allSameBlocks;
    }

    private static List<Block> FindNearSameValue(Block block, NeighborPos neighborPos)
    {
        var map = MapManager.Instance.map;
        List<Block> sameBlockList = new List<Block>();
        for (int i = 0; i < neighborPos.neighbor.Count; i++)
        {
            var neighbor = block.Coord + neighborPos.neighbor[i].neighborPos;
            if (map.MapTiles.ContainsKey(neighbor) && map.MapTiles[neighbor].MovableBlockOnMapTile != null)
            {
                var neighborValue = map.MapTiles[neighbor].MovableBlockOnMapTile.value;
                if (neighborValue == block.value)
                {
                    sameBlockList.Add(map.MapTiles[neighbor].MovableBlockOnMapTile);
                }
            }
        }

        return sameBlockList;
    }
    
    
}