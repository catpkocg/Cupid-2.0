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
    [SerializeField] private int lineClearBlockCondition;
    [SerializeField] private int sameColorClearBlockCondition;
    [SerializeField] private Ease easyType;
    
    //[SerializeField] private int addMaxCount = 30;
    //[SerializeField] private int blockCount = 3;
    
    public int StageLevel => stageLevel;
    public int BlockNumber => blockNumber;
    public float AnimationSpeed => animationSpeed;
    public float LineClearBlockCondition => lineClearBlockCondition;
    public float SameColorClearBlockCondition => sameColorClearBlockCondition;
    public Ease EasyType => easyType;
    
    //public int AddMaxCount => addMaxCount;
    //public int BlockCount => blockCount;

}
