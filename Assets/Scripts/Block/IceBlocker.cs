using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Wayway.Engine;

public class IceBlocker : Block
{
    
    [SerializeField] private int iceCounter;
    [SerializeField] private List<Sprite> iceSprites;
    
    public int colorValue;
    
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
        CheckIceCondition();
    }
    
    private void CheckIceCondition()
    {
        switch (iceCounter)
        {
            case 1 :
                //팡시키면 됨
                PangAnimation();
                //체크 카운터에 넣음
                break;
            case 2:
                //boxCounter - 1
                iceCounter--;
                //스프라이트 변경 1로
                GetComponent<SpriteRenderer>().sprite = iceSprites[0];
                //애니메이션 효과
                DeleteOneIceAnimation();
                break;
            case 3:
                //boxCounter - 1
                iceCounter--;
                //스프라이트 변경 2로
                GetComponent<SpriteRenderer>().sprite = iceSprites[1];
                //애니메이션 효과
                DeleteOneIceAnimation();
                break;
            case 4:
                //boxCounter - 1
                iceCounter--;
                //스프라이트 변경 3로
                GetComponent<SpriteRenderer>().sprite = iceSprites[2];
                //애니메이션 효과
                DeleteOneIceAnimation();
                break;
        }
    }
    
    private void DeleteOneIceAnimation()
    {
        var block = this;
        var gameConfig = GameManager.Instance.gameConfig;
        block.transform.DOScale(Vector3.one * 0.8f, 0.1f)
            .SetEase(gameConfig.EasyType)
            .SetLoops(2, LoopType.Yoyo);
    }
    
}