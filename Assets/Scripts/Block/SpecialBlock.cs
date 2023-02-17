using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBlock : Block
{

    public Line line;
    
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
        switch (line)
        {
            case Line.xLine:
                //x축 전체 삭제
                break;
            case Line.yLine:
                //y축 전체 삭제
                break;
            case Line.zLine:
                //z축 전체 삭제
                break;
        }
        
        Destroy(gameObject);
        Debug.Log("일반 블럭");
    }
}

public enum Line
{
    None = 0,
    xLine,
    yLine,
    zLine,
}
