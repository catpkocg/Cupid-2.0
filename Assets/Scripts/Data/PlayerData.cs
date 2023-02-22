using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Cupid/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int clearedMaxStage;
    public SerializeDictionary<int, float> stagesScore;
}