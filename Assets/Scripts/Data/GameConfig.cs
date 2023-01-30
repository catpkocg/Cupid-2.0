using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
[CreateAssetMenu(menuName = "Cupid/Game Config")]
public class GameConfig : ScriptableObject
{
    
    [SerializeField] private int stageLevel;
    [SerializeField] private int blockNumber;

    [SerializeField] private float animationSpeed;
    [SerializeField] private int specialBlock1Condition;
    [SerializeField] private int specialBlock2Condition;

    [SerializeField] private Ease easyType;
    
    //[SerializeField] private int addMaxCount = 30;
    //[SerializeField] private int blockCount = 3;
    
    public int StageLevel => stageLevel;
    public int BlockNumber => blockNumber;

    public float AnimationSpeed => animationSpeed;
    public float SpecialBlock1Condition => specialBlock1Condition;
    public float SpecialBlock2Condition => specialBlock2Condition;

    public Ease EasyType => easyType;
    //public int AddMaxCount => addMaxCount;
    //public int BlockCount => blockCount;

}
