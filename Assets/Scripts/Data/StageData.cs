using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Cupid/StageData")]
public class StageData : ScriptableObject
{
    [SerializeField] private Map stageMap;
    [SerializeField] private List<ClearConditionBase> clearCondition;
    [SerializeField] private int moveLimit;

    public Map StageMap => stageMap;
    public int MoveLimit => moveLimit;
    
}