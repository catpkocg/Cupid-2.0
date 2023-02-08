using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDesignSpawn : MonoBehaviour
{
    [SerializeField] private Transform BackGround;
    
    [SerializeField] private MapPreset mapPreset;
    
    [SerializeField] private List<Block> normalBlocks;
    [SerializeField] private List<Blocker> blockers;


    public void SpawnOnTileMap()
    {
        
        var a = Instantiate(normalBlocks[0], new Vector3(0, 0, 0), Quaternion.identity);
        var b= Instantiate(blockers[0], new Vector3(1, 1, 0), Quaternion.identity);
        
        mapPreset.allBlocks.Add(new Vector3Int(0,0,0),a);
        mapPreset.allBlocks .Add(new Vector3Int(1,1,0),b);
        
        
        
        
    }

    

}
