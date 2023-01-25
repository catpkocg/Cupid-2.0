using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cupid/Game Config")]
public class GameConfig : ScriptableObject
{
    
    [SerializeField] private int stageLevel = 1;
    //[SerializeField] private int addMaxCount = 30;
    //[SerializeField] private int blockCount = 3;
    
    public int StageLevel => stageLevel;
    
    //public int AddMaxCount => addMaxCount;
    //public int BlockCount => blockCount;
    
}
