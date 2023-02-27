using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Cupid/Game Config")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private int stageLevel;
    [SerializeField] private int blockNumber;
    [SerializeField] private float moveAnimationDelay = 0.3f;
    [SerializeField] private float animationSpeed;
    [SerializeField] private int lineClearBlockCondition;
    [SerializeField] private int sameColorClearBlockCondition;
    [SerializeField] private float yScaleOnMove = 0.8f;
    [SerializeField] private float xScaleOnMove = 1.1f;
    [SerializeField] private float scaleAnimDuration = 0.1f;
    [SerializeField] private Ease easyType;
    [SerializeField] private Animator explosion;
    
    //[SerializeField] private int addMaxCount = 30;
    //[SerializeField] private int blockCount = 3;
    
    public int StageLevel
    {
        get => stageLevel;
        set => stageLevel = value;
    }

    public int BlockNumber => blockNumber;
    public float MoveAnimationDelay => moveAnimationDelay;
    public float AnimationSpeed => animationSpeed;
    public int LineClearBlockCondition => lineClearBlockCondition;
    public int SameColorClearBlockCondition => sameColorClearBlockCondition;
    public float YScaleOnMove => yScaleOnMove;
    public float XScaleOnMove => xScaleOnMove;
    public float ScaleAnimDuration => scaleAnimDuration;
    public Ease EasyType => easyType;
    public Animator Explosion => explosion;

    //public int AddMaxCount => addMaxCount;
    //public int BlockCount => blockCount;

}
