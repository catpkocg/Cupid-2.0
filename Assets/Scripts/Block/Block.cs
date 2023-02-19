using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    public BlockType BlockType { get; set; }

    public bool IsMovable;
    
    public int ShapeValue;
    public List<GameObject> dir;
    public GameObject foot;
    public Vector3Int Coord;
    public int score;
    
    [SerializeField] public int value;

    protected Action OnPang;
    protected Action<Block, MapTile> MoveBlock;
    
    public void Pang()
    {
        OnPang?.Invoke();
    }
    //이동 구현
    public void Move(Block block, MapTile mapTile)
    {
        MoveBlock?.Invoke(block, mapTile);
    }
    
    
}

