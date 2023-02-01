#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;



public class MapPreset : MonoBehaviour
{
    public MapLayer BackgroundMap;
    public List<MapLayer> MapLayerList = new();
    public int StageNumber = 0;
    
    
    private string prefabLocation = "Assets/Prefabs";
    
    
    [Button(ButtonSizes.Gigantic)]
    private void CreateMapPrefab()
    {
        var test = gameObject;
        
        PrefabUtility.SaveAsPrefabAsset(test, "Assets/Prefabs/Prefabs.prefab");
        // AssetDatabase.CreateAsset(test, "Assets/Prefabs.asset");
    }
}


#endif