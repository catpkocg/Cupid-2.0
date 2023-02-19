using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Wayway.Engine.Singleton;

public class Map : MonoSingleton<Map>
{
    public GameConfig GameConfig;
    public List<Vector3Int> SpawnPlace = new ();
    public List<MapTilePresetData> MapTilePresetDataList = new();
    // public List<Vector3Int> MapTileKey = new List<Vector3Int>();
    // public List<MapTile> MapTileValue = new List<MapTile>();
    public Dictionary<Vector3Int, MapTile> MapTiles = new ();

    [SerializeField] private Spawn spawn;
    [SerializeField] private NeighborPos neighborPos;

    private List<Block> allBlockForCheckDir = new();

    private int CalCulateMaxAndMinDif(List<int> allNum)
    {
        var min = allNum.Min();
        var max = allNum.Max();

        var amount = max - min;

        return amount;
    }

    private int CalCulateAverage(List<int> allNum)
    {
        int Sum = allNum.Sum();
        int Average = Sum / allNum.Count;

        return Average;
    }
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
