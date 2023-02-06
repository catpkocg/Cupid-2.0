using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    
    public BlockType BlockType { get; set; }
    
    [SerializeField] public int value;

    public int specialValue = 0;
    
    public List<GameObject> dir;
    
    public GameObject foot;
    
    public Vector3Int Coord { get; set; }

    
    public int score = 1;
    
    public abstract void Pang();
    
    
    
}

