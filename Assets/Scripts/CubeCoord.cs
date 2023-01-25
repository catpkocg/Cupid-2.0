using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCoord : MonoBehaviour
{
    
    private List<Vector3Int> GetNeighbors(Vector3Int unityCell, int range)
    {
        var centerCubePos = UnityCellToCube(unityCell);

        var result = new List<Vector3Int>();
    
        int min = -range, max = range;

        for (int x = min; x <= max; x++)
        {
            for (int y = min; y <= max; y++)
            {
                var z = -x - y;
                if (z < min || z > max)
                {
                    continue;
                }

                var cubePosOffset = new Vector3Int(x, y, z);
                result.Add(CubeToUnityCell(centerCubePos + cubePosOffset));
            }

        }

        return result;
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
    private Vector3Int CubeToUnityCell(Vector3Int cube)
    {
        var x = cube.x;
        var z = cube.z;
        var col = x + (z - (z & 1)) / 2;
        var row = z;

        return new Vector3Int(col, row,  0);
    }
}

