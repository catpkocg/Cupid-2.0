using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoordUtil
{
    public static bool GetAxisValue(Vector3Int target, Vector3Int current, int line)
    {
        return line switch
        {
            0 => target.x == current.x,
            1 => target.y == current.y,
            2 => target.z == current.z,
            _ => false
        };
    }
    
    public static Vector3Int UnityCellToCube(Vector3Int cell)
    {
        
        var yCell = cell.x; 
        var xCell = cell.y;
        var z = yCell - (xCell - (xCell & 1)) / 2;
        var x = xCell;
        var y = -x - z;
        return new Vector3Int(x, y, z);
    }
    
    public static Vector3Int CubeToUnityCell(Vector3Int cube)
    {
        var x = cube.z;
        var z = cube.x;
        var col = x + (z - (z & 1)) / 2;
        var row = z;

        return new Vector3Int( col,row,  0);
    }

    // public static List<Vector3Int> GetNeighbors(Vector3Int unityCell, int range)
    // {
    //     var centerCubePos = UnityCellToCube(unityCell);
    //
    //     var result = new List<Vector3Int>();
    //
    //     int min = -range, max = range;
    //
    //     for (int x = min; x <= max; x++)
    //     {
    //         for (int y = min; y <= max; y++)
    //         {
    //             var z = -x - y;
    //             if (z < min || z > max)
    //             {
    //                 continue;
    //             }
    //
    //             var cubePosOffset = new Vector3Int(x, y, z);
    //             result.Add(CubeToUnityCell(centerCubePos + cubePosOffset));
    //         }
    //
    //     }
    //
    //     return result;
    // }
    
}

