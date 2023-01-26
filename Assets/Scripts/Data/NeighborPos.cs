using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeighborPos", menuName = "HexAround/NeighborPos", order = 1)]
public class NeighborPos : ScriptableObject
{
    public List<Around> neighbor = new ();
}

[Serializable]
public class Around
{
    public Vector3Int neighborPos;
}