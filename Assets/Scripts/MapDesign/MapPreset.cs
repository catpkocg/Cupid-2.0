using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class MapPreset : MonoBehaviour
{
    public MapLayer BackgroundMap;
    public List<MapLayer> MapLayerList;
    public List<Vector3Int> MapBounds = new ();
    public Dictionary<Vector3Int, Block> MovableBlocks = new ();
    public Dictionary<Vector3Int, Block> UnMovableBlocks = new();

    [SerializeField] private int setStageNumber;
    [SerializeField] private MapDesignSpawn spawn;
    [SerializeField] private GameObject mapContainer;
    
    [Button(ButtonSizes.Gigantic)]
    private void CreateMapPrefab()
    {
        spawn.SettingForMapPrefabs();
        var Map = mapContainer;
        PrefabUtility.SaveAsPrefabAsset(Map, "Assets/Prefabs/Maps/Map"+setStageNumber+".prefab");
        MapBounds.Clear();
        MovableBlocks.Clear();
        UnMovableBlocks.Clear();
        for (var i = spawn.blockCase.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(spawn.blockCase.transform.GetChild(i).gameObject);
        }
    }
}
