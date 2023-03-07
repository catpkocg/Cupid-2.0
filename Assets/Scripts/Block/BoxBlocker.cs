using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoxBlocker : Block
{
    
    [SerializeField] private int boxCounter;
    [SerializeField] private List<Sprite> boxSprites;

    public int colorValue;
    public bool alreadyPang = false;
    
    private void Awake()
    {
        OnPang += Pang; 
        MoveBlock += Move;
    }

    private void OnDisable()
    {
        OnPang -= Pang;
        MoveBlock += Move;
    }

    public override void Pang()
    {
        if (alreadyPang == false)
        {
            CheckBoxCondition();
            alreadyPang = true;
        }
        
    }

    public override void Move(MapTile mapTile)
    {
        IsMoving = true;
        var gameConfig = GameManager.Instance.gameConfig;
        mapTile.MovableBlockOnMapTile = this;
        mapTile.MovableBlockOnMapTile.Coord = mapTile.MapTileCoord;
        
        MoveAnimation(mapTile.transform.position);
    }

    private void CheckBoxCondition()
    {
        switch (boxCounter)
        {
            case 1 :
                //팡시키면 됨
                PangAnimation();
                //체크 카운터에 넣음
                break;
            case 2:
                //boxCounter - 1
                boxCounter--;
                //스프라이트 변경 1로
                GetComponent<SpriteRenderer>().sprite = boxSprites[0];
                //애니메이션 효과
                DeleteOneBoxAnimation();
                break;
            case 3:
                //boxCounter - 1
                boxCounter--;
                //스프라이트 변경 2로
                GetComponent<SpriteRenderer>().sprite = boxSprites[1];
                //애니메이션 효과
                DeleteOneBoxAnimation();
                break;
            case 4:
                //boxCounter - 1
                boxCounter--;
                //스프라이트 변경 3로
                GetComponent<SpriteRenderer>().sprite = boxSprites[2];
                //애니메이션 효과
                DeleteOneBoxAnimation();
                break;
        }
    }

    private void DeleteOneBoxAnimation()
    {
        var block = this;
        var gameConfig = GameManager.Instance.gameConfig;
        block.transform.DOScale(Vector3.one * 0.8f, 0.1f)
            .SetEase(gameConfig.EasyType)
            .SetLoops(2, LoopType.Yoyo);
    }
    
}