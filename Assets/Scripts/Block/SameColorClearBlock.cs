using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameColorClearBlock : SpecialBlock
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
        
        //같은 색깔 전체 삭제
        
        Destroy(gameObject);
        Debug.Log("일반 블럭");
    }
    
    
    
    
    
}
