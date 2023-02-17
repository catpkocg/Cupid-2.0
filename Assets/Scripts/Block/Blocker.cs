using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : Block
{
    public Vector3Int CubeCoord;
    public Vector3 BlockerPos;
    
    public void Pang()
    {
        Debug.Log("blocker pang");
    }
}

public enum BlockerType
{
    None = 0,
    BoxBlocker,
    IceBlocker,
}

