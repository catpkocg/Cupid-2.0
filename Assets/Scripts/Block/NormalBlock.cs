using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBlock : Block
{
    private void Awake()
    {
        OnPang += Pang;
    }

    private void OnDisable()
    {
        OnPang -= Pang;
    }

    private void Pang()
    {
        Destroy(gameObject);
        Debug.Log("일반 블럭");
    }
}
