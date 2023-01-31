using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Wayway.Engine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private List<Block> allKindsOfBlock;
    [SerializeField] private Transform blockBase;
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private List<Block> specialOne;
    [SerializeField] private List<Block> specialTwo;
     
    
    //이동을 위한 변수들, 새로운 블락과 새로운블락이 이동할곳, 기존블럭과 기존블럭이 이동할곳
    public List<Block> newBlocks;
    public List<Vector3> newBlocksPos;
    public List<Block> notNewBlocks;
    public List<Vector3> notNewBlocksPos;

    public int moveCounter;
    
    
    void Start()
    {
        SpawnBlockOnTile();
        moveCounter = 0;
    }
    

    public void SpawnSpecialOneBlock(Vector3 putPos, int dir)
    {
        var grid = Map.Instance.tilemap.GetComponentInParent<Grid>();
        
        var cellCoord = grid.WorldToCell(putPos);
        var cubeCoord = Util.UnityCellToCube(cellCoord);
        Debug.Log(specialOne.Count + " 왜 길이가 짧아????????");
        var specialBlock = Instantiate(specialOne[dir-1], putPos, Quaternion.identity);
        specialBlock.transform.SetParent(blockBase);
        specialBlock.Coord = cubeCoord;
        specialBlock.specialValue = (int)gameConfig.SpecialBlock1Condition + dir;
        
        Map.Instance.BlockPlace[cubeCoord] = specialBlock;
        
        for (int i = 0; i < specialBlock.dir.Count; i++)
        {
            specialBlock.dir[i] = null;
        }
        specialBlock.foot = null;
    }

    public void SpawnSpecialTwoBlock(Vector3 putPos)
    {
        var grid = Map.Instance.tilemap.GetComponentInParent<Grid>();
        
        var cellCoord = grid.WorldToCell(putPos);
        var cubeCoord = Util.UnityCellToCube(cellCoord);
        
        var random = Random.Range(0, Map.Instance.gameConfig.BlockNumber);
        var specialBlock = Instantiate(specialTwo[random], putPos, Quaternion.identity);
        specialBlock.transform.SetParent(blockBase);

        specialBlock.specialValue = (int)gameConfig.SpecialBlock2Condition;
        specialBlock.Coord = cubeCoord;
        Map.Instance.BlockPlace[cubeCoord] = specialBlock;
        
        for (int i = 0; i < specialBlock.dir.Count; i++)
        {
            specialBlock.dir[i] = null;
        }

        specialBlock.foot = null;
        
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
        newBlocks = new List<Block>();
        newBlocksPos = new List<Vector3>();
        var grid = Map.Instance.tilemap.GetComponentInParent<Grid>();
        for (int i = 0; i < Map.Instance.newBlockSpawnPos.Count; i++)
        {
            var howManyNeedToSpawn = Map.Instance.CountNullPlace(Map.Instance.newBlockSpawnPos[i]);
            Debug.Log(i + "번 줄은 " + howManyNeedToSpawn + "개 생성해야됨");
            for (int j = 0; j < howManyNeedToSpawn; j++)
            {
                var cellCoord = grid.CellToWorld(Util.CubeToUnityCell(Map.Instance.newBlockSpawnPos[i] + new Vector3Int(0,-1,1) * j));
                Debug.Log(cellCoord);
                var random = Random.Range(0, Map.Instance.gameConfig.BlockNumber);
                var block = Instantiate(allKindsOfBlock[random], cellCoord, Quaternion.identity);
                
                newBlocks.Add(block);
                newBlocksPos.Add(grid.CellToWorld(Util.CubeToUnityCell(Map.Instance.newBlockSpawnPos[i] + new Vector3Int(0,1,-1) * (howManyNeedToSpawn - j))));
                
                block.transform.SetParent(blockBase);
            }
        }
    }
    public void CheckTarget()
    {
        notNewBlocks = new List<Block>();
        notNewBlocksPos = new List<Vector3>();
        
        var grid = Map.Instance.tilemap.GetComponentInParent<Grid>();
        //var keys = Map.Instance.BlockPlace.Keys;
        
        Map.Instance.BlockPlace.Keys.ForEach(key =>
        {
            if (Map.Instance.BlockPlace[key] != null)
            {
                var moveCount = Map.Instance.CountNullPlace(key);
                if (moveCount != 0)
                {
                    notNewBlocks.Add(Map.Instance.BlockPlace[key]);
                    notNewBlocksPos.Add(grid.CellToWorld(Util.CubeToUnityCell(key + (new Vector3Int(0, 1, -1) * moveCount))));
                }
            }
        });
    }

    public void MoveAllDown()
    {
        var grid = Map.Instance.tilemap.GetComponentInParent<Grid>();
        
        var sequenceAnim = DOTween.Sequence();
        
        for (int j = 0; j < notNewBlocks.Count; j++)
        {
            var pos = notNewBlocksPos[j];
            var posForMap = grid.WorldToCell(pos);
            sequenceAnim.Join(notNewBlocks[j].transform.DOMove(pos, gameConfig.AnimationSpeed).SetEase(gameConfig.EasyType));
            var blockCoord = Util.UnityCellToCube(posForMap);
            notNewBlocks[j].Coord = blockCoord;
            notNewBlocks[j].GetComponentInChildren<TextMeshPro>().text = blockCoord.ToString();
            Map.Instance.BlockPlace[blockCoord] = notNewBlocks[j];
        }

        for (int i = 0; i < newBlocks.Count; i++)
        {
            var pos = newBlocksPos[i];
            var posForMap = grid.WorldToCell(pos);
            sequenceAnim.Join(newBlocks[i].transform.DOMove(pos, gameConfig.AnimationSpeed).SetEase(gameConfig.EasyType));
            var blockCoord = Util.UnityCellToCube(posForMap);
            newBlocks[i].Coord = blockCoord;
            newBlocks[i].GetComponentInChildren<TextMeshPro>().text = blockCoord.ToString();
            Map.Instance.BlockPlace[blockCoord] = newBlocks[i];
        }

        moveCounter++;
        
        sequenceAnim.OnComplete(ChangeStatesForMove);
    }
    
    public void ChangeStatesForMove()
    {
        Debug.Log("움직임 끝남");
        Map.Instance.DrawDirectionOnBlock();
        GameManager.Instance.State = States.DeleteBlock;
    }

    
}