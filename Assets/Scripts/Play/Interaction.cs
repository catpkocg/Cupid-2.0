using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wayway.Engine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private NeighborPos neighborPos;
    
    public void OnTileClickHandler(Vector3Int coord)
    {
        var mapTile = GameManager.Instance.map.MapTiles[coord];
        var sameBlockList = FindAllNearSameValue(mapTile.MovableBlockOnMapTile);
        Debug.Log(sameBlockList.Count);
        if (sameBlockList.Count > 1)
        {
            for (int i = 0; i < sameBlockList.Count; i++)
            {
                sameBlockList[i].Pang();
            }
        }
        
        GameManager.Instance.ChangeState(States.CreateNewBlock);
        //mapTile.MovableBlockOnMapTile.Pang();
    }
    
    public List<Block> FindAllNearSameValue(Block block)
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
            var sameBlocks = FindNearSameValue(currSearchTarget);
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
    
            if(500 < tempCount) break;
            tempCount++;
        }
        return allSameBlocks;
    }

    public List<Block> FindNearSameValue(Block block)
    {
        var map = GameManager.Instance.map;
        List<Block> sameBlockList = new List<Block>();
        for (int i = 0; i < neighborPos.neighbor.Count; i++)
        {
            var neighbor = block.Coord + neighborPos.neighbor[i].neighborPos;
            if (map.MapTiles.ContainsKey(neighbor) && map.MapTiles[neighbor].MovableBlockOnMapTile != null)
            {
                var neighborValue = map.MapTiles[neighbor].MovableBlockOnMapTile.value;
                // 블럭끼리의 값이 같은거 + 나무상자의 value일경우 같은 블럭에 넣어서 삭제할수있도록 한다.
                if (neighborValue == block.value)
                {
                    sameBlockList.Add(map.MapTiles[neighbor].MovableBlockOnMapTile);
                }
            }
        }
        return sameBlockList;
    }
    
    
}
