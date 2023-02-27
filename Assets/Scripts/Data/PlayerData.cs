using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Cupid/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int clearedMaxStage = 0;
    public List<StageInformation> stagesInformation;

    //public SerializeDictionary<int, float> StageInformation;
}

[Serializable]
public class StageInformation
{
    [HorizontalGroup("ClearCondition"), HideLabel] public int StageLevel;
    [HorizontalGroup("ClearCondition"), HideLabel] public int StageScore = 0;

    public StageInformation(int stageLevel, int stageScore)
    {
        StageLevel = stageLevel;
        StageScore = stageScore;
    }
}



