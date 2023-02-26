using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Wayway.Engine.Singleton;

public class Map : MonoBehaviour
{
    public GameConfig GameConfig;
    public List<SpawnPos> SpawnPlace = new ();
    public Dictionary<Vector3Int, MapTile> MapTiles = new ();
    public List<MapTilePresetData> MapTilePresetDataList = new();
    public List<ClearCondition> ClearConditionData = new();
    public int BlockNumber;
    public int PerfectScore;
    public int MoveLimit;
}

[Serializable]
public class MapTilePresetData
{
    [HorizontalGroup("MapTilePresetData"), HideLabel] public Vector3Int Coord;
    [HorizontalGroup("MapTilePresetData"), HideLabel] public MapTile MapTile;

    public MapTilePresetData(Vector3Int coord, MapTile mapTile)
    {
        Coord = coord;
        MapTile = mapTile;
    }
} 
