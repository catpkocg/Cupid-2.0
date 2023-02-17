using System;
using UnityEngine;

public abstract class ClearConditionBase : ScriptableObject
{
    public bool IsCleared = false;

    public Action OnCleared;

    public abstract void CheckBlockPang(Block block);

    // 터질때 마다 터진놈의 데이터를 열람하고 내 컨디션이 현재 어디까지 왔는지 판단 또는 저장
    // 컨디션이 완료됨을 저장
}