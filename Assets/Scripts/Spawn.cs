using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private List<Block> allKindsOfBlock;
    [SerializeField] private Transform blockBase;
    
    private void SpawnBlockOnTile()
    {
        
        var grid = Map.Instance.tilemap.GetComponentInParent<Grid>();
        
        for (int i = 0; i < Map.Instance.canSpwanPlace.Count; i++)
        {
            var cellCoord = grid.WorldToCell(Map.Instance.canSpwanPlace[i]);
            var cubeCoord = UnityCellToCube(cellCoord);
            var random = Random.Range(0, Map.Instance.gameConfig.BlockNumber);
            var block = Instantiate(allKindsOfBlock[random], Map.Instance.canSpwanPlace[i], Quaternion.identity);
            block.transform.SetParent(blockBase);
            block.GetComponentInChildren<TextMeshPro>().text = cubeCoord.ToString();

        }
    }
    void Start()
    {
        SpawnBlockOnTile();
    }

    private Vector3Int UnityCellToCube(Vector3Int cell)
    {
        var yCell = cell.x; 
        var xCell = cell.y;
        var x = yCell - (xCell - (xCell & 1)) / 2;
        var z = xCell;
        var y = -x - z;
        return new Vector3Int(x, y, z);
    }
}
