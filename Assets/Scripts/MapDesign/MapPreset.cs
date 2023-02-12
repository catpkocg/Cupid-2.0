using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapPreset : MonoBehaviour
{
    public MapLayer BackgroundMap;
    public MapLayer SpawnPlace;
    public List<MapLayer> MapLayerList;

    public List<Block> movableBlocks;
    public List<Block> unmovableBlocks;

    private Dictionary<Vector3Int, List<Block>> BlockPlace = new ();
    
    
    private void ChangeToDictionary(List<Block> blocks)
    {
        BlockPlace = new Dictionary<Vector3Int, List<Block>>();
        for (var i = 0; i < blocks.Count; i++)
        {
            BlockPlace.Add(blocks[i].Coord, CreateBlockList(blocks[i].Coord));
        }
    }

    private List<Block> CreateBlockList(Vector3Int coord)
    {
        var samePosBlocks = new List<Block>();
        foreach (var t in movableBlocks)
        {
            var movableBlockCoord = t.Coord;
            if (movableBlockCoord == coord)
            {
                samePosBlocks.Add(t);
            }
        }
        foreach (var t in unmovableBlocks)
        {
            var unmovableBlockCoord = t.Coord;
            if (unmovableBlockCoord == coord)
            {
                samePosBlocks.Add(t);
            }
        }
        return samePosBlocks;
    }
    
    // private void _ChangeToDictionary(List<Block> blocks)
    // {
    //     for (var i = 0; i < blocks.Count; i++)
    //     {
    //         if (!BlockPlace.ContainsKey(blocks[i].Coord)) 
    //             BlockPlace.Add(blocks[i].Coord, new List<Block>());
    //
    //         foreach (var t in movableBlocks) if (t.Coord == blocks[i].Coord) BlockPlace[t.Coord].Add(t);
    //         foreach (var t in unmovableBlocks) if (t.Coord == blocks[i].Coord) BlockPlace[t.Coord].Add(t);
    //     }
    // }
}





// c# 이벤트, 유니티 이벤트 

// Data 저장, 

// 한시스템을 이용해서 저장을 한다.
// 인게임 저장될때 사용 하는 시스템?

// UGS 인게인 저장 x , 개발 과정에서 사용하는툴 (Json)

// 프로젝트당 하나만 사용한다.

// 외부 에셋 (Easy Save) - 좋다, 편하다.

