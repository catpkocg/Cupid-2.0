using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    public BlockType BlockType { get; set; }
    public int ShapeValue;
    public List<GameObject> dir;
    public GameObject foot;
    public Vector3Int Coord;
    public int score;
    
    [SerializeField] public int value;
    
    
    public abstract void Pang();
    //이동 구현
    
    
}
