using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Cupid/ConditionImage")]
public class ConditionImage : ScriptableObject
{
    public SerializeDictionary<ClearConditionBlock, Sprite> ImagesForUI;
}