using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    public BlockType BlockType { get; set; }

    public bool IsMovable;
    
    public bool IsMoving;
    
    public int ShapeValue;
    public List<GameObject> dir;
    public GameObject foot;
    public Vector3Int Coord;
    public int score;
    
    [SerializeField] public int value;

    protected Action OnPang;
    protected Action<MapTile> MoveBlock;
    
    public void Pang()
    {
        OnPang?.Invoke();
    }
    //이동 구현
    public void Move(MapTile mapTile)
    {
        MoveBlock?.Invoke(mapTile);
    }
    
}

