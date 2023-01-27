using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private List<Block> allKindsOfBlock;
    [SerializeField] private Transform blockBase;
    
    //이동을 위한 변수들, 새로운 블락과 새로운블락이 이동할곳, 기존블럭과 기존블럭이 이동할곳
    public List<Block> newBlocks;
    public List<Vector3> newBlocksPos;
    public List<Block> notNewBlocks;
    public List<Vector3> notNewBlocksPos;
    
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

    public void SpawnForEmptyPlace()
    {
        var grid = Map.Instance.tilemap.GetComponentInParent<Grid>();
        for (int i = 0; i < Map.Instance.newBlockSpawnPos.Count; i++)
        {
            var howManyNeedToSpawn = Map.Instance.CountNullPlace(Map.Instance.newBlockSpawnPos[i]);
            Debug.Log(i + "번 줄은 " + howManyNeedToSpawn + "개 생성해야됨");
            for (int j = 0; j < howManyNeedToSpawn; j++)
            {
                var cellCoord = grid.CellToLocal(Util.CubeToUnityCell(Map.Instance.newBlockSpawnPos[i] + new Vector3Int(0,-1,1) * j));
                Debug.Log(cellCoord);
                var random = Random.Range(0, Map.Instance.gameConfig.BlockNumber);
                var block = Instantiate(allKindsOfBlock[random], cellCoord, Quaternion.identity);
                
                newBlocks.Add(block);
                newBlocksPos.Add(grid.CellToLocal(Util.CubeToUnityCell(Map.Instance.newBlockSpawnPos[i] + new Vector3Int(0,1,-1) * (howManyNeedToSpawn + j))));
                
                block.transform.SetParent(blockBase);
            }
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

    public void MoveAllBlock()
    {
        var grid = Map.Instance.tilemap.GetComponentInParent<Grid>();
        Dictionary<Vector3Int, Block>.KeyCollection keys = Map.Instance.BlockPlace.Keys;
        
        foreach (Vector3Int key in keys)
        {
            var moveCount = Map.Instance.CountNullPlace(key);
            if (moveCount != 0)
            {
                notNewBlocks.Add(Map.Instance.BlockPlace[key]);
                notNewBlocksPos.Add(grid.CellToLocal(Util.CubeToUnityCell(key)));
            }
            
            
        }
    }
    
    
}