using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private List<Block> allKindsOfBlock;
    [SerializeField] private Transform blockBase;
    
    public List<Block> allBlock;
    public List<Block> newBlocks;
    public List<Vector3Int> newBlocksPos;
    public List<Vector3Int> allBlockTarget;
    
    void Start()
    {
        SpawnBlockOnTile();
    }
    private void SpawnBlockOnTile()
    {
        
        var grid = Map.Instance.tilemap.GetComponentInParent<Grid>();
        
        for (int i = 0; i < Map.Instance.canSpwanPlace.Count; i++)
        {
            var cellCoord = grid.WorldToCell(Map.Instance.canSpwanPlace[i]);
            var cubeCoord = Util.UnityCellToCube(cellCoord);
            var random = Random.Range(0, Map.Instance.gameConfig.BlockNumber);
            var block = Instantiate(allKindsOfBlock[random], Map.Instance.canSpwanPlace[i], Quaternion.identity);
            block.transform.SetParent(blockBase);
            
            block.GetComponentInChildren<TextMeshPro>().text = cubeCoord.ToString();
            block.Coord = cubeCoord;
            Map.Instance.BlockPlace.Add(cubeCoord,block);
        }
    }

    public void SpawnForEmptyPlace(List<Vector3> emptyPlace)
    {
        var grid = Map.Instance.tilemap.GetComponentInParent<Grid>();
        for (int i = 0; i < emptyPlace.Count; i++)
        {
            var cellCoord = grid.WorldToCell(emptyPlace[i]);
            var cubeCoord = Util.UnityCellToCube(cellCoord);
            var random = Random.Range(0, Map.Instance.gameConfig.BlockNumber);
            var block = Instantiate(allKindsOfBlock[random], emptyPlace[i], Quaternion.identity);
            block.transform.SetParent(blockBase);
        }
    }
    
    /*public void CreateNewBlockForEmptyPlaceAndCheckTarget()
    {
        newBlocks = new List<Block>();
        newBlocksPos = new List<Vector3Int>();
        var grid = Map.Instance.GetComponent<Grid>();
        
        for (var j = 0; j < Map.Instance.Config.GridSize.y; j++)
        {
            var height = Map.Instance.Config.GridSize.x + (j % 2 == 0 ? 0 : -1);
            var currentLineNullNum = CalCuNullNum(new Vector2Int(height, j));
            for (var i = 0; i < currentLineNullNum; i++)
            {
                var random = Random.Range(0, Map.Instance.Config.BlockCount);
                var pos = grid.GetCellCenterWorld(new Vector3Int(height+i, j, 0));
                var block = Instantiate(threeKindsOfBlock[random], pos, Quaternion.identity);
                newBlocks.Add(block);
                newBlocksPos.Add(new Vector2Int((height+i) - currentLineNullNum,j));
                block.transform.SetParent(blockBase.transform);
            }
        }
    }*/
    
    
}