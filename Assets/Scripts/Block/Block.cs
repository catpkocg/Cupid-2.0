using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    
    public bool IsMovable;
    public bool IsMoving;
    
    public int drawValue;
    public List<GameObject> dir;
    public GameObject foot;
    public Vector3Int Coord;
    
    //점수계산이 필요한가?? 현재는 없음
    //public int score;
    
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

    public void PangMainBlock(Block block)
    {
        MapManager.Instance.map.MapTiles[block.Coord].MovableBlockOnMapTile = null;
        Destroy(block.gameObject);
    }
}

