using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    
    [SerializeField] public int value;

    public int specialValue = 0;
    
    public List<GameObject> dir;
    // public GameObject dirX;
    // public GameObject dirY;
    // public GameObject dirZ;
    public GameObject foot;
    
    public Vector3Int Coord { get; set; }

    
    public int score = 1;
    
    public abstract void Pang();
}


class ColorBlock : Block
{
    public override void Pang()
    {
        Debug.Log("팡팡");
    }
}

class SpecialBlock : Block
{
    public override void Pang()
    {
        Debug.Log("스페셜 블럭 팡팡");
    }
}
