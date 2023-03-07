using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Wayway.Engine;

public class IceBlocker : Block
{
    [SerializeField] private int colorValue;
    [SerializeField] private int iceCounter;
    private void Awake()
    {
        OnPang += Pang;
    }

    private void OnDisable()
    {
        OnPang -= Pang;
    }

    public override void Pang()
    {
        PangAnimation();
        
    }
    
}